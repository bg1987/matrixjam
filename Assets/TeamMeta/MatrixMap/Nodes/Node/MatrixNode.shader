Shader "Unlit/MatrixNode"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Color1 ("Color1", Color) = (0.12,0.12,0.5)
        _ColorIntensity1 ("ColorIntensity1", float) = 1
        _Color2 ("Color2", Color) = (0.1,0.7,0.55)
        _ColorIntensity2 ("ColorIntensity2", float) = 1

        _MainTex ("Texture", 2D) = "white" {}
        _Radius ("Radius", Range(0,0.5)) = 0.5
        _P ("P", float) = 1.94
        _Z ("Z", float) = 3
        _Seed ("Seed", float) = 1

        _OutlineColor ("OutlineColor", Color) = (0.1,0.7,0.55)
        _OutlineColorIntensity ("OutlineColorIntensity", float) = 1
        _OutlineWidth ("OutlineWidth", range(0,0.5)) = 0.5

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back 
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag


            #include "UnityCG.cginc"

            struct MeshData
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Radius;
            float _P;
            float _Z;
            float _Seed;
            float4 _Color;
            float4 _Color1;
            float4 _Color2;
            float _ColorIntensity1;
            float _ColorIntensity2;

            float4 _OutlineColor;
            float _OutlineColorIntensity;
            float _OutlineWidth;

            float _MatrixMapTime;

            float2x2 mm2(float a)
            {
                float c = cos(a), s = sin(a);
                return float2x2(c,-s,s,c);
            }
            v2f vert (MeshData v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                //o.uv = v.uv;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            float CalculateSphereMask(float2 uv,float radius)
            {

                float2 origin = float2(0.5,0.5);
                
                
                return distance(origin,uv)<radius;
            }
            float2 SphereIntersection(float3 spherePos, float3 ro, float3 rd)
            {
                // float3 roToSphere = spherePos - rd;
                float3 roToSphere = spherePos - ro;

                // float distance =length(roToSphere);
                float radius = 0.5;
                float3 closestRayPointToSphereCenter =dot(roToSphere,rd)*rd;
                
                float closestRayPointToSphereCenterSide = distance(closestRayPointToSphereCenter, roToSphere);
                float tSide = pow(radius,2)-pow(closestRayPointToSphereCenterSide,2);
                tSide = sqrt(tSide);
                float3 firstIntersection = rd * (length(closestRayPointToSphereCenter) - tSide)+ro;
                float3 secondIntersection = rd * (length(closestRayPointToSphereCenter) + tSide)+ro;

                float2 intersectionDistances;
                intersectionDistances.x = length(closestRayPointToSphereCenter) - tSide;
                intersectionDistances.y = length(secondIntersection);

                // return step(distance,0.6);
                if (distance(closestRayPointToSphereCenter, roToSphere)>radius)
                    return float2(0,0);
                else
                    return float2(1,1);




                // return secondIntersection.zz;

                return intersectionDistances.xy;
                return smoothstep(0.0,0.5,tSide);
                return step(distance(closestRayPointToSphereCenter+roToSphere, (normalize(roToSphere)*radius))+roToSphere,9.49);
                // return distance;

            }
            float2 iSphere2(float3 ro, float3 rd)
            {
                float b = dot(ro, rd);
                float c = length(ro)*length(ro) - 1.0;
                float h = b*b - c;
                if(h <0.0)
                    return float2(-1.0,-1.0);
                else 
                    return float2((-b - sqrt(h)), (-b + sqrt(h)));


            }
            float Hash(float3 p)  // replace this by something better
            {
                p  = frac( p*0.3183099+0.1 );
                p *= 17.0;
                return frac( p.x*p.y*p.z*(p.x+p.y+p.z) );
            }

            float ProNoise( float3 x )
            {
                float3 i = floor(x);
                float3 f = frac(x);
                f = f*f*(3.0-2.0*f);
                
                return lerp(lerp(lerp( Hash(i+float3(0,0,0)), 
                                    Hash(i+float3(1,0,0)),f.x),
                            lerp( Hash(i+float3(0,1,0)), 
                                    Hash(i+float3(1,1,0)),f.x),f.y),
                        lerp(lerp( Hash(i+float3(0,0,1)), 
                                    Hash(i+float3(1,0,1)),f.x),
                            lerp( Hash(i+float3(0,1,1)), 
                                    Hash(i+float3(1,1,1)),f.x),f.y),f.z);
            }
            float flow(float3 p, float t)
            {
                float z=2.;
                float rz = 0.;
                float3 bp = p;
                // noiseTexture
                for (float i= 1;i < 5.;i++ )
                {
                    // p += t*0.1;
                    //float2 noiseValue = tex2D( _MainTex,_SinTime.w*p.xz+199.5 ).yx;
                    //float2 noiseUV = float2((cos(t+p.y)+1)/2,(sin(t+p.x)+1)/2); Not a very exciting look
                    float2 noiseUV = (cos((t+p.x))+1)/2;
                    float2 noiseValue = tex2D( _MainTex,noiseUV ).yx;

                    rz+= (sin((ProNoise(p+noiseValue.xyx+t*0.8))*6.0)*0.5+0.5) /z;
                    p = lerp(bp,p,0.6);
                    z *= _Z;
                    p *= _P;
                    //p*= m3;
                }
                return rz;	
            }
            float CalculateOutlineMask(float2 uv){
                float outlineMask = 1-CalculateSphereMask(uv, _OutlineWidth);
                float radiusMask = CalculateSphereMask(uv, _Radius);
                outlineMask = outlineMask && radiusMask;
                return outlineMask;
            }
            float4 frag (v2f i) : SV_Target
            {
                
                // sample the texture
                fixed4 color = tex2D(_MainTex, i.uv);

                _Color1 = _Color1 * pow(2,_ColorIntensity1);
                _Color2 = _Color2 * pow(2,_ColorIntensity2);

                bool isSphere = CalculateSphereMask(i.uv,_Radius);
                clip(isSphere-0.1);
                float3 origin = float3(0.5,0.5,0);

                float2 inputPoint = float2(i.uv.xy)-origin;

                float3 ro = float3(0, 0, -5);

                float3 rd = float3(inputPoint- ro ,_Radius*-(ro.z+0.1));
                rd = normalize(rd);
                float t = _MatrixMapTime+100;
                // t = _Time.y;
                t+=_Seed*24.35;

                float2x2 mx = mm2(t*0.4);
                float2x2 my = mm2(t*0.3); 

                ro.xz = mul(mx,ro.xz);
                rd.xz = mul(mx,rd.xz);
                ro.xy = mul(my,ro.xy);
                rd.xy = mul(my,rd.xy);

                float3 col = float3(0.0125,0.,0.025);

                float2 sph = iSphere2(ro,rd);

                if (sph.x > 0.)
                {

                    float3 pos = ro+rd*sph.x;
                    float3 pos2 = ro+rd*sph.y;

                    float3 rf = reflect( rd, pos);
                    float3 rf2 = reflect( rd, pos2);

                    float nz = (-log(abs(flow(rf*1.2,t*0.3)-0.01)));
                    float nz2 = (-log(abs(flow(rf2*1.2,-t*1.1)-0.01)));

                    col += (0.1*nz*nz* _Color1 + 0.05*nz2*nz2*_Color2)*0.8;

                    float len = lerp(0,1, length(col)*2);
                    len =saturate(len);
                    col.rgb *= _Color.rgb;
                    float alphaMask = len>= (1-_Color.a);
                    float alphaGradient = len*_Color.a;
                    // if(alpha<0)
                    //     alpha = 0;
                    float alpha = alphaGradient*alphaMask;

                    float outlineMask = CalculateOutlineMask(i.uv);
                    float4 outlineColor = _OutlineColor * pow(2,_OutlineColorIntensity);
                    float4 _Outline =float4(outlineMask*outlineColor.rgb,_OutlineColor.a*alpha);
                    if(outlineMask)
                        return _Outline;


                    return float4(col.rgb,alpha);
                }
                else
                    return float4(0,0,0,0);
            }
            ENDCG
        }
    }
}

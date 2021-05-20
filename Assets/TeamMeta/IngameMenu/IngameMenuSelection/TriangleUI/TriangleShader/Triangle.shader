Shader "Unlit/Triangle"
{
    Properties
    {
        _Scale ("Scale(Script)", vector) = (1,1,1,1)
        _Color("Color", Color) = (0,1,0,1)
        _BorderColor("BorderColor", Color) = (0,1,0,1)
        _BlubBorderColor("BlubBorderColor", Color) = (0,1,0,1)
        _MainTex ("Texture", 2D) = "white" {}
        _A ("A", vector) = (0,0,0,0)
        _B ("B", vector) = (0,1,0,0)
        _C ("C", vector) = (1,0,0,0)
        _BlubBorderWidth ("BlubBorderWidth", float) = 0.1
        _BorderWidth ("_BorderWidth", float) = 0.1
        _SizeOffset ("_SizeOffset", float) = 0.01
        _TriangleSmooth ("TriangleSmooth", Range(0,1)) = 0.995
        _TriangleSmoothBlubBorderOffset ("TriangleSmoothBlubBorderOffset", Range(0,1)) = 0.03

        _BlubBorderWidthHighValueDuctTapeFixForTriangleInCenterArtifact ("BlubBorderWidthHighValueDuctTapeFixForTriangleInCenterArtifact", float) = 0.55
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
                float4 color : COLOR;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float4 color : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _BorderColor;
            float4 _BlubBorderColor;
            float2 _A;
            float2 _B;
            float2 _C;
            float _BlubBorderWidth;
            float _BorderWidth;
            float _SizeOffset;
            float _TriangleSmooth;
            float _TriangleSmoothBlubBorderOffset;
            float3 _Scale;

            float _BlubBorderWidthHighValueDuctTapeFixForTriangleInCenterArtifact;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.color = v.color;

                return o;
            }
            float sign (float2 p1, float2 p2, float2 p3)
            {
                return (p1.x - p3.x) * (p2.y - p3.y) - (p2.x - p3.x) * (p1.y - p3.y);
            }

            float sdTriangle(float2 p, float2 a, float2 b, float2 c) {
                float2 ba = b - a, cb = c - b, ac = a - c;
                float2 pa = p - a, pb = p - b, pc = p - c;

                // Calculate barycentric areas (simplified for usage):
                float abp = abs(ba.x * pa.y - ba.y * pa.x);
                float bcp = abs(cb.x * pb.y - cb.y * pb.x);
                float cap = abs(ac.x * pc.y - ac.y * pc.x);
                float abc = abs(ba.x * cb.y - ba.y * cb.x);

                // Distances to the edges (simplified to defer the square root):
                float2 ae = pa - ba * clamp(dot(pa, ba) / dot(ba, ba), 0.0, 1.0);
                float2 be = pb - cb * clamp(dot(pb, cb) / dot(cb, cb), 0.0, 1.0);
                float2 ce = pc - ac * clamp(dot(pc, ac) / dot(ac, ac), 0.0, 1.0);

                // Minimum of the edge distances:
                float tri = sqrt(min(dot(ae, ae), min(dot(be, be), dot(ce, ce))));

                // Use barycentric areas to determine the sign (is the point inside or outside?):
                tri *= abs(abp + bcp + cap - abc) < 0.001 ? -1.0 : 1.0;

                return tri;
            }
            float df_line(float2 p, float2 a, float2 b)
            {
                float2 pa = p - a, ba = b - a;
                float h = clamp(dot(pa,ba) / dot(ba,ba), 0., 1.);	
                return length(pa - ba * h);
            }

            float sharpen(in float d, in float w)
            {
                float e = 1. / 3.2;
                return 1. - smoothstep(-e, e, d - w);
            }
            float df_bounds(float2 uv, float2 a, float2 b, float2 c)
            {
                float cp = 0.;

                float l0 = sharpen(df_line(uv, a, b), _BlubBorderWidth);
                float l1 = sharpen(df_line(uv, b, c), _BlubBorderWidth);
                float l2 = sharpen(df_line(uv, c, a), _BlubBorderWidth);

                cp = max(max(l0, l1),l2);

                return cp;
            }
            
            float BlubBorderMask(float2 p, float2 a, float2 b, float2 c){
      
                float blubBorderMask =  smoothstep(0.0,0.02, df_bounds(p, a, b, c)-0.55);

                return blubBorderMask;
            }
            float TriangleMask(float2 p, float2 a, float2 b, float2 c){
                // float scale = 2+((_Scale.x+_Scale.y)/2);
                // float scaleSomething = scale * scale * (3 - 2 * scale);
                // float mask = smoothstep(_TriangleSmooth+1/scaleSomething,1, 1-sdTriangle(p,a,b,c));
                float mask = smoothstep(_TriangleSmooth,1, 1-sdTriangle(p,a,b,c));

                return mask;
            }
            float2 CalculateTriangleCenter(float2 a, float2 b, float2 c){
                float2 center;
                center.x =(a.x+b.x+c.x)/3;
                center.y =(a.y+b.y+c.y)/3;

                return center;
            }
            float InverseLerp(float from, float to, float value){
                return (value - from) / (to - from);
            }
            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;

                float2 p = i.uv;

                _TriangleSmooth -= lerp(0,_TriangleSmoothBlubBorderOffset, _BlubBorderColor.a);
                // _TriangleSmooth -= 0.03;
                //Needs a tiny size offset to not have jagged lines
                float2 center = CalculateTriangleCenter(_A,_B,_C);

                _A*=_SizeOffset;
                _A += (center)*(1-_SizeOffset);
                _B*=_SizeOffset;
                _B += (center)*(1-_SizeOffset);
                _C*=_SizeOffset;
                _C += (center)*(1-_SizeOffset);

                float2 blubTriangleA = _A;
                float2 blubTriangleB = _B;
                float2 blubTriangleC = _C;

                blubTriangleA.x+=_BlubBorderWidth;
                blubTriangleA.y+=_BlubBorderWidth;
                blubTriangleB.y-=_BlubBorderWidth;
                blubTriangleC.x-=_BlubBorderWidth;
                blubTriangleC.y+=_BlubBorderWidth;

                // return blubBorderMask;
                float blubMask = TriangleMask(p,blubTriangleA,blubTriangleB,blubTriangleC);
                float blubBorderMask = BlubBorderMask(p,blubTriangleA,blubTriangleB,blubTriangleC);
                if(_BlubBorderColor.a<_BlubBorderWidthHighValueDuctTapeFixForTriangleInCenterArtifact)
                {
                    blubBorderMask*=InverseLerp( 0,_BlubBorderWidthHighValueDuctTapeFixForTriangleInCenterArtifact,_BlubBorderColor.a);
                }
                float blubBorderMaskPlus = BlubBorderMask(p,_A,_B,_C) ;
                // blubBorderMask = blubBorderMask || (blubMask &&!blubBorderMask);

                float overAllTriangleMask = TriangleMask(p,_A,_B,_C);
                // return overAllTriangleMask;
                float d = sdTriangle(p,blubTriangleA,blubTriangleB,blubTriangleC);
                // float forLerping =(blubBorderMask*(1-d));
                // _BorderColor= lerp(_BorderColor,_BlubBorderColor,forLerping);
                // _BorderWidth-=_BlubBorderWidth;
                // _BorderWidth=saturate(_BorderWidth);

                float borderDistanceMask =  lerp( float3(0,0,0), float3(1,1,1), 1.0-smoothstep(0,_BorderWidth,abs(d)/_BorderWidth) )*_BorderColor.a;

                float borderMask =  (overAllTriangleMask && borderDistanceMask)*borderDistanceMask;

                col = lerp(float4(0,0,0,0) ,col,blubMask);
                col = lerp(col,_BorderColor,borderMask);
                // col.a = borderMask>0.9;

                col = lerp(col,_BlubBorderColor,blubBorderMask);
                // return borderMask;
                col = lerp(col,float4(0,0,0,0),(blubBorderMaskPlus && borderMask)&& !blubBorderMask);

                return col*i.color;
            }
            ENDCG
        }
    }
}

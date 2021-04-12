Shader "Matrix/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Dissolve("Dissolve",range(0,1)) = 0
        _DissolveMap ("DissolveMap", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)
        
        //Edge 1
        [Toggle()] _UseEdgeColor("UseEdgeColor",float) = 1
        _EdgeColor1 ("EdgeColor1", Color) = (0,0,1,1)
        _EdgeColorIntensity1 ("EdgeColorIntensity1", float) = 0
        _EdgeWidth1 ("EdgeWidth1", Range(0,1)) = 0.1
        //Edge 2
        [Toggle()] _UseEdgeColor2("UseEdgeColor2",float) = 1
        _EdgeColor2 ("EdgeColor2", Color) = (0,0,1,1)
        _EdgeColorIntensity2 ("EdgeColorIntensity2", float) = 0
        _EdgeWidth2 ("EdgeWidth2", Range(0,1)) = 0.1

        _DissolveStrength ("DissolveStrength", float) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Always
        
        Blend SrcAlpha OneMinusSrcAlpha
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

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                float2 dissolveMapUV : TEXCOORD1;
            };

            sampler2D _DissolveMap;
            float4 _DissolveMap_ST;

            sampler2D _MainTex;
            float _Dissolve;
            
            float4 _Color;
            //Edge 1            
            bool _UseEdgeColor;
            float _EdgeWidth1;
            float4 _EdgeColor1;
            float _EdgeColorIntensity1;
            //Edge 2
            bool _UseEdgeColor2;
            float _EdgeWidth2;
            float4 _EdgeColor2;
            float _EdgeColorIntensity2;

            float _DissolveStrength;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.dissolveMapUV = TRANSFORM_TEX(v.uv, _DissolveMap);

                return o;
            }

            bool CalculateEdgeMask(float edgeWidth, float dissolveMap){
                float edgeStart = _Dissolve - edgeWidth*(1-_Dissolve);
                float edgeEnd = _Dissolve + edgeWidth*(_Dissolve);

                bool edgeMask = edgeStart<dissolveMap  && edgeEnd >dissolveMap;
                return edgeMask;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                col *= _Color;
                // return col;
                
                // return i.dissolveMapUV;
                float dissolveMap = tex2D(_DissolveMap, i.dissolveMapUV);
                dissolveMap = 1-dissolveMap;
                // float pi = 3.14159265359;
                // return dissolveMap>_Dissolve;

                bool dissolveMask = (dissolveMap<_Dissolve);
                // return float4(dissolveMask.xxx,1);
                // float t = (cos(2*pi * dissolveMap)+1)/2;
                clip(1-dissolveMask-0.001);
                // return float4(dissolveMask.xxx,1);
                _EdgeColor1.xyz = _EdgeColor1 * pow(2,_EdgeColorIntensity1);
                _EdgeColor2.xyz = _EdgeColor2 * pow(2,_EdgeColorIntensity2);
                // return float4(_EdgeColor2.xyz,1);

                bool edgeMask = CalculateEdgeMask(_EdgeWidth1, dissolveMap);
                bool edgeMask2 = CalculateEdgeMask(_EdgeWidth2, dissolveMap);

                edgeMask=edgeMask*_UseEdgeColor;
                edgeMask2=edgeMask2*_UseEdgeColor2;

                col=lerp(col,_EdgeColor1,edgeMask);
                col=lerp(col,_EdgeColor2,edgeMask*edgeMask2);

                // float alphaEdge = edgeMask*_EdgeColor1.a;
                // float alphaEdge2 = edgeMask2*_EdgeColor2.a;
                // float alpha = max(alphaEdge,alphaEdge2);
                // float alpha = max(alphaEdge,alphaEdge2);
                // alpha = max(alpha,col.a);
                float alpha = max(_EdgeColor1.a*edgeMask,_Color.w);
                // alpha = max(alpha,_EdgeColor2.a*edgeMask2);
                return float4(col.xyz,alpha);
                // just invert the colors
                // _Color.rgb = 1 - _Color.rgb;

                // return float4(col.xyz,_Color.w);
            }
            ENDCG
        }
    }
}

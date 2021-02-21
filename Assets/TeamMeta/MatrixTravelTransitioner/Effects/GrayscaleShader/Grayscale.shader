Shader "Hidden/Grayscale"
{
    Properties
    {
        _Grayness("Grayness",range(0,1)) = 0
        _InvertColorWeight("InvertColorWeight",range(0,1)) = 0.2
        _MainTex ("Texture", 2D) = "white" {}
        _InvertBlackThreshold ("InvertBlackThreshold", range(0,1)) = 0.1
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Grayness;
            float _InvertColorWeight;
            float _InvertBlackThreshold;

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);
                // just invert the colors
                float3 withInvertColorWeight = lerp(col.rgb, 1 - col.rgb, -_Grayness* _InvertColorWeight);
                col.rgb = dot(col.rgb, float3(0.3, 0.59, 0.11))<_InvertBlackThreshold?col.rgb:withInvertColorWeight;
                // col.rgb = lerp(col.rgb, 1 - col.rgb, -_Grayness* _InvertColorWeight);
                col.rgb = lerp(col.rgb, dot(col.rgb, float3(0.3, 0.59, 0.11)), _Grayness);
                
                return col;
            }
            ENDCG
        }
    }
}

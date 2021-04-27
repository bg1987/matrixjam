Shader "Unlit/LineShader"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill", Range(0,1)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            float4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Fill;
            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col*=_Color;
                
                // if(i.uv.y<0.01)
                //     i.uv.y = 0;
                bool fillMask = 1-i.uv.y<_Fill && _Fill!=0;
                // bool fillMask = !(1-i.uv.y)==_Fill;

                float alpha = _Color.a* fillMask;
                return float4(col.rgb,alpha);
            }
            ENDCG
        }
    }
}

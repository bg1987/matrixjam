Shader "Unlit/LineShader"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        [hdr] _EndColor ("EndColor", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Fill ("Fill", Range(0,1)) = 1
        _Width ("Width", Range(0,1)) = 1

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
            float4 _EndColor;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Fill;
            float _Width;
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
                col*=lerp(_Color,_EndColor,1-i.uv.y);
                // if(i.uv.y<0.01)
                //     i.uv.y = 0;
                float x = abs(i.uv.x*2 -1);
                // x = 1-distance(x, 1);
                // return float4(x.xxxx);
                // x = x * (x>=0.8);
                float pd = 1-( saturate (x-_Width)/(fwidth(i.uv)));
                float yo = length(float2(ddx(x),ddy(x)));
                // pd = 1-smoothstep(_Width-yo,_Width,x);

                pd = saturate(pd);
                // float mask = saturate(x/pd);
                // if(x>0.5)
                    // return float4(pd.xxx,1);
                    // return float4(pd.xxxx);
                bool widthMask = x<=_Width;
                bool fillMask = 1-i.uv.y<_Fill && _Fill!=0;
                // bool fillMask = !(1-i.uv.y)==_Fill;

                float alpha = _Color.a* fillMask * (pd);
                return float4(col.rgb,alpha);
            }
            ENDCG
        }
    }
}

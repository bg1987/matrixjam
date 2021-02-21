Shader "Hidden/Dissolve"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Dissolve("Dissolve",range(0,1)) = 0
        _DissolveMap ("DissolveMap", 2D) = "white" {}
        _Color ("Color", Color) = (0.5,0.5,0.5,1)

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        // No culling or depth
        Cull Off ZWrite Off ZTest Off
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
            };

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            float _Dissolve;
            sampler2D _DissolveMap;
            float4 _Color;

            fixed4 frag (Interpolators i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, i.uv);

                fixed4 dissolveMask = tex2D(_DissolveMap, i.uv);
                // float pi = 3.14159265359;
                // float t = (cos(2*pi * dissolveMask)+1)/2;
                clip( (dissolveMask>_Dissolve)-0.001);
                // float alpha = (dissolveMask>_Dissolve);
                // just invert the colors
                // _Color.rgb = 1 - _Color.rgb;
                return float4(col.xyz,_Color.w);
            }
            ENDCG
        }
    }
}

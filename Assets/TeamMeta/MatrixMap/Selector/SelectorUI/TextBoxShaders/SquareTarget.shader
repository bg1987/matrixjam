Shader "Unlit/SquareTarget"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _LineWidth ("LineWidth", float) = 0.1
        _LineLength ("LineLength", float) = 0.3
        

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
                float3 scale : TEXCOORD1;
            };

            float4 _Color;

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _LineWidth;
            float _LineLength;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                
                float3 scale = float3
                (
                    length(unity_ObjectToWorld._m00_m10_m20),
                    length(unity_ObjectToWorld._m01_m11_m21),
                    length(unity_ObjectToWorld._m02_m12_m22)
                );
                o.scale = scale;

                return o;
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col*=_Color;

                // float biggerScale = max(i.scale.x,i.scale.y);
                // float lineWidth = 0.1/biggerScale;
                // float lineHeight = 0.1/biggerScale;

                float lineWidth = _LineWidth;
                float lineWidthScaledX = lineWidth/i.scale.x;
                float lineWidthScaledY = lineWidth/i.scale.y;
                float lineHeight = _LineLength;
                float lineHeightScaledX = lineHeight/i.scale.x;
                float lineHeightScaledY = lineHeight/i.scale.y;

                float2 leftStartPosition = float2(0,1);
                float2 rightStartPosition = float2(1,0);
                
                bool LeftLineMaskHorizontal = i.uv.y> leftStartPosition.y-lineWidthScaledY && i.uv.x< leftStartPosition.x+lineHeightScaledX;
                bool leftLineMaskVertical =   i.uv.x< leftStartPosition.x+lineWidthScaledX && i.uv.y> leftStartPosition.y-lineHeightScaledY;
                    
                // bool test = (i.uv.y> (leftStartPosition.y-lineWidthScaledY)) || (rightStartPosition.y+lineWidthScaledY)>i.uv.y;
                // test = (i.uv.x< (leftStartPosition.x+lineWidthScaledX) || i.uv.x> (rightStartPosition.x-lineWidthScaledX));
                // return float4( test.xxx,1);

                bool leftLineMask = LeftLineMaskHorizontal || leftLineMaskVertical;


                bool rightLineMaskHorizontal = i.uv.x> rightStartPosition.x-lineWidthScaledX && i.uv.y< rightStartPosition.y+lineHeightScaledY;
                bool rightLineMaskVertical =   i.uv.y< rightStartPosition.y+lineWidthScaledY && i.uv.x> rightStartPosition.x-lineHeightScaledX;

                bool rightLineMask = rightLineMaskHorizontal || rightLineMaskVertical;
                bool lineMask = rightLineMask || leftLineMask;
                // return float4( lineMask.xxx,lineMask.x);
                // return float4(i.scale.xxx,1);


                return float4(col.rgb,lineMask*col.a);
            }
            ENDCG
        }
    }
}

Shader "Unlit/Brackets"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        _Width ("Width", float) = 0.1
        _MainTex ("Texture", 2D) = "white" {}
    
        _StartPos ("Brackets(x->Distance, y->Height, z, w->Nothing)", vector) = (0.72,0.7,1,1) 
        _BracketsDistance ("BracketsDistance", float) = 1
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
            // make fog work
            #pragma multi_compile_fog

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

            float _Width;
            float _BracketsDistance;

            float2 _StartPos;
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
            float InverseLerp(float A, float B, float T)
            {
                return (T - A)/(B - A);
            }

            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                col*= _Color;

                float xOneToOne = i.uv.x*2-1;
                xOneToOne = abs(xOneToOne);

                float yOneToOne = i.uv.y*2-1;
                yOneToOne =abs(yOneToOne);

                float2 uvOneToOne = float2(xOneToOne,yOneToOne);

                float2 uv = xOneToOne;
                float2 startPos =_StartPos;
                float2 endPos = float2(_BracketsDistance,0.5);
                
                startPos.x += (i.scale.x - 1)/i.scale.x;
                startPos.x += -(i.scale.y-1)/i.scale.x; 
                startPos.x -= 1-endPos.x;
                _Width = _Width/i.scale.y;
                
                // endPos.x += (i.scale.x - 1)/i.scale.x;
                // endPos.x += -(i.scale.y-1)/i.scale.x; 
                
                float xT = InverseLerp(startPos.x,endPos.x,uv.x);
                xT = 1-xT;

                bool mask = yOneToOne<xT && yOneToOne<startPos.y;
                bool mask2 = yOneToOne+_Width>xT;
                bool BracketsMask = mask && mask2;
                 
                col*=BracketsMask;
                return col;

            }
            ENDCG
        }
    }
}

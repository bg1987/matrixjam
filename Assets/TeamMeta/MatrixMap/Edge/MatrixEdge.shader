Shader "Unlit/MatrixEdge"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        [hdr] _EndColor ("Color", Color) = (1,1,1,1)
        _MainTex ("Texture", 2D) = "white" {}
        _Dissolve ("Dissolve", Range(0,1)) = 0
        _EdgeLength ("EdgeLength(By Script)", float) = 1 //Will be used by a script that has edge length calculated
        _StartWorldPosition ("StartWorldPosition(By Script)", vector) = (0,0,0) //Will be used by a script that has edge length calculated
        _EndWorldPosition ("EndWorldPosition(By Script)", vector) = (1,1,0) //Will be used by a script that has edge length calculated
        _EndEdgeSize ("EndEdgeSize", float) = 1 //Size here is the same as the one used in Unity's cube
        _EndEdgeOffset ("EdgeOffset", float) = 0 //Size here is the same as the one used in Unity's cube
        _StartEdgeRadius ("StartEdgeRadius", float) = 2 //Size here is the same as the one used in Unity's cube
        
        _ScrollSpeed ("ScrollSpeed", Float) = 1

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
                // float2 noiseUV : TEXCOORD1;
            };

            struct Interpolators
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                float3 worldPos : TEXCOORD1;
                float2 noiseUV : TEXCOORD2;
            };

            float4 _Color;
            float4 _EndColor;
            sampler2D _MainTex;
            float4 _MainTex_ST;

            float _Dissolve;
            float3 _StartWorldPosition;
            float3 _EndWorldPosition;
            float _EdgeLength;
            float _EndEdgeSize;
            float _EndEdgeOffset;
            float _StartEdgeRadius;

            float _ScrollSpeed;

            float _MatrixMapTime;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.noiseUV = TRANSFORM_TEX(v.uv, _MainTex);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);

                return o;
            }

            float Unity_InverseLerp_float(float A, float B, float T)
            {
                return (T - A)/(B - A);
            }
            float4 frag (Interpolators i) : SV_Target
            {
                clip((distance(_StartWorldPosition, i.worldPos) > _StartEdgeRadius) - 1);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.noiseUV);

                float dissolveMask = 1-i.uv.y;
                bool shouldDissolve = _Dissolve<dissolveMask;
                clip(shouldDissolve-1);

                // col = dissolveMask;

                // col.a = 1;
                float time = _MatrixMapTime;
                float tile = frac((_EdgeLength*i.uv.y)-time);
                tile = tile*2 -1;
                tile = abs(tile);
                // return _Color*tile;
                col = _Color*tile;

                float2 edgeStart = float2(0.5, (_EdgeLength - _EndEdgeSize - _EndEdgeOffset));
                float2 edgeEnd = float2(0.5, _EdgeLength);
                bool isEdge = _EdgeLength * i.uv.y  >= _EdgeLength - _EndEdgeSize - _EndEdgeOffset;
                
                float distanceToEdgeEnd = _EdgeLength - edgeStart.y;
                distanceToEdgeEnd -= _EndEdgeOffset;
                float distanceToEdgeEndX = 1 - edgeStart.x;

                float distanceToEdgeY = distance(edgeStart.y,_EdgeLength*i.uv.y);
                float distanceToEdgeX = distance(edgeStart.x,i.uv.x);


                float edgeStartToEndT = Unity_InverseLerp_float(0,distanceToEdgeEnd,distanceToEdgeY);
                edgeStartToEndT = 1 - edgeStartToEndT;

                float distanceFromEdgeX = abs(i.uv.x*2 - 1); //Distance in x axis that goes:  UV: 0, 0.5, 1 <=> 1, 0, 1
                bool shouldDrawEdge = distanceFromEdgeX<=edgeStartToEndT; //
                clip(shouldDrawEdge - isEdge);

                float endArrowMask = isEdge;
                col*= lerp(1,_EndColor,endArrowMask);
                // return endArrowMask + col;
                return col;
            }
            ENDCG
        }
    }
}

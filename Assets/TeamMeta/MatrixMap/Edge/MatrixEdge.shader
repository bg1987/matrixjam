Shader "Unlit/MatrixEdge"
{
    Properties
    {
        [hdr] _Color ("Color", Color) = (1,1,1,1)
        [hdr] _EndColor ("EndColor", Color) = (1,1,1,1)

        _Color1 ("Color1", Color) = (0.12,0.12,0.5)
        _ColorIntensity1 ("ColorIntensity1", float) = 1
        _Color2 ("Color2", Color) = (0.1,0.7,0.55)
        _ColorIntensity2 ("ColorIntensity2", float) = 1
        _ColorsBlendMap ("ColorsBlendMap", 2D) = "white" {}
       [Toggle]  _UseTwoColors ("UseTwoColorsMode", float) = 0

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
                float2 colorsBlendMapUV : TEXCOORD3;
            };

            float4 _Color;
            float4 _EndColor;

            float4 _Color1;
            float4 _Color2;
            float _ColorIntensity1;
            float _ColorIntensity2;
            sampler2D _ColorsBlendMap;
            float4 _ColorsBlendMap_ST;

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
            bool _UseTwoColors;
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.noiseUV = TRANSFORM_TEX(v.uv, _MainTex);

                _ColorsBlendMap_ST.y*=_EdgeLength;
                o.colorsBlendMapUV = TRANSFORM_TEX(v.uv, _ColorsBlendMap);
                o.worldPos = mul (unity_ObjectToWorld, v.vertex);

                return o;
            }
            float4 GetMixedColor(float4 color1, float4 color2, float time, Interpolators i)
            {
                i.colorsBlendMapUV.y+=time*0.01;
                i.colorsBlendMapUV.x+=time*0.02;
                float4 colorsBlendMap = tex2D(_ColorsBlendMap, i.colorsBlendMapUV);

                float4 blendedColor = float4(0,0,0,0);
                if(colorsBlendMap.y>=0.5)
                    blendedColor = color1;
                else if(colorsBlendMap.y<0.5)
                    blendedColor = color2;
                return blendedColor;
            }
            float Unity_InverseLerp_float(float A, float B, float T)
            {
                return (T - A)/(B - A);
            }
            float4 frag (Interpolators i) : SV_Target
            {
                float time = _MatrixMapTime;
                // time = _Time.y;
                clip((distance(_StartWorldPosition, i.worldPos) > _StartEdgeRadius) - 1);

                // sample the texture
                fixed4 col = tex2D(_MainTex, i.noiseUV);

                float dissolveMask = 1-i.uv.y;
                bool shouldDissolve = _Dissolve<dissolveMask;
                clip(shouldDissolve-1);

                // col = dissolveMask;

                // col.a = 1;
                float tileCount = floor(_EdgeLength*i.uv.y-time+0.5);
                float tile = frac((_EdgeLength*i.uv.y)-time);
                tile = tile*2 -1;
                tile = abs(tile);

                if(_UseTwoColors)
                {
                    float4 colorHdr1 = _Color1*pow(2, _ColorIntensity1);
                    float4 colorHdr2 = _Color2*pow(2, _ColorIntensity2);

                    bool evenTile = fmod(tileCount,2)==0;
                    _Color = lerp(colorHdr1,colorHdr2,evenTile);
                    _Color.a = 1;
                }

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
                // float lastRelevantTile = _EdgeLength - _EndEdgeSize - _EndEdgeOffset - 1;
                float lastRelevantTile = _EdgeLength - _EdgeLength*i.uv.y - _EndEdgeSize - _EndEdgeOffset;
                // float lastRelevantTileMask = _EdgeLength - _EdgeLength*i.uv.y - _EndEdgeSize - _EndEdgeOffset;
                // lastRelevantTile+= frac(time+0.5);
                float lastRelevantTileMask= lastRelevantTile<1?1:0;

                // float3 lastRelevantTileColor = lerp(float3(1,1,1),float3(0,0,0),lastRelevantTile);

                lastRelevantTile = saturate(lastRelevantTile);
                // return float4(lastRelevantTile,lastRelevantTile,lastRelevantTile,1);
                // return tile>0.5;
                if(lastRelevantTileMask==1 ){
                    float3 lastRelevantTileColor = _EndColor;
                    lastRelevantTileColor = lerp(lastRelevantTileColor, col.rgb ,lastRelevantTile);

                    col.rgb = lastRelevantTileColor;

                }
                    // col.rgb = float3(1,1,1);
                    // return float4(1,1,1,1);
                // if(tileCount+1>lastRelevantTile && lastRelevantTile<lastRelevantTile)
                //     return float4(1,1,1,1);
                // else
                //     return float4(0,0,0,1);
                float distanceToEdgeTileY = floor(distance(edgeStart.y,_EdgeLength*i.uv.y));
                // float4 result = distanceToEdgeY - distanceToEdgeTileY;
                // result.a = 1;
                // return result;
                return col;
            }
            ENDCG
        }
    }
}

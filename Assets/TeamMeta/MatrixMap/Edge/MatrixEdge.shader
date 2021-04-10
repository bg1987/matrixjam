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
        _EndEdgeColorHeight ("EndEdgeColorHeight", float) = 1 //Size here is the same as the one used in Unity's cube
        _EndEdgeOffset ("EdgeOffset", float) = 0 //Size here is the same as the one used in Unity's cube
        _StartEdgeRadius ("StartEdgeRadius", float) = 2 //Size here is the same as the one used in Unity's cube
        
        _ScrollSpeed ("ScrollSpeed", Float) = 1
        
        _TravelProgress ("TravelProgress", range(0,1)) = 0
        _TravelSize ("TravelSize", Float) = 1
        [hdr] _TravelColor ("TravelColor", Color) = (1,1,1,1)


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
            float _EndEdgeColorHeight;
            float _StartEdgeRadius;

            float _ScrollSpeed;

            float _MatrixMapTime;
            bool _UseTwoColors;

            float _TravelProgress;
            float _TravelSize;
            float4 _TravelColor;
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
                // col*= lerp(1,_EndColor,endArrowMask);
                // return endArrowMask + col;
                // float lastRelevantTile = _EdgeLength - _EndEdgeSize - _EndEdgeOffset - 1;

                //--Travel-- 
                _TravelSize=_TravelSize/_EdgeLength;
                float travelHeightStart = _TravelProgress - _TravelSize*(1-_TravelProgress);
                float travelHeightEnd = _TravelProgress + _TravelSize*(_TravelProgress);

                float travelMask = travelHeightEnd> i.uv.y && travelHeightStart<i.uv.y && _TravelProgress!=0;
                travelMask = saturate(travelMask);
                // travelMask= travelMask==1;
                //travelMask.a=1;

                float travelFadeInGradient = Unity_InverseLerp_float(travelHeightEnd,travelHeightStart, _TravelProgress);
                float travelMid = distance(travelHeightEnd, travelHeightStart);
                float travelMidT = Unity_InverseLerp_float(travelHeightStart,travelHeightEnd, i.uv.y);

                float travelMidOneZeroOne = abs((travelMidT*2)-1);

                float travelGradient = 1-travelMidOneZeroOne;
                travelGradient = saturate(travelGradient);
                float4 travelColor = travelGradient*travelMask * _TravelColor;
                travelColor.a = 1;
                // return travelColor;

                col.rgb = lerp(col.rgb,travelColor,travelGradient);


                //--Last Tile Color--

                // float lastRelevantTileStart = (-_EdgeLength*i.uv.y) - _EndEdgeSize - _EndEdgeOffset;
                float lastRelevantTileStart = _EdgeLength - _EndEdgeSize - _EndEdgeOffset- _EndEdgeColorHeight;
                float lastRelevantTile = _EdgeLength*i.uv.y - lastRelevantTileStart;

                // float lastRelevantTile = _EdgeLength - _EdgeLength*i.uv.y - _EndEdgeSize - _EndEdgeOffset;
                lastRelevantTile = saturate(lastRelevantTile);
                // lastRelevantTile = 1-lastRelevantTile;

                float lastRelevantTileMask= lastRelevantTile>0?1:0;

                // && travelHeightEnd>=lastRelevantTile
                //&& travelHeightEnd>=lastRelevantTile
                // return float4(travelHeightEnd,travelHeightEnd,travelHeightEnd,1);
                // return float4(lastRelevantTile,lastRelevantTile,lastRelevantTile,1);
                float travelHeightAdditionToEnd = _TravelSize*(_EdgeLength);
                float endThresh = _EdgeLength*_TravelProgress -travelHeightAdditionToEnd;

                float val = lastRelevantTileMask==1 && endThresh> lastRelevantTileStart* i.uv.y;
                val = lastRelevantTileMask-0.5;
                val += saturate(val);
                val += (travelGradient)-0.5;
                // return float4(val.xxx,1);

                if(lastRelevantTileMask==1 && travelHeightEnd - (_TravelSize/2)> i.uv.y ){
                    float3 lastRelevantTileColor = _EndColor;
                    lastRelevantTileColor = lerp(col.rgb,lastRelevantTileColor ,lastRelevantTile);

                    col.rgb = lastRelevantTileColor;

                }

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

Shader "Unlit/MatrixBG"
{
    Properties
    {
        _Color("Color", Color) = (0,1,0,1)
        _Speed ("Speed", float) = 0.1
        _AppearProgress ("AppearProgress", Range(0,2)) = 1

        _Progress ("Progress", float) = 0

        _BlueNoise ("BlueNoise", 2D) = "white" {}
        _Seed ("Seed", float) = 0
        _Columns ("Columns", range(1,10000)) = 128


    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" }
        
        Blend SrcAlpha OneMinusSrcAlpha
        ZWrite Off
        ZTest LEqual
 
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

            float _Speed;
            float _Progress;
            float _AppearProgress;

            sampler2D _BlueNoise;
            float _Seed;
            float _Columns;
            float4 _Color;

            
            Interpolators vert (MeshData v)
            {
                Interpolators o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                return o;
            }
            float4 Rain(float2 coordinates){
                // _Time.y
                float y;
                float height = 450;
                float lineHeight = 1/height;
                // float rainT = lerp(0,lineHeight,1-coordinates.y);
                // float rainColor = lerp(0,1,(1-coordinates.y)*height);
                float column = floor(coordinates.x*_Columns);
                float randomOffset = tex2D(_BlueNoise, float2(_Seed,_Seed));
                float randomOffsetWidth = tex2D(_BlueNoise, float2(coordinates.x,coordinates.y));

                float columnStartingOffset = sin(column*randomOffset);
                float speed  = cos (column*3.)*.15 + .35; 
                // return float4(columnStartingOffset,columnStartingOffset,columnStartingOffset,1);
                // rainT = 1-rainT;
                float rainStart = 0;
                float rainEnd = lineHeight;
                float offset = 1;
                // float rainT = lerp(0,1,(1-coordinates.y)*height);
                float rainT = frac(coordinates.y + _Time.y *speed + columnStartingOffset);
                // float rainT = frac(1-coordinates.y - _Progress);
                // rainT += frac(coordinates.y + _Progress);
                bool isOverTheEdge = coordinates.y + _Progress>1 || coordinates.y + _Progress>10;

                // float4 wtf = isOverTheEdge? 0:float4(0,1,0,1)/(rainT*20);
                // return wtf;
                float4 normalReturnValue = _Color/(rainT*120* (2-_AppearProgress));
                float4 secondReturnValue = _Color/(rainT/coordinates.x);
                normalReturnValue.y=saturate(normalReturnValue.y);
                // normalReturnValue.w=1;
                return normalReturnValue;
                float4 returnValue = randomOffsetWidth<0?normalReturnValue:secondReturnValue;
                returnValue.y = saturate(returnValue.y);
                return float4(returnValue.xyz,0.7);
                
                // return 
                return isOverTheEdge;
            }
            fixed4 frag (Interpolators i) : SV_Target
            {
                // sample the texture
                // float2 rainCoordinates = tex2D(_BlueNoise, i.uv*_Seed);
                
                float4 rainValue = Rain(i.uv);
                return float4(rainValue.xyz,clamp(_AppearProgress,0,rainValue.w*_AppearProgress));
                float4 finalColor = 1 - i.uv.y >= _AppearProgress ? 0 : rainValue;
                return finalColor;
                // return fixed4( rainCoordinates.xy,0,1);


                // return col;
                // return fixed4(i.uv,0,0.5);
            }
            ENDCG
        }
    }
}

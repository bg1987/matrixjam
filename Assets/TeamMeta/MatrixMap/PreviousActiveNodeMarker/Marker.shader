Shader "Unlit/Marker"
{
    

    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (0.12,0.12,0.5,1)
        _Color2 ("Color2", Color) = (0.12,0.12,0.5,1)
        _ColorIntensity ("ColorIntensity", float) = 1
        _ColorIntensity2 ("ColorIntensity2", float) = 1

        _ColorPulseProgress ("ColorPulseProgress", range(0,1)) = 0
        _ColorPulseRange ("ColorPulseRange", float) = 0
        _ColorFlickerSpeed ("ColorFlickerSpeed", float) = 0.1

        _WaveProgress ("WaveProgress", range(0,1)) = 0
        _WaveColor ("WaveColor", Color) = (0.12,0.12,0.5,1)
        _WaveColorIntensity ("WaveColorIntensity", float) = 1
        _WaveWidth ("WaveWidth", range(0,1)) = 0.2
        [Toggle()] _WaveDirectionDown ("_WaveDirectionDown", float) = 1

        _AppearProgress ("AppearProgress", range(0,1)) = 1

    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue"="Transparent" }
        ZWrite On
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
                float2 textureUV : TEXCOORD1;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;

            float4 _Color;
            float4 _Color2;
            float _ColorIntensity;
            float _ColorIntensity2;
            //Color Pulse
            float _ColorPulseProgress;
            float _ColorPulseRange;

            //Color Flicker
            float _ColorFlickerSpeed;
            
            //Wave
            float _WaveProgress;
            float4 _WaveColor;
            float _WaveColorIntensity;
            float _WaveWidth;
            bool _WaveDirectionDown;

            float _AppearProgress;

            Interpolators vert (MeshData v)
            {
                Interpolators o;
                // v.vertex.y+= _Time
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                o.textureUV = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }
            float InverseLerp(float A, float B, float T)
            {
                return (T - A)/(B - A);
            }
            fixed4 frag (Interpolators i) : SV_Target
            {
                float time = _Time.y;
                float fracTime = frac( time*_WaveProgress);
                float sinTime = sin(time);
                if(_WaveDirectionDown)
                    _WaveProgress= 1-_WaveProgress;

                _WaveProgress = _WaveDirectionDown*(1-fracTime);
                _WaveProgress += (_WaveDirectionDown==0)*(fracTime);
                // sample the texture

                //Color pulse
                _ColorIntensity+= cos( time)* _ColorPulseRange;

                _Color = _Color* pow(2,_ColorIntensity);
                _Color2 = _Color2* pow(2,_ColorIntensity2);
                
                // col *= _Color;

                _WaveColor = _WaveColor* pow(2,_WaveColorIntensity);

                
                float4 col = lerp(_Color,_Color2,tex2D(_MainTex,(time*_ColorFlickerSpeed)+i.uv.y + (+i.textureUV.xy)));
                col = lerp(col,_Color2,tex2D(_MainTex, time*0.1).x);
                // return float4(tex2D(_MainTex,time*0.1).xxxx);
                

                //Triangle Mask
                float triangleMaskHeight = i.uv.y;
                float triangleMaskWidth = abs((i.uv.x*2)-1);
                bool triangleMask = triangleMaskHeight>= triangleMaskWidth;

                //Color Wave
                float waveStart = _WaveProgress - _WaveWidth*(1-_WaveProgress);
                float waveEnd = _WaveProgress + _WaveWidth*(_WaveProgress);

                bool waveMask = waveStart<= i.uv.y  && waveEnd >i.uv.y && _WaveProgress != 1;

                // return float4(travelMidT.xxx*waveMask,1);
                // return waveMask;
                // col *= lerp(col,_WaveColor,travelGradient.x);

                if(waveMask)
                    col = _WaveColor;
                // return float4( triangleMask.xxx,triangleMask.x);
                col = col*triangleMask;
                col.a = saturate(col.a);
                // col.a = _AppearProgress*triangleMask;
                float appearMap = tex2D(_MainTex,(i.uv.y)* _AppearProgress);
                // float appearMap = tex2D(_MainTex,(i.uv.y+(_MainTex,i.uv.x*0.1))* _AppearProgress); //Too sophisticated 
                col.a *= _AppearProgress>=appearMap && _AppearProgress>=appearMap;
                return col;
            }
            ENDCG
        }
    }
}

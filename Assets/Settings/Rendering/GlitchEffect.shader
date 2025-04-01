Shader "Custom/GlitchEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _GlitchIntensity ("Glitch Intensity", Range(0,1)) = 0
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert_img
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float _GlitchIntensity;

            float rand(float2 co)
            {
                return frac(sin(dot(co, float2(12.9898,78.233))) * 43758.5453);
            }

            fixed4 frag(v2f_img i) : SV_Target
            {
                float2 uv = i.uv;

                if (_GlitchIntensity <= 0.001f)
                {
                    return tex2D(_MainTex, uv);
                }

                float bandCount = 15.0; 
                float bandIndex = floor(uv.y * bandCount);

                float glitchValue = rand(float2(bandIndex, floor(_Time.y * 10.0)));

                if (glitchValue > 0.7)
                {
                    uv.x += (glitchValue - 0.5) * _GlitchIntensity * 0.15;
                }
                else
                {
                    float smallShift = rand(float2(uv.y * 100.0, _Time.y * 5.0)) - 0.5;
                    uv.x += smallShift * _GlitchIntensity * 0.03;
                }

                float2 uvR = uv;
                float2 uvG = uv;
                float2 uvB = uv;

                uvR.x += (rand(float2(bandIndex, floor(_Time.y * 5.0))) - 0.5) * _GlitchIntensity * 0.02;
                uvB.x -= (rand(float2(bandIndex + 100.0, floor(_Time.y * 5.0))) - 0.5) * _GlitchIntensity * 0.02;

                fixed4 colR = tex2D(_MainTex, uvR);
                fixed4 colG = tex2D(_MainTex, uvG);
                fixed4 colB = tex2D(_MainTex, uvB);

                fixed4 finalColor = fixed4(colR.r, colG.g, colB.b, 1.0);

                return finalColor;
            }
            ENDCG
        }
    }
}
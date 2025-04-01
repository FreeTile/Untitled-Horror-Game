Shader "Hidden/PSXEffect"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Pixelate ("Pixelation", Float) = 240
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            ZTest Always Cull Off ZWrite Off

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;
            float _Pixelate;

            struct appdata_t { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert (appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float3 Quantize(float3 color, float steps)
            {
                return floor(color * steps) / steps;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float2 pixelUV = floor(i.uv * _Pixelate) / _Pixelate;
                float4 col = tex2D(_MainTex, pixelUV);
                col.rgb = Quantize(col.rgb, 150);
                return col;
            }
            ENDCG
        }
    }
}
Shader "Custom/TextureCombine"
{
    Properties
    {
        _RedTexture("Red Texture", 2D) = "white" {}
        _GreenTexture("Green Texture", 2D) = "white" {}
        _BlueTexture("Blue Texture", 2D) = "white" {}
        _AlphaTexture("Alpha Texture", 2D) = "white" {}
        _Invert("Invert",float) = 0.0
    }

        SubShader
    {
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            // Input properties
            sampler2D _RedTexture;
            sampler2D _GreenTexture;
            sampler2D _BlueTexture;
            sampler2D _AlphaTexture;
            float _Invert;

            // Output texture
            half4 _OutputColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv_RedTexture : TEXCOORD0;
                float2 uv_GreenTexture : TEXCOORD1;
                float2 uv_BlueTexture : TEXCOORD2;
                float2 uv_AlphaTexture : TEXCOORD3;
            };

            struct v2f
            {
                float2 uv_RedTexture : TEXCOORD0;
                float2 uv_GreenTexture : TEXCOORD1;
                float2 uv_BlueTexture : TEXCOORD2;
                float2 uv_AlphaTexture : TEXCOORD3;
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv_RedTexture = v.uv_RedTexture;
                o.uv_GreenTexture = v.uv_GreenTexture;
                o.uv_BlueTexture = v.uv_BlueTexture;
                o.uv_AlphaTexture = v.uv_AlphaTexture;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 red = tex2D(_RedTexture, i.uv_RedTexture);
                fixed4 green = tex2D(_GreenTexture, i.uv_GreenTexture);
                fixed4 blue = tex2D(_BlueTexture, i.uv_BlueTexture);
                fixed4 alpha = tex2D(_AlphaTexture, i.uv_AlphaTexture);

                half3 albedo = half3(red.r, green.g, blue.b);
                half alphaValue = 0;
                
                if (_Invert == 0.0)
                {
                    alphaValue = alpha.r;
                }
                else 
                {
                    alphaValue = 1 - alpha.r;
                }
                _OutputColor = half4(albedo, alphaValue);
                return _OutputColor;
            }
            ENDCG
        }
    }
}

Shader "Horus/Unlit/WoolWavy"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}

        _Color ("Color", Color) = (1,1,1,1)
        _AOColor ("AO Color", Color) = (1,1,1,1)
        _Amplitude ("Wave Amplitude", Float) = 0.05
        _Frequency ("Wave Frequency", Float) = 8.0
        _Speed ("Wave Speed", Float) = 1.0
        _WaveWidth ("Visible Width", Float) = 0.03
        _Display ("Display", Range(0, 1)) = 0.0
        _Brightness ("Brightness", Range(0.1, 10)) = 1.0
        _Ambient ("Ambient Light", Range(0, 1)) = 0.3
        _DiffusePower ("Diffuse Power", Range(0, 10)) = 0.7
        _LightDir ("Light Direction", Vector) = (0.4, 1, 0.6, 0)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowStrength ("Shadow Strength", Range(0, 2)) = 1.0
        _ShadowExposure ("Shadow Exposure", Range(0, 5)) = 5.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        
        Stencil
        {
            Ref 128
            Comp Always
            Pass Replace
        }
        
        LOD 100

        Pass
        {
            Cull Back

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float4 _AOColor;
            float _Amplitude;
            float _Frequency;
            float _Speed;
            float _WaveWidth;
            float _Display;
            float _Brightness;
            float _Ambient;
            float _DiffusePower;
            float4 _LightDir;
            float4 _ShadowColor;
            float _ShadowStrength;
            float _ShadowExposure;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float2 worldUV : TEXCOORD2;
            };

            v2f vert (appdata v)
            {
                v2f o;
                float2 uv = v.uv;

                float fade = sin(uv.x * 3.1415926); // 0 ở hai đầu, 1 ở giữa
                float wave = sin((uv.x + _Time.y * _Speed) * _Frequency * 6.2831) * _Amplitude * fade;

                float3 pos = v.vertex.xyz;
                pos.x += wave; // Đẩy theo chiều X thay vì Y

                o.vertex = UnityObjectToClipPos(float4(pos, 1));
                o.uv = TRANSFORM_TEX(uv, _MainTex);
                o.worldUV = uv;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                float fade = sin(i.worldUV.x * 3.1415926); // 0 ở hai đầu, 1 ở giữa
                float centerY = sin((i.worldUV.x + _Time.y * _Speed) * _Frequency * 6.2831) * _Amplitude * fade;
                float dist = abs(i.worldUV.y - 0.5 - centerY);
                float size = _WaveWidth * (1 - abs(i.worldUV.x - 0.5));
                float waveLight = (1 - dist / size) * 0.5 + 0.5;
                clip(size - dist);

                clip(_Display - i.worldUV.x);

                // Chỉ lấy sợi len ở giữa texture (sợi len chạy ngang)
                // Texture có 4 sợi theo chiều dọc, lấy sợi ở vị trí 0.375 -> 0.625 (chiều X của texture)
                float2 warpedUV = i.uv;
                warpedUV.x = 0.375 + (i.worldUV.x * 0.25); // Chỉ lấy 1/4 texture ở giữa theo chiều X
                warpedUV.y += centerY * 0.5; // Uốn UV theo sóng theo chiều Y
                
                // Albedo
                fixed4 albedo = tex2D(_MainTex, warpedUV) * _Color;

                // AO from albedo alpha
                half ao = albedo.a;
                half3 aoFinal = ao * _AOColor.rgb;

                // Lighting direction
                float3 lightDir = normalize(_LightDir.xyz);
                
                // Diffuse lighting - đơn giản hóa vì không có world normal
                float NdotL = saturate(dot(float3(0, 0, 1), lightDir));

                // Base brightness with AO
                half3 baseBrightness = albedo.rgb * aoFinal;

                // Lit color with ambient, diffuse power and brightness
                half3 litBase = baseBrightness * (_Ambient + _DiffusePower * NdotL) * _Brightness;
                half shadowFactor = saturate(NdotL * _ShadowExposure);
                half3 shadowedBase = lerp(baseBrightness, baseBrightness * _ShadowColor.rgb, _ShadowStrength) * _Brightness;
                half3 litColor = lerp(shadowedBase, litBase, shadowFactor);

                // Áp dụng wave light effect
                litColor *= waveLight * 2;

                return fixed4(litColor, albedo.a);
            }
            ENDCG
        }
    }
}

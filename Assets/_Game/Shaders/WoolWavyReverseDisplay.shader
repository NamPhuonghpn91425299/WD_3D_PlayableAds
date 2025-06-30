Shader "Horus/Unlit/WoolWavyReverseDisplay"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1,1,1,1)
        _Amplitude ("Wave Amplitude", Float) = 0.05
        _Frequency ("Wave Frequency", Float) = 8.0
        _Speed ("Wave Speed", Float) = 1.0
        _WaveWidth ("Visible Width", Float) = 0.03
        _Display ("Display", Range(0, 1)) = 0.0
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 100

        Stencil
        {
            Ref 128
            Comp NotEqual
            Pass Replace
            Fail Keep
            ZFail Keep
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _Amplitude;
            float _Frequency;
            float _Speed;
            float _WaveWidth;
            float _Display;

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

            v2f vert(appdata v)
            {
                v2f o;
                float2 uv = v.uv;

                float fade = sin(uv.x * 3.1415926); // 0 ở hai đầu, 1 ở giữa
                float wave = sin((uv.x + _Time.y * _Speed) * _Frequency * 6.2831) * _Amplitude * fade;

                float3 pos = v.vertex.xyz;
                pos.y += wave; // Đẩy theo chiều thickness (y) của line renderer trên XY

                o.vertex = UnityObjectToClipPos(float4(pos, 1));
                o.uv = TRANSFORM_TEX(uv, _MainTex);
                o.worldUV = uv;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float fade = sin(i.worldUV.x * 3.1415926); // 0 ở hai đầu, 1 ở giữa
                float centerY = sin((i.worldUV.x + _Time.y * _Speed) * _Frequency * 6.2831) * _Amplitude * fade;
                float dist = abs(i.worldUV.y - 0.5 - centerY);
                clip(_WaveWidth - dist);

                clip(_Display - i.worldUV.x);

                fixed4 tex = tex2D(_MainTex, i.uv) * _Color;
                return tex;
            }
            ENDCG
        }
    }
}
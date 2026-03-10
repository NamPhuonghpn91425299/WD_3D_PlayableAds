Shader "Horus/UnLit/IgnoreYarn_SamsungFix"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _AOTex ("Ambient Occlusion", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _AOColor ("AO Color", Color) = (1,1,1,1)
        _Brightness ("Brightness", Range(0.1, 10)) = 1.0
        _Ambient ("Ambient Light", Range(0, 1)) = 0.3
        _DiffusePower ("Diffuse Power", Range(0, 10)) = 0.7
        _LightDir ("Light Direction", Vector) = (0.4, 1, 0.6, 0)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" "Queue"="Geometry" }

        // Loại bỏ Stencil để tránh lỗi trên Samsung Internet Browser
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            
            // Tắt Instancing để tăng tính tương thích cho WebGL Samsung
            // #pragma multi_compile_instancing 

            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _AOTex;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float4 _LightDir;
            
            // Đưa về biến thường thay vì Instance Buffer để an toàn cho GPU Mali
            float4 _Color;
            float4 _AOColor;
            float _Brightness;
            float _Ambient;
            float _DiffusePower;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 lightDirTangent : TEXCOORD1; // Tính hướng sáng tại Vertex
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Tính toán TBN tại Vertex Shader (Tối ưu cho Samsung)
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;
                float3x3 worldToTangent = float3x3(worldTangent, worldBinormal, worldNormal);

                // Chuyển hướng sáng về không gian Tangent ngay tại đây
                o.lightDirTangent = mul(worldToTangent, _LightDir.xyz);

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                // Sử dụng half/fixed để tối ưu cho GPU Mali (Samsung)
                fixed4 albedo = tex2D(_MainTex, i.uv) * _Color;
                half ao = tex2D(_AOTex, i.uv).r;
                half3 aoFinal = ao * _AOColor.rgb;

                // Normal map xử lý đơn giản hơn
                float3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.uv));
                
                // Lighting (NdotL) tính trong không gian Tangent
                half NdotL = saturate(dot(tangentNormal, normalize(i.lightDirTangent)));

                half3 baseBrightness = albedo.rgb * aoFinal;
                half3 litColor = baseBrightness * (_Ambient + _DiffusePower * NdotL) * _Brightness;

                return fixed4(litColor, albedo.a);
            }
            ENDCG
        }
    }
    FallBack "Mobile/Unlit"
}
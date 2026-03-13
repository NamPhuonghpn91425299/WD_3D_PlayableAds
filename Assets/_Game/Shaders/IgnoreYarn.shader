Shader "Horus/UnLit/IgnoreYarn_Fixed"
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

        Stencil
        {
            Ref 128
            Comp NotEqual
            Pass Keep
            Fail Keep
        }

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _AOTex;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float4 _LightDir;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float4, _AOColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _Brightness)
                UNITY_DEFINE_INSTANCED_PROP(float, _Ambient)
                UNITY_DEFINE_INSTANCED_PROP(float, _DiffusePower)
            UNITY_INSTANCING_BUFFER_END(Props)

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 worldNormal : TEXCOORD3;
                float3 worldTangent : TEXCOORD4;
                float3 worldBinormal : TEXCOORD5;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);

                // Tính toán không gian thế giới
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                float3 worldBinormal = cross(worldNormal, worldTangent) * v.tangent.w;

                o.worldNormal = worldNormal;
                o.worldTangent = worldTangent;
                o.worldBinormal = worldBinormal;

                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);

                // Access Instanced Props
                float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                float4 aoColor = UNITY_ACCESS_INSTANCED_PROP(Props, _AOColor);
                float brightness = UNITY_ACCESS_INSTANCED_PROP(Props, _Brightness);
                float ambient = UNITY_ACCESS_INSTANCED_PROP(Props, _Ambient);
                float diffusePower = UNITY_ACCESS_INSTANCED_PROP(Props, _DiffusePower);

                // Albedo & AO
                fixed4 albedo = tex2D(_MainTex, i.uv) * color;
                half ao = tex2D(_AOTex, i.uv).r;
                half3 aoFinal = lerp(half3(1,1,1), aoColor.rgb, 1.0 - ao); // Cách tính AO an toàn hơn

                // Normal Reconstruction (An toàn cho Mali GPU)
                float3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.uv));
                float3 worldNormal = normalize(
                    tangentNormal.x * i.worldTangent +
                    tangentNormal.y * i.worldBinormal +
                    tangentNormal.z * i.worldNormal
                );

                // Lighting
                float3 lightDir = normalize(_LightDir.xyz);
                float NdotL = saturate(dot(worldNormal, lightDir));

                half3 litColor = albedo.rgb * aoFinal * (ambient + diffusePower * NdotL) * brightness;

                return fixed4(litColor, albedo.a);
            }
            ENDCG
        }
    }
    FallBack "Mobile/Diffuse"
}
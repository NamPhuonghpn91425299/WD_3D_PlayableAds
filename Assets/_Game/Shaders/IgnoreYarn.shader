Shader "Horus/UnLit/IgnoreYarn"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Brightness ("Brightness", Range(0.1, 10)) = 1.0
        _Ambient ("Ambient Light", Range(0, 1)) = 0.3
        _DiffusePower ("Diffuse Power", Range(0, 2)) = 0.7
        _LightDir ("Light Direction", Vector) = (0.4, 1, 0.6, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }

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
            #include "UnityCG.cginc"
            #pragma multi_compile_instancing

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float4 _LightDir;

            UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
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
                float2 uv2 : TEXCOORD1;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float2 normalUV : TEXCOORD1;
                float2 uv2 : TEXCOORD2;
                float3 worldNormal : TEXCOORD3;
                float3 worldTangent : TEXCOORD4;
                float3 worldBinormal : TEXCOORD5;
                float3 worldViewDir : TEXCOORD6;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID // necessary only if you want to access instanced properties in fragment Shader.
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o)
                ; // necessary only if you want to access instanced properties in the fragment Shader.
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.normalUV = TRANSFORM_TEX(v.uv, _NormalMap);
                o.uv2 = v.uv2;

                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.worldNormal = UnityObjectToWorldNormal(v.normal);
                o.worldTangent = UnityObjectToWorldDir(v.tangent.xyz);
                o.worldBinormal = cross(o.worldNormal, o.worldTangent) * v.tangent.w;
                o.worldViewDir = UnityWorldSpaceViewDir(worldPos);

                return o;
            }

            // Temporary - sua lai sau
            fixed4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i)

                float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                float brightness = UNITY_ACCESS_INSTANCED_PROP(Props, _Brightness);
                float ambient = UNITY_ACCESS_INSTANCED_PROP(Props, _Ambient);
                float diffusePower = UNITY_ACCESS_INSTANCED_PROP(Props, _DiffusePower);

                float3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.normalUV));
                float3x3 TBN = float3x3(normalize(i.worldTangent), normalize(i.worldBinormal), normalize(i.worldNormal));
                float3 worldNormal = normalize(mul(tangentNormal, TBN));

                // calculate manually -_- leave it be
                float3 lightDir = normalize(_LightDir.xyz);
                float NdotL = saturate(dot(worldNormal, lightDir));

                fixed4 albedo = tex2D(_MainTex, i.uv) * color;
                float3 litColor = albedo.rgb * (ambient + diffusePower * NdotL) * brightness;

                return fixed4(litColor, albedo.a);
            }
            ENDCG
        }
    }

    FallBack "Standard"
}
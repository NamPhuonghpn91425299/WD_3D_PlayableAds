Shader "Horus/Unlit/WoolMeshUnlit"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Display ("Display", Range(0,1)) = 0.5
        _Brightness ("Brightness", Range(0.1, 10)) = 1.0
        _Ambient ("Ambient Light", Range(0, 1)) = 0.3
        _DiffusePower ("Diffuse Power", Range(0, 2)) = 0.7
        _LightDir ("Light Direction", Vector) = (0.4, 1, 0.6, 0)

        _RimColor ("Rim Color", Color) = (1,1,1,1)
        _RimPower ("Rim Power", Range(0.1, 8)) = 3.0
        _RimStrength ("Rim Strength", Range(0, 1)) = 0.5
        _UseRim ("Use Rim Light", Float) = 1.0

        _UseHaloGlow ("Use Halo Glow", Float) = 1.0
        _HaloColor ("Halo Color", Color) = (1, 1, 1, 1)
        _HaloPower ("Halo Power", Range(0.1, 8)) = 2.5
        _HaloIntensity ("Halo Intensity", Range(0, 3)) = 1.0
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200
        Cull Off

        // ---------- Main Unlit Pass ----------
        Pass
        {
            Name "Main"
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float4 _LightDir;
            float4 _RimColor;
            float _RimPower;
            float _RimStrength;
            float _UseRim;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float, _Display)
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
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
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

            fixed4 frag (v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                float display = UNITY_ACCESS_INSTANCED_PROP(Props, _Display);
                clip(display - i.uv2.y);

                float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                float brightness = UNITY_ACCESS_INSTANCED_PROP(Props, _Brightness);
                float ambient = UNITY_ACCESS_INSTANCED_PROP(Props, _Ambient);
                float diffusePower = UNITY_ACCESS_INSTANCED_PROP(Props, _DiffusePower);

                float3 viewDir = normalize(i.worldViewDir);
                float3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.normalUV));
                float3x3 TBN = float3x3(normalize(i.worldTangent), normalize(i.worldBinormal), normalize(i.worldNormal));
                float3 worldNormal = normalize(mul(tangentNormal, TBN));

                float3 lightDir = normalize(_LightDir.xyz);
                float NdotL = saturate(dot(worldNormal, lightDir));

                fixed4 albedo = tex2D(_MainTex, i.uv) * color;
                float3 litColor = albedo.rgb * (ambient + diffusePower * NdotL) * brightness;

                if (_UseRim > 0.5)
                {
                    float rim = 1.0 - saturate(dot(viewDir, worldNormal));
                    float rimFactor = pow(rim, _RimPower) * _RimStrength;
                    litColor += _RimColor.rgb * rimFactor;
                }

                return fixed4(litColor, albedo.a);
            }
            ENDCG
        }

        // ---------- Halo Glow Pass ----------
        Pass
        {
            Name "HaloGlow"
            Tags { "LightMode"="Always" }

            Cull Front
            ZWrite Off
            Blend SrcAlpha One

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            float _UseHaloGlow;
            float _HaloPower;
            float _HaloIntensity;
            float4 _HaloColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float3 worldNormal : TEXCOORD0;
                float3 viewDir : TEXCOORD1;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                float3 worldNormal = UnityObjectToWorldNormal(v.normal);
                float3 offset = worldNormal * 0.05; // glow thickness
                o.vertex = UnityObjectToClipPos(v.vertex + float4(offset, 0));
                o.worldNormal = worldNormal;
                o.viewDir = normalize(_WorldSpaceCameraPos - worldPos);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                if (_UseHaloGlow < 0.5)
                    discard;

                float glow = pow(1.0 - saturate(dot(i.viewDir, i.worldNormal)), _HaloPower);
                return _HaloColor * glow * _HaloIntensity;
            }
            ENDCG
        }
    }

    FallBack Off
}

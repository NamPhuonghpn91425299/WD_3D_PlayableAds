Shader "Horus/Unlit/WoolMeshUnlit_Vuong_YQ_Mobile"
{
    Properties
    {
        [Header(Textures)]
        _MainTex ("Albedo", 2D) = "white" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _NormalStrength ("Normal Strength", Range(0, 20)) = 1.0
        _AOTex ("Ambient Occlusion", 2D) = "white" {}
        _AOColor ("AO Color", Color) = (1,1,1,1)

        [Header(Lighting)]
        _LightDir ("Light Direction", Vector) = (0.4, 1, 0.6, 0)
        _ShadowColor ("Shadow Color", Color) = (0,0,0,1)
        _ShadowStrength ("Shadow Strength", Range(0, 2)) = 1.0
        _ShadowExposure ("Shadow Exposure", Range(0, 5)) = 5.0
        _DiffusePower ("Diffuse Power", Range(0, 10)) = 0.7

        

        [Header(Display)]
        _Display ("Display", Range(0,1)) = 0.5
        _Brightness ("Brightness", Range(0.1, 10)) = 1.0
        _Saturation ("Saturation", Range(0, 2)) = 1.0
        _Threshold ("Threshold", Float) = 0
        _ScaleFactor ("Scale Factor", Range(0.1, 5)) = 1.0

        [Header(Precision)]
        [Toggle(_LOWP)] _Lowp ("Low Precision", Float) = 0

        [Header(Dark Thread)]
        _DarkThreadColor ("Dark Thread Color", Color) = (0,0,0,1)
        _DarkThreadThreshold ("Dark Thread Threshold", Range(0, 1)) = 0.3
        _DarkThreadSmoothness ("Dark Thread Smoothness", Range(0.01, 0.5)) = 0.1

        [Header(Dissolve)]
        _DissolveCutout ("Dissolve Edge", Range(0, 1)) = 0
        _TestNoiseTex ("Test Noise Tex", Range(0, 1)) = 0
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _EdgeNoiseScale ("Edge Noise Scale", Range(0,1)) = 0.2
}

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 150
        Cull Off
        
        Stencil
        {
            Ref 128
            Comp NotEqual
            Pass Keep
            Fail Keep
        }

        Pass
        {
            Name "Main_LowEnd"
            Tags { "LightMode"="ForwardBase" }

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            #pragma multi_compile_instancing
            #pragma shader_feature _LOWP

            #include "UnityCG.cginc"

            #if defined(_LOWP)
            #define REAL half
            #define REAL2 half2
            #define REAL3 half3
            #define REAL4 half4
            #define REAL3x3 half3x3
            #else
            #define REAL float
            #define REAL2 float2
            #define REAL3 float3
            #define REAL4 float4
            #define REAL3x3 float3x3
            #endif

            sampler2D _MainTex;
            sampler2D _NormalMap;
            sampler2D _AOTex;
            sampler2D _NoiseTex;
            float4 _MainTex_ST;
            float4 _NormalMap_ST;
            float4 _LightDir;

            float _DissolveCutout;
            float _EdgeNoiseScale;
            float _TestNoiseTex;

            UNITY_INSTANCING_BUFFER_START(Props)
                UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP(float, _NormalStrength)
                UNITY_DEFINE_INSTANCED_PROP(float4, _AOColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _DiffusePower)
                
                UNITY_DEFINE_INSTANCED_PROP(float4, _ShadowColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _ShadowStrength)
                UNITY_DEFINE_INSTANCED_PROP(float, _ShadowExposure)
                UNITY_DEFINE_INSTANCED_PROP(float, _Display)
                UNITY_DEFINE_INSTANCED_PROP(float, _Brightness)
                UNITY_DEFINE_INSTANCED_PROP(float, _Saturation)
                UNITY_DEFINE_INSTANCED_PROP(float, _Threshold)
                UNITY_DEFINE_INSTANCED_PROP(float, _ScaleFactor)
                UNITY_DEFINE_INSTANCED_PROP(float4, _DarkThreadColor)
                UNITY_DEFINE_INSTANCED_PROP(float, _DarkThreadThreshold)
                UNITY_DEFINE_INSTANCED_PROP(float, _DarkThreadSmoothness)
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
                REAL2 uv : TEXCOORD0;
                REAL2 normalUV : TEXCOORD1;
                REAL2 uv2 : TEXCOORD2;
                REAL3 worldNormal : TEXCOORD3;
                REAL3 worldTangent : TEXCOORD4;
                REAL3 worldBinormal : TEXCOORD5;
                REAL3 worldViewDir : TEXCOORD6;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            v2f vert(appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_TRANSFER_INSTANCE_ID(v, o);
                float threshold = UNITY_ACCESS_INSTANCED_PROP(Props, _Threshold);
                float scaleFactor = UNITY_ACCESS_INSTANCED_PROP(Props, _ScaleFactor);
                float3 posOS = v.vertex.xyz;
                float3 normalOS = normalize(v.normal);
                posOS += normalOS * threshold;
                posOS *= scaleFactor;
                o.vertex = UnityObjectToClipPos(float4(posOS, 1));
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

            REAL4 frag(v2f i) : SV_Target
            {
                UNITY_SETUP_INSTANCE_ID(i);
                
                REAL display = UNITY_ACCESS_INSTANCED_PROP(Props, _Display);
                
                REAL4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
                REAL brightness = UNITY_ACCESS_INSTANCED_PROP(Props, _Brightness);
                REAL saturation = UNITY_ACCESS_INSTANCED_PROP(Props, _Saturation);
                REAL diffusePower = UNITY_ACCESS_INSTANCED_PROP(Props, _DiffusePower);
                REAL4 aoColor = UNITY_ACCESS_INSTANCED_PROP(Props, _AOColor);
                REAL normalStrength = UNITY_ACCESS_INSTANCED_PROP(Props, _NormalStrength);
                
                REAL4 shadowColor = UNITY_ACCESS_INSTANCED_PROP(Props, _ShadowColor);
                REAL shadowStrength = UNITY_ACCESS_INSTANCED_PROP(Props, _ShadowStrength);
                REAL shadowExposure = UNITY_ACCESS_INSTANCED_PROP(Props, _ShadowExposure);
                REAL4 darkAreaColor = UNITY_ACCESS_INSTANCED_PROP(Props, _DarkThreadColor);
                REAL darkAreaThreshold = UNITY_ACCESS_INSTANCED_PROP(Props, _DarkThreadThreshold);
                REAL darkAreaSmoothness = UNITY_ACCESS_INSTANCED_PROP(Props, _DarkThreadSmoothness);
                
                REAL4 albedo = tex2D(_MainTex, i.uv) * color;
                REAL ao = albedo.a;
                REAL3 aoFinal = ao * aoColor.rgb;
                
                REAL3 viewDir = normalize(i.worldViewDir);
                REAL3 tangentNormal = UnpackNormal(tex2D(_NormalMap, i.normalUV));
                REAL3x3 TBN = REAL3x3(normalize(i.worldTangent), normalize(i.worldBinormal), normalize(i.worldNormal));
                REAL3 normalMapWorldNormal = normalize(mul(tangentNormal, TBN));
                REAL3 worldNormal = normalize(lerp(i.worldNormal, normalMapWorldNormal, normalStrength));
                
                REAL3 lightDir = normalize(_LightDir.xyz);
                REAL NdotL = saturate(dot(worldNormal, lightDir));
                
                REAL3 baseBrightness = albedo.rgb * aoFinal;
                REAL luminance = dot(baseBrightness, REAL3(0.299, 0.587, 0.114));
                REAL darkFactor = smoothstep(darkAreaThreshold - darkAreaSmoothness, darkAreaThreshold + darkAreaSmoothness, luminance);
                darkFactor = 1.0 - darkFactor;
                baseBrightness = lerp(baseBrightness, darkAreaColor.rgb, darkFactor);
                
                REAL shadowFactor = saturate(NdotL * shadowExposure);
                REAL3 litAlbedo = baseBrightness * (1.0 + diffusePower * NdotL) * brightness;
                REAL3 shadowedAlbedo = lerp(baseBrightness, baseBrightness * shadowColor.rgb, shadowStrength) * brightness;
                REAL3 litColor = lerp(shadowedAlbedo, litAlbedo, shadowFactor);
                
                REAL dissolveModeActive = step(display, 0.95);
                REAL dissolveEnabled = step(0.5, _DissolveCutout);
                REAL testNoiseMode = step(0.5, _TestNoiseTex);
                
                REAL noise = tex2D(_NoiseTex, i.uv * 5.0).r;
                
                REAL edgeValueTest = _EdgeNoiseScale;
                REAL edgeValueNormal = clamp(1.0 - display, 0.0, _EdgeNoiseScale);
                
                REAL edgeValue = lerp(edgeValueNormal, edgeValueTest, testNoiseMode);
                
                REAL dissolveWithNoise = i.uv2.y + noise * edgeValue;
                REAL dissolveFinal = lerp(i.uv2.y, dissolveWithNoise, dissolveEnabled);
                
                clip((display - dissolveFinal) * dissolveModeActive);

                REAL finalLuminance = dot(litColor, REAL3(0.299, 0.587, 0.114));
                litColor = lerp(finalLuminance, litColor, saturation);

                return REAL4(litColor, albedo.a);
            }
            ENDCG
        }
    }

    FallBack "Mobile/Diffuse"
}

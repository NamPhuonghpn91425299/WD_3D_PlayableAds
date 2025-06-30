Shader "Horus/Lit/WoolTransparent"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _FresnelPower ("Fresnel Power", Range(0.1, 10)) = 2
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _Threshold ("Threshold", Float) = 0.03
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300
        Cull Back // Hiển thị cả mặt trong và mặt ngoài

        CGPROGRAM
        #pragma surface surf Standard alpha:fade vertex:vert
        #pragma multi_compile_instancing

        sampler2D _MainTex;
        sampler2D _NormalMap;
        float _FresnelPower;
        fixed4 _FresnelColor;
        float _Threshold;

        UNITY_INSTANCING_BUFFER_START(Props)
        // (add instanced properties here if needed)
        UNITY_INSTANCING_BUFFER_END(Props)

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;
            float3 viewDir; // cần cho Fresnel
        };

        void vert(inout appdata_full v)
        {
            UNITY_SETUP_INSTANCE_ID(v);
            float threshold = UNITY_ACCESS_INSTANCED_PROP(Props, _Threshold);
            float3 normalWS = normalize(UnityObjectToWorldNormal(v.normal));
            float3 posWS = mul(unity_ObjectToWorld, v.vertex).xyz;
            posWS += normalWS * threshold;
            float3 posOS = mul(unity_WorldToObject, float4(posWS, 1)).xyz;
            v.vertex.xyz = posOS;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex);
            float3 normalTangent = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            o.Normal = normalTangent;
            o.Albedo = albedo.rgb;

            // Fresnel effect (N.V)
            float fresnel = pow(1.0 - saturate(dot(normalize(normalTangent), normalize(IN.viewDir))), _FresnelPower);
            fixed3 fresnelColor = _FresnelColor.rgb * fresnel;

            o.Emission = fresnelColor;

            o.Alpha = albedo.a * saturate(fresnel); // hoặc chỉ dùng albedo.a nếu không muốn Fresnel ảnh hưởng trong suốt
        }
        ENDCG
    }

    FallBack "Transparent/Cutout/VertexLit"
}

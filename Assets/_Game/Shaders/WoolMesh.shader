Shader "Horus/Lit/WoolMeshPBR"
{
    Properties
    {
        _MainTex ("Albedo", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _Color ("Color Tint", Color) = (1,1,1,1)
        _Metallic ("Metallic", Range(0,1)) = 0.0
        _Smoothness ("Smoothness", Range(0,1)) = 0.5
        _Display ("Display", Range(0,1)) = 0.5
        _ScreenTiling ("Screen Tiling", Vector) = (1, 1, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "RenderType"="Opaque"
        }
        LOD 300
        Cull Off // <--- Cho phép hiển thị cả mặt trong và mặt ngoài

        CGPROGRAM
        #pragma surface surf Standard
        #pragma multi_compile_instancing

        sampler2D _MainTex;
        sampler2D _NormalMap;

        UNITY_INSTANCING_BUFFER_START(Props)
            UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
            UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
            UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
            UNITY_DEFINE_INSTANCED_PROP(float, _Display)
            UNITY_DEFINE_INSTANCED_PROP(float4, _ScreenTiling)
        UNITY_INSTANCING_BUFFER_END(Props)

        struct Input
        {
            float2 uv_MainTex;
            float2 uv2_MainTex;
            float2 uv_NormalMap;
            float3 worldPos;
            float3 viewDir;
        };

        void Unity_Remap_float(float In, float2 InMinMax, float2 OutMinMax, out float Out)
        {
            Out = OutMinMax.x + (In - InMinMax.x) * (OutMinMax.y - OutMinMax.x) / (InMinMax.y - InMinMax.x);
        }

        void surf(Input IN, inout SurfaceOutputStandard o)
        {
            float display = UNITY_ACCESS_INSTANCED_PROP(Props, _Display);

            float posY = IN.uv2_MainTex.y * 2;
            clip(display - posY);

            float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
            float metallic = UNITY_ACCESS_INSTANCED_PROP(Props, _Metallic);
            float smoothness = UNITY_ACCESS_INSTANCED_PROP(Props, _Smoothness);
            float4 screenTiling = UNITY_ACCESS_INSTANCED_PROP(Props, _ScreenTiling);

            fixed4 albedo = tex2D(_MainTex, IN.uv_MainTex) * color;
            o.Albedo = albedo.rgb;
            o.Normal = UnpackNormal(tex2D(_NormalMap, IN.uv_NormalMap));
            o.Metallic = metallic;
            o.Smoothness = smoothness;
            o.Alpha = albedo.a;
        }
        ENDCG
    }

    FallBack "Standard"
}
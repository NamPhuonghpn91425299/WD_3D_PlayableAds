Shader "Horus/ItemColor1Enhanced"
{
  Properties
  {
      _MainTex ("Albedo", 2D) = "white" {}
      _Color ("Color Tint", Color) = (1,1,1,1)
      _LightDir("Light Direction", Vector) = (0.4, 1, 0.6, 0)
      _Metallic ("Metallic", Range(0, 1)) = 0.0
      _Smoothness ("Smoothness", Range(0, 1)) = 0.5
      
      [Header(Enhanced Specular)]
      _SpecularIntensity ("Specular Intensity", Range(0, 5)) = 2.0
      _SpecularPower ("Specular Power", Range(16, 512)) = 128
      _SpecularColor ("Specular Color", Color) = (1,1,1,1)
      
      [Header(Lighting)]
      _LightIntensity ("Light Intensity", Range(0.5, 2)) = 1.0
      _AmbientIntensity ("Ambient Intensity", Range(0, 0.5)) = 0.2
  }

  SubShader
  {
      Tags { "RenderType"="Opaque" "Queue"="Geometry" }
      LOD 200
      Cull Back

      Pass
      {
          Name "ForwardBase"
          Tags { "LightMode"="ForwardBase" }

          CGPROGRAM
          #pragma vertex vert
          #pragma fragment frag
          #pragma multi_compile_instancing
          #pragma multi_compile_fwdbase
          #pragma target 3.0

          #include "UnityCG.cginc"

          // Texture and global properties
          sampler2D _MainTex;
          float4 _MainTex_ST;
          float4 _LightDir;

          // GPU Instancing Buffer - Only Essential Properties
          UNITY_INSTANCING_BUFFER_START(Props)
              UNITY_DEFINE_INSTANCED_PROP(float4, _Color)
              UNITY_DEFINE_INSTANCED_PROP(float, _Metallic)
              UNITY_DEFINE_INSTANCED_PROP(float, _Smoothness)
              UNITY_DEFINE_INSTANCED_PROP(float, _SpecularIntensity)
              UNITY_DEFINE_INSTANCED_PROP(float, _SpecularPower)
              UNITY_DEFINE_INSTANCED_PROP(float4, _SpecularColor)
              UNITY_DEFINE_INSTANCED_PROP(float, _LightIntensity)
              UNITY_DEFINE_INSTANCED_PROP(float, _AmbientIntensity)
          UNITY_INSTANCING_BUFFER_END(Props)

          struct appdata
          {
              float4 vertex : POSITION;
              float3 normal : NORMAL;
              float2 uv : TEXCOORD0;
              UNITY_VERTEX_INPUT_INSTANCE_ID
          };

          struct v2f
          {
              float4 pos : SV_POSITION;
              float2 uv : TEXCOORD0;
              float3 worldNormal : TEXCOORD1;
              float3 worldPos : TEXCOORD2;
              UNITY_VERTEX_INPUT_INSTANCE_ID
              UNITY_VERTEX_OUTPUT_STEREO
          };

          v2f vert(appdata v)
          {
              v2f o;
              UNITY_SETUP_INSTANCE_ID(v);
              UNITY_TRANSFER_INSTANCE_ID(v, o);
              UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
              
              o.pos = UnityObjectToClipPos(v.vertex);
              o.uv = TRANSFORM_TEX(v.uv, _MainTex);
              o.worldNormal = UnityObjectToWorldNormal(v.normal);
              o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
              
              return o;
          }

          fixed4 frag(v2f i) : SV_Target
          {
              UNITY_SETUP_INSTANCE_ID(i);

              // Sample texture once
              fixed4 albedo = tex2D(_MainTex, i.uv);
              
              // Get instanced properties
              float4 color = UNITY_ACCESS_INSTANCED_PROP(Props, _Color);
              float metallic = UNITY_ACCESS_INSTANCED_PROP(Props, _Metallic);
              float smoothness = UNITY_ACCESS_INSTANCED_PROP(Props, _Smoothness);
              float specularIntensity = UNITY_ACCESS_INSTANCED_PROP(Props, _SpecularIntensity);
              float specularPower = UNITY_ACCESS_INSTANCED_PROP(Props, _SpecularPower);
              float4 specularColor = UNITY_ACCESS_INSTANCED_PROP(Props, _SpecularColor);
              float lightIntensity = UNITY_ACCESS_INSTANCED_PROP(Props, _LightIntensity);
              float ambientIntensity = UNITY_ACCESS_INSTANCED_PROP(Props, _AmbientIntensity);
              
              // Apply color tint
              albedo *= color;
              
              // Normalize vectors (optimized)
              float3 normal = normalize(i.worldNormal);
              float3 lightDir = normalize(_LightDir.xyz);
              float3 viewDir = normalize(_WorldSpaceCameraPos - i.worldPos);
              float3 halfDir = normalize(lightDir + viewDir);
              
              // Dot products (calculate once)
              float NdotL = saturate(dot(normal, lightDir));
              float NdotH = saturate(dot(normal, halfDir));
              
              // Material setup (PBR-like)
              float3 diffuseColor = lerp(albedo.rgb, float3(0,0,0), metallic);
              float3 f0 = lerp(float3(0.04, 0.04, 0.04), albedo.rgb, metallic);
              
              // Apply custom specular color
              f0 = lerp(f0, specularColor.rgb, specularColor.a);
              
              // Lighting calculation
              float3 ambient = diffuseColor * ambientIntensity;
              float3 diffuse = diffuseColor * NdotL * lightIntensity;
              
              // Optimized Blinn-Phong specular
              float specularTerm = pow(NdotH, specularPower * smoothness);

              
              float3 specular = f0 * specularTerm * specularIntensity * lightIntensity;
              
              // Final color
              float3 finalColor = ambient + diffuse + specular;
              
              // Simple tone mapping
              finalColor = finalColor / (1.0 + finalColor * 0.5);
              
              return float4(finalColor, albedo.a);
          }
          ENDCG
      }
  }
  
  FallBack "Mobile/Diffuse"
}
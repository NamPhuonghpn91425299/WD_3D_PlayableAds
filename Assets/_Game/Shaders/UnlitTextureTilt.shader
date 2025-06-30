Shader "Unlit/UnlitTextureTilt"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _NormalMap("Normal Map", 2D) = "bump" {}
        _TintColor("Color Tint", Color) = (1,1,1,1)
        _LightDirection("Light Direction", Vector) = (0, 1, 0, 0)
        _LightColor("Light Color", Color) = (1,1,1,1)
    }

    SubShader
    {
        Tags { "RenderType" = "Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
                float4 tangent : TANGENT;
            };

            struct v2f
            {
                float4 pos : SV_POSITION;
                float2 uv : TEXCOORD0;
                float3 viewNormal : TEXCOORD1;
                float3x3 TBN : TEXCOORD2;
            };

            sampler2D _MainTex;
            sampler2D _NormalMap;
            float4 _TintColor;
            float4 _LightDirection;
            float4 _LightColor;

            v2f vert(appdata v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;

                float3 normal = normalize(UnityObjectToWorldNormal(v.normal));
                float3 tangent = normalize(UnityObjectToWorldDir(v.tangent.xyz));
                float3 bitangent = cross(normal, tangent) * v.tangent.w;

                o.TBN = float3x3(tangent, bitangent, normal);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                float3 normalMap = tex2D(_NormalMap, i.uv).xyz * 2 - 1;
                float3 worldNormal = normalize(mul(i.TBN, normalMap));

                float3 lightDir = normalize(_LightDirection.xyz);
                float ndotl = saturate(dot(worldNormal, lightDir));

                float3 baseColor = tex2D(_MainTex, i.uv).rgb * _TintColor.rgb;
                float3 finalColor = baseColor * _LightColor.rgb * ndotl;

                return float4(finalColor, 1);
            }
            ENDCG
        }
    }

    FallBack Off
}

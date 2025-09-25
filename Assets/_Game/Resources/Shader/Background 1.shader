Shader "Horus/BackgroundTiling2D_Radial_Width"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _ScrollXSpeed("Scroll X", Range(-5,5)) = 1.0
        _ScrollYSpeed("Scroll Y", Range(-5,5)) = 0.0
        _Tiling("Tiling (X,Y)", Vector) = (1,1,0,0)
        _GradientCenter("Gradient Center", Color) = (1,1,1,1)
        _GradientEdge("Gradient Edge", Color) = (0,0,0,1)
        _MainTexOpacity("MainTex Opacity", Range(0,1)) = 1.0

            // --- THÊM MỚI 1: Thêm thuộc tính độ rộng ---
            _GradientWidth("Gradient Width", Range(0.01, 5)) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Geometry" "RenderType" = "Opaque" }
            Cull Off
            ZWrite Off
            ZTest LEqual
            Lighting Off

            Pass
            {
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"

                sampler2D _MainTex;
                fixed4 _Color;
                float _ScrollXSpeed;
                float _ScrollYSpeed;
                float4 _Tiling;
                fixed4 _GradientCenter;
                fixed4 _GradientEdge;
                float _MainTexOpacity;

                // --- THÊM MỚI 2: Khai báo biến độ rộng ---
                float _GradientWidth;

                struct appdata_t
                {
                    float4 vertex : POSITION;
                    float2 uv : TEXCOORD0;
                };

                struct v2f
                {
                    float4 vertex : SV_POSITION;
                    float2 uv : TEXCOORD0;
                    float2 screenPos : TEXCOORD1;
                };

                v2f vert(appdata_t v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);
                    o.uv = (v.uv * _Tiling.xy) + float2(_ScrollXSpeed, _ScrollYSpeed) * _Time.y;
                    float4 clipPos = o.vertex;
                    o.screenPos = clipPos.xy / clipPos.w;
                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // --- THAY ĐỔI 3: Sử dụng _GradientWidth ---
                    float dist = length(i.screenPos) / _GradientWidth;

                    fixed4 gradientBackground = lerp(_GradientCenter, _GradientEdge, saturate(dist));

                    fixed4 textureColor = tex2D(_MainTex, frac(i.uv));
                    textureColor *= _Color;
                    textureColor.a *= _MainTexOpacity;

                    fixed4 finalColor;
                    finalColor.rgb = lerp(gradientBackground.rgb, textureColor.rgb, textureColor.a);
                    finalColor.a = 1.0;

                    return finalColor;
                }
                ENDCG
            }
        }
            FallBack "Unlit/Texture"
}
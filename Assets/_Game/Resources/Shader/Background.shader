Shader "Horus/BackgroundTiling2D"
{
    Properties
    {
        _MainTex("Texture", 2D) = "white" {}
        _Color("Tint", Color) = (1,1,1,1)
        _ScrollXSpeed("Scroll X", Range(-5,5)) = 1.0
        _ScrollYSpeed("Scroll Y", Range(-5,5)) = 0.0
        _Tiling("Tiling (X,Y)", Vector) = (1,1,0,0)
        _BgColor("Background Color", Color) = (0,0,0,1)
        _GradientTop("Gradient Top", Color) = (1,1,1,1)
        _GradientBottom("Gradient Bottom", Color) = (1,1,1,1)
        _MainTexOpacity("MainTex Opacity", Range(0,1)) = 1.0
    }
    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
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
            float4 _MainTex_ST;
            fixed4 _Color;
            float _ScrollXSpeed;
            float _ScrollYSpeed;
            float4 _Tiling;
            fixed4 _BgColor;
            fixed4 _GradientTop;
            fixed4 _GradientBottom;
            float _MainTexOpacity;

            struct appdata_t
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
                float2 uv : TEXCOORD0;
                float  screenY01 : TEXCOORD1;
            };

            v2f vert(appdata_t v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = (v.uv * _Tiling.xy) + float2(_ScrollXSpeed, _ScrollYSpeed) * _Time.y;
                // Calculate normalized screen Y (0=bottom, 1=top)
                float4 clipPos = UnityObjectToClipPos(v.vertex);
                o.screenY01 = (clipPos.y / clipPos.w) * 0.5 + 0.5;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 col = tex2D(_MainTex, frac(i.uv));
                col *= _Color;
                col.a *= _MainTexOpacity;
                // Blend with background color based on alpha
                col.rgb = lerp(_BgColor.rgb, col.rgb, col.a);
                col.a = max(col.a, _BgColor.a);
                // Vertical gradient overlay
                fixed4 grad = lerp(_GradientBottom, _GradientTop, saturate(i.screenY01));
                col.rgb *= grad.rgb;
                col.a *= grad.a;
                return col;
            }
            ENDCG
        }
    }
    FallBack "Unlit/Texture"
}
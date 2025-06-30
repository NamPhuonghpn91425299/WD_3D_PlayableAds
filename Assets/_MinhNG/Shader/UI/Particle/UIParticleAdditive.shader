Shader "Horus/UI/ParticleAdditive"
{
    Properties
    {
        [Toggle]_UseLerpColor("Use Lerp Color", Range(0, 1)) = 0
        [HDR]_Color1("BrightColor", Color) = (1, 1, 1, 1)
        _Color2("DarkColor", Color) = (1, 1, 1, 1)
        [Header(Base)][HDR]_TintColor("Tint Color", Color) = (0.5, 0.5, 0.5, 0.5)
        _MainTex("Texture", 2D) = "white" {}
        _Boost ("Boost", Range(0, 10)) = 2
        _Clip("Clip", Range(0,1)) = 0.01
        _ ("--------------",float) = 1
        [Enum(UnityEngine.Rendering.CompareFunction)] _StencilComp ("Stencil Comparison", Float) = 8
        _Stencil ("Stencil ID", Float) = 0
        _StencilOp ("Stencil Operation", Float) = 0
        _StencilWriteMask ("Stencil Write Mask", Float) = 255
        _StencilReadMask ("Stencil Read Mask", Float) = 255
        [Enum(UnityEngine.Rendering.ColorWriteMask)] _ColorMask ("Color Mask", Float) = 15
        [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0
        [Enum(UnityEngine.Rendering.CompareFunction)] _ZTest ("ZTest", Range(0, 8)) = 4
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Off
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend SrcAlpha One
        ColorMask [_ColorMask]
        ZTest [_Ztest]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0
            
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"
            
            #pragma multi_compile_local _ UNITY_UI_CLIP_RECT
            #pragma multi_compile_local _ UNITY_UI_ALPHACLIP
            #pragma shader_feature _USELERPCOLOR_ON

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _MainTex_TexelSize;
            float4 _TintColor;
            float4 _Color1;
            float4 _Color2;
            float _Boost;
            float _Clip;
            float4 _ClipRect;

            float CustomGet2DClipping(float2 position, float4 clipRect)
            {
                return step(clipRect.x, position.x) * step(position.x, clipRect.z) * step(clipRect.y, position.y) *
                    step(position.y, clipRect.w);
            }

            struct appdata
            {
                float4 vertex : POSITION;
                float4 texcoord : TEXCOORD0;
                half4 color : COLOR;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                float2 texcoord : TEXCOORD0;
                float3 worldPos : TEXCOORD2;
                float4 vertex : TEXCOORD3;
                half4 color : COLOR;
            };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = v.vertex;
                o.worldPos = mul(unity_ObjectToWorld, v.vertex).xyz;
                o.position = UnityObjectToClipPos(v.vertex);
                o.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                o.color = v.color;
                return o;
            }

            half4 frag(v2f i) : SV_Target
            {
                float4 mainTexture = tex2D(_MainTex, i.texcoord);

                float4 combineColor;
                #if _USELERPCOLOR_ON
                combineColor = lerp(_Color2, _Color1, mainTexture.r);
                #else
                combineColor = _TintColor;
                #endif

                float4 col = _Boost * i.color * combineColor * mainTexture;
                if (col.a < _Clip)
                {
                    col.a = -1;
                }
                
                // Ensure correct clipping
                #ifdef UNITY_UI_CLIP_RECT
                col.a *= CustomGet2DClipping(i.vertex.xy, _ClipRect);
                #endif
                
                clip(col.a - 0.001);
                
                return col;
            }
            ENDCG
        }
    }
}
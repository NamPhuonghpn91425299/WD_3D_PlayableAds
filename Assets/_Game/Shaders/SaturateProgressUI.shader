Shader "Horus/UI/SaturateProgress"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _UVRemap ("UV Remap", Vector) = (0, 0, 1, 1)
        _Progress ("Progress", Range(0, 1)) = 0
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
        Blend SrcAlpha OneMinusSrcAlpha
        ColorMask [_ColorMask]

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

            struct appdata_t
            {
                float4 vertex : POSITION;
                float4 color : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 position : SV_POSITION;
                half4 color : COLOR;
                float2 texcoord : TEXCOORD0;
                float2 texcoordLocal : TEXCOORD1;
                float4 worldPosition : TEXCOORD2;
            };

            sampler2D _MainTex;
            fixed4 _Color;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float4 _MainTex_ST;
            float _Progress;
            float4 _UVRemap;

            float CustomGet2DClipping(float2 position, float4 clipRect)
            {
                return step(clipRect.x, position.x) * step(position.x, clipRect.z) * step(clipRect.y, position.y) *
                    step(position.y, clipRect.w);
            }

            v2f vert(appdata_t v)
            {
                v2f OUT;
                OUT.worldPosition = v.vertex;
                OUT.position = UnityObjectToClipPos(v.vertex);
                OUT.texcoord = TRANSFORM_TEX(v.texcoord, _MainTex);
                OUT.texcoordLocal = v.texcoord;
                OUT.texcoordLocal.xy = (v.texcoord - _UVRemap.xy) / _UVRemap.zw;
                OUT.color = v.color * _Color;
                return OUT;
            }

            fixed4 frag(v2f IN) : SV_Target
            {
                half4 color = (tex2D(_MainTex, IN.texcoord) + _TextureSampleAdd) * IN.color;
                
                float luminance = dot(color.rgb, float3(0.299, 0.587, 0.114));
                
                // Create a mask: 0 for pixels below progress, 1 for pixels above or equal to progress
                float mask = step(_Progress, IN.texcoordLocal.y);

                // Lerp between color and grayscale based on mask
                color.rgb = lerp(color.rgb, luminance.xxx, mask);

                #ifdef UNITY_UI_CLIP_RECT
    color.a *= CustomGet2DClipping(IN.worldPosition.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
    clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
    Fallback "UI/Default"
}
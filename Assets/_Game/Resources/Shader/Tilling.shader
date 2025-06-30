// Shader để di chuyển texture trên UI Image/Sprite với tốc độ và tiling tùy chỉnh
// Dành cho Unity Built-in Render Pipeline
Shader "UI/ScrollableTexture_BuiltIn" // Đổi tên một chút để phân biệt nếu cần
{
    Properties
    {
        [PerRendererData] _MainTex("Sprite Texture", 2D) = "white" {} // Texture chính
        _Color("Tint", Color) = (1,1,1,1) // Màu phủ (Tint)

        _ScrollXSpeed("Tốc độ X", Range(-5, 5)) = 1.0 // Tốc độ di chuyển X
        _ScrollYSpeed("Tốc độ Y", Range(-5, 5)) = 0.0 // Tốc độ di chuyển Y
        _Tiling("Tiling (X, Y)", Vector) = (1, 1, 0, 0) // Độ lặp lại texture (X, Y)

        // Thuộc tính cần thiết cho UI Masking (Built-in RP)
        _StencilComp("Stencil Comparison", Float) = 8
        _Stencil("Stencil ID", Float) = 0
        _StencilOp("Stencil Operation", Float) = 0
        _StencilWriteMask("Stencil Write Mask", Float) = 255
        _StencilReadMask("Stencil Read Mask", Float) = 255
        _ColorMask("Color Mask", Float) = 15

        [Toggle(UNITY_UI_CLIP_RECT)] _UseUIClipRect("Use Clip Rect", Float) = 0 // Bật/tắt Clipping
    }

        SubShader
        {
            Tags
            {
                "Queue" = "Transparent"
                "IgnoreProjector" = "True"
                "RenderType" = "Transparent"
                "PreviewType" = "Plane"
                "CanUseSpriteAtlas" = "True"
            }

            // Thiết lập Stencil cho UI Masking
            Stencil
            {
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }

            Cull Off
            Lighting Off
            ZWrite Off
            ZTest[unity_GUIZTestMode] // Sử dụng ZTest chuẩn của UI
            Blend SrcAlpha OneMinusSrcAlpha // Hòa trộn alpha
            ColorMask[_ColorMask]

            Pass
            {
                Name "Default"
                CGPROGRAM
                #pragma vertex vert
                #pragma fragment frag
                #pragma target 2.0

                #include "UnityCG.cginc"    // Thư viện CG cơ bản (Built-in)
                #include "UnityUI.cginc"    // Thư viện UI (Built-in)

                #pragma multi_compile __ UNITY_UI_CLIP_RECT // Compile biến thể cho UI Clipping

            // Struct dữ liệu đầu vào cho vertex shader
            struct appdata_t
            {
                float4 vertex   : POSITION;
                float2 texcoord : TEXCOORD0;
                float4 color    : COLOR;
                // worldPosition được UnityUI.cginc thêm vào tự động khi cần thiết (cho clipping)
                // Nếu gặp lỗi, thêm dòng sau:
                // float4 worldPosition : TEXCOORD1;
            };

        // Struct dữ liệu đầu ra từ vertex shader
        struct v2f
        {
            float4 vertex   : SV_POSITION;
            float2 texcoord : TEXCOORD0;
            float4 color    : COLOR;
            float4 worldPosition : TEXCOORD1; // Cần cho clipping
            float4 clipRect : TEXCOORD2;    // Clip Rect từ UnityUI.cginc
        };

        // Khai báo các biến Properties
        sampler2D _MainTex;
        // _MainTex_ST không cần dùng trực tiếp vì ta tự tính toán tiling/offset
        fixed4 _Color;
        float _ScrollXSpeed;
        float _ScrollYSpeed;
        float4 _Tiling;

        // Biến cho UI Clipping (từ UnityUI.cginc)
        float4 _ClipRect;

        // Vertex Shader
        v2f vert(appdata_t v)
        {
            v2f o;
            // Khởi tạo worldPosition (quan trọng cho clipping)
            o.worldPosition = v.vertex; // Sẽ được chuyển đổi đúng trong UnityObjectToClipPos nếu cần
            o.vertex = UnityObjectToClipPos(v.vertex); // Chuyển đổi sang Clip Space
            o.texcoord = v.texcoord; // Giữ nguyên UV gốc
            o.color = v.color * _Color; // Áp dụng màu đỉnh và Tint

            // UnityUI.cginc xử lý việc tính toán clipRect dựa trên _ClipRect và worldPosition
            // Nếu bạn không #include "UnityUI.cginc", bạn cần tự tính toán clipRect
             o.clipRect = _ClipRect; // Gán trực tiếp _ClipRect

            return o;
        }

        // Fragment Shader
        fixed4 frag(v2f i) : SV_Target
        {
            // Tính toán UV di chuyển và lặp lại
            float2 scrolledUV = (i.texcoord * _Tiling.xy) + float2(_ScrollXSpeed, _ScrollYSpeed) * _Time.y;

            // Lấy màu từ texture, dùng frac() để lặp lại
            fixed4 col = tex2D(_MainTex, frac(scrolledUV));

            // Áp dụng màu đỉnh/Tint
            col *= i.color;

            // Áp dụng UI Clipping (nếu bật)
            #ifdef UNITY_UI_CLIP_RECT
            // Sử dụng hàm clipping từ UnityUI.cginc
            // Hàm này cần worldPosition và _ClipRect (đã được xử lý trong vert hoặc tự động bởi include)
             col.a *= UnityGet2DClipping(i.worldPosition.xy, i.clipRect);
            #endif

             // Masking được xử lý tự động bởi thiết lập Stencil ở trên

             return col;
         }
         ENDCG
     }
        }
}

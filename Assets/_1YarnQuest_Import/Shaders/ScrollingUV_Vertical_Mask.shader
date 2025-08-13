Shader "Unlit/ScrollingUV_Vertical_Mask"
{
    Properties
    {
        _MainTex("Texture Chính (RGB)", 2D) = "white" {}
        _MaskTex("Mask (A)", 2D) = "white" {}
        _ScrollSpeed("Tốc độ cuộn dọc", Float) = 1.0
    }
        SubShader
        {
            Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
            LOD 100
            Blend SrcAlpha OneMinusSrcAlpha // Bật blend để mask hoạt động
            ZWrite Off // Tắt ghi vào depth buffer cho hiệu ứng trong suốt

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
                };

                struct v2f
                {
                    float2 uv_main : TEXCOORD0;
                    float2 uv_mask : TEXCOORD1;
                    float4 vertex : SV_POSITION;
                };

                sampler2D _MainTex;
                sampler2D _MaskTex;
                float4 _MainTex_ST; // Hỗ trợ tiling và offset cho texture chính
                float _ScrollSpeed;

                v2f vert(appdata v)
                {
                    v2f o;
                    o.vertex = UnityObjectToClipPos(v.vertex);

                    // Áp dụng tiling và offset cho texture chính
                    o.uv_main = TRANSFORM_TEX(v.uv, _MainTex);

                    // UV cho mask không cần thay đổi
                    o.uv_mask = v.uv;

                    return o;
                }

                fixed4 frag(v2f i) : SV_Target
                {
                    // Tạo hiệu ứng cuộn bằng cách thay đổi tọa độ V (chiều dọc) của UV theo thời gian
                    float2 scrolledUV = i.uv_main;
                    scrolledUV.y += _Time.y * _ScrollSpeed;

                    // Lấy màu từ texture chính với UV đã cuộn
                    fixed4 col = tex2D(_MainTex, scrolledUV);

                    // Lấy giá trị alpha từ mask
                    // Ta có thể dùng bất kỳ kênh nào (R, G, B, hoặc A) của mask
                    // Ở đây, ta dùng kênh Alpha (a)
                    fixed mask = tex2D(_MaskTex, i.uv_mask).a;

                    // Áp dụng alpha của mask vào màu cuối cùng
                    col.a *= mask;

                    return col;
                }
                ENDCG
            }
        }
}
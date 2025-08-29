Shader "Unlit/IgnoreYarn_WhiteBlackToRed"
{
    Properties
    {
        [NoScaleOffset] _MainTex ("Texture", 2D) = "white" {}
        _Progress ("Progress", Range(0, 1)) = 0
    }
    SubShader
    {
        Stencil
        {
            Ref 128
            Comp NotEqual
            Pass Keep
            Fail Keep
        }
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            #include "UnityCG.cginc"
            
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            sampler2D _MainTex;
            float _Progress;

            v2f vert (appdata v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texcol = tex2D(_MainTex, i.uv);

                // Tính độ sáng (0 = đen, 1 = trắng)
                fixed luminance = dot(texcol.rgb, fixed3(0.299, 0.587, 0.114));

                // Màu đỏ đậm và đỏ tươi
                fixed3 darkRed = fixed3(0.3, 0.0, 0.0);
                fixed3 brightRed = fixed3(1.0, 0.0, 0.0);

                // Lerp giữa đỏ đậm và đỏ tươi dựa vào độ sáng
                fixed3 targetRed = lerp(darkRed, brightRed, luminance);

                // Blend từ màu gốc sang targetRed theo _Progress
                fixed3 finalRGB = lerp(texcol.rgb, targetRed, saturate(_Progress));

                return fixed4(finalRGB, texcol.a);
            }
            ENDCG
        }
    }
}

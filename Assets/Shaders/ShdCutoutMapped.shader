Shader "Games/BeatThemAll/CutoutMapped"
{
    Properties
    {
        [NoScaleOffset]_MainTex ("Texture", 2D) = "white" {}
        _TillingOffset("Tilling Offset", Vector) = (1., 1., 0., 0.)
        _ColorRedChannel("Red channel color", Color) = (1., 1., 1., 1.)
        _ColorGreenChannel("Green channel color", Color) = (1., 1., 1., 1.)
        _ColorBlueChannel("Blue channel color", Color) = (1., 1., 1., 1.)

        //[HideInInspector]
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }

        Blend SrcAlpha OneMinusSrcAlpha

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            sampler2D _MainTex;
            float4 _MainTex_ST;

            fixed4 _TillingOffset;

            fixed4 _ColorRedChannel;
            fixed4 _ColorGreenChannel;
            fixed4 _ColorBlueChannel;

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };



            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                o.uv = o.uv.xy * _TillingOffset.xy + _TillingOffset.zw;

                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 mappedColor = tex2D(_MainTex, i.uv);
                //mappedColor = fixed4(i.uv.x, i.uv.y, 0.0, 1.0);

                clip(mappedColor.a - 0.01);

                fixed4 finalColor = _ColorRedChannel * mappedColor.r + _ColorGreenChannel * mappedColor.g + _ColorBlueChannel * mappedColor.b;
                return finalColor;
            }
            ENDCG
        }
    }
}

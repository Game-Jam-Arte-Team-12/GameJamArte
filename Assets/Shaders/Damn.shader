Shader "Custom/Damn"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Albedo (RGB)", 2D) = "white" {}


		_ColorRedChannel("Red channel color", Color) = (1., 1., 1., 1.)
        _ColorGreenChannel("Green channel color", Color) = (1., 1., 1., 1.)
        _ColorBlueChannel("Blue channel color", Color) = (1., 1., 1., 1.)

    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }

        CGPROGRAM
        // Physically based Standard lighting model, and enable shadows on all light types
        #pragma surface surf Standard fullforwardshadows

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

		            fixed4 _ColorRedChannel;
            fixed4 _ColorGreenChannel;
            fixed4 _ColorBlueChannel;

        struct Input
        {
            float2 uv_MainTex;
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 mappedColor = tex2D(_MainTex, IN.uv_MainTex);
            clip(mappedColor.a - 0.01);

            fixed4 finalColor = _ColorRedChannel * mappedColor.r + _ColorGreenChannel * mappedColor.g + _ColorBlueChannel * mappedColor.b;
            o.Albedo = finalColor.rgb;
            // Metallic and smoothness come from slider variables
            o.Alpha = finalColor.a;
        }
        ENDCG
    }
    FallBack "Diffuse"
}

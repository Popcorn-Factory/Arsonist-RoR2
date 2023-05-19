Shader "Custom/FresnelLitToon"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color("Color", Color) = (1,1,1,1)
        _FresnelColor ("Fresnel Color", Color) = (1,1,1,1)
        _FresnelExponent ("Fresnel Exponent", Range(0, 10)) = 1
        _MainTex2 ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0
        [HDR] _Emission ("Emission", color) = (0,0,0)
        _RimAmount("Rim Amount", Range(0,1)) = 0.716
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 200
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull Off

        CGPROGRAM
        //Disable lighting and make it transparent.
        #pragma surface surf Standard alpha

        // Use shader model 3.0 target, to get nicer looking lighting
        #pragma target 3.0

        sampler2D _MainTex;

        struct Input
        {
            float2 uv_MainTex;
            float3 worldNormal;
            float3 viewDir;
            INTERNAL_DATA
        };

        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Emission;
        float4 _FresnelColor;
        float _FresnelExponent;
        float _RimAmount;

        // Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
        // See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
        // #pragma instancing_options assumeuniformscaling
        UNITY_INSTANCING_BUFFER_START(Props)
            // put more per-instance properties here
        UNITY_INSTANCING_BUFFER_END(Props)

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            // Albedo comes from a texture tinted by color
            fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
            float fresnel = dot(IN.worldNormal, IN.viewDir);
            float fresnelStore = dot(IN.worldNormal, IN.viewDir);
            fresnel = saturate(1 - fresnel);
            fresnel = pow(fresnel, _FresnelExponent);
            float3 fresnelColor = fresnel * _FresnelColor;

            float toonFresnelModifier = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, 1 - fresnelStore);
            float3 fresnelColorToon = _FresnelColor * toonFresnelModifier;



            o.Albedo = c.rgb;
            // Metallic and smoothness come from slider variables
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
            if (fresnelStore < 0.02)
            {
                o.Alpha = 0.2;
            }
            else
            {
                o.Alpha = smoothstep(_RimAmount - 0.01, _RimAmount + 0.01, 1 - fresnelStore) * _Color.a;
            }
            o.Emission = _Emission * fresnelColorToon;
        }
        ENDCG
    }
    FallBack "Standard"
}

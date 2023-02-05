// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'
// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/Frensel"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Bias ("Bias", Range(0,1)) = 0.1
        _Scale ("Scale", Range(0,1)) = 0.1
        _Power ("Power", Range(0,1)) = 0.1
        _Speed ("Speed of Transition", Range(0, 100)) = 1.0
        _SpeedTex ("Movement of Tex", Range(0, 100)) = 1.0
        _Color ("Color", Color) = (0, 0, 0, 1)
        _VertexWobble ("Wobble Amplitude", Range(0, 10)) = 0.03
        _VertexFrequency ("Wobble Frequency", Range(0, 100)) = 20
        _VertexTimeMultiplier ("Wobble Speed", Range(0, 100)) = 1
    }
    SubShader
    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha
        Cull back

        Pass
        {
            CGPROGRAM
// Upgrade NOTE: excluded shader from DX11; has structs without semantics (struct appdata members normal)
//          #pragma exclude_renderers d3d11
            #pragma vertex vert
            #pragma fragment frag
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
                float R : COLOR;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
            float _Bias;
            float _Scale;
            float _Power;
            float4 _Color;
            float _Speed;
            float _SpeedTex;
            float _VertexWobble;
            float _VertexFrequency;
            float _VertexTimeMultiplier;

            v2f vert(appdata v)
            { 
                v2f o;
                float4 transformedVertex = float4(v.vertex.x, v.vertex.y, v.vertex.z, v.vertex.w);
                transformedVertex.x = v.vertex.x + sin(v.vertex.y * _VertexFrequency + _Time *  _VertexTimeMultiplier * _VertexFrequency) * _VertexWobble;
                transformedVertex.z = v.vertex.z + cos(v.vertex.y * _VertexFrequency + _Time * _VertexTimeMultiplier * _VertexFrequency) * _VertexWobble;
                o.vertex = UnityObjectToClipPos(transformedVertex);
                o.uv = v.uv;

                float3 posWorld = mul(unity_ObjectToWorld, transformedVertex).xyz;
                float3 bruh = mul((unity_ObjectToWorld), v.normal);
                float3 normWorld = normalize(bruh);

                float3 I = normalize(posWorld - _WorldSpaceCameraPos.xyz);
                float addition = sin(_Time * _Speed);
                o.R = _Bias + (0.5 + addition + _Scale) * pow(1.0 + dot(I, normWorld), _Power);
                //o.R = max(0, min(1, _Bias + _Scale * pow(1 + dot(I, normWorld), _Power)));
                return o;
            }

            float4 frag(v2f i) :  COLOR
            {  
                float2 texColModif = float2((i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw + (_Time.x * _SpeedTex % 2)).x, (i.uv.xy * _MainTex_ST.xy + _MainTex_ST.zw + sin(_Time.x * _SpeedTex) / 2).y);
                float4 col = tex2D(_MainTex, texColModif) * _Color;
                return lerp(col, _Color, i.R);

                // * sin(_Time * _Speed)
            }
            ENDCG
        }
    }
}

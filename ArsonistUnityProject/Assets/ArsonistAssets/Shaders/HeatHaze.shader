// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:True,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-8836-RGB,alpha-3592-OUT,voffset-1895-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31791,y:32840,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:abecc60f88f8b8f43887d7bc037ee040,ntxv:0,isnm:False|UVIN-6839-UVOUT;n:type:ShaderForge.SFN_SceneColor,id:8836,x:31814,y:33164,varname:node_8836,prsc:2|UVIN-8611-OUT;n:type:ShaderForge.SFN_TexCoord,id:8908,x:31172,y:32823,varname:node_8908,prsc:2,uv:0,uaff:True;n:type:ShaderForge.SFN_Time,id:5780,x:30990,y:32726,varname:node_5780,prsc:2;n:type:ShaderForge.SFN_Slider,id:6730,x:30894,y:32589,ptovrint:False,ptlb:Time Multiplier,ptin:_TimeMultiplier,varname:node_6730,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.2,max:50;n:type:ShaderForge.SFN_Multiply,id:3660,x:31498,y:32557,varname:node_3660,prsc:2|A-6730-OUT,B-5780-T;n:type:ShaderForge.SFN_RemapRange,id:6388,x:31677,y:32557,varname:node_6388,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-3660-OUT;n:type:ShaderForge.SFN_Panner,id:6839,x:31556,y:32847,varname:node_6839,prsc:2,spu:1,spv:1|UVIN-8908-UVOUT,DIST-6388-OUT;n:type:ShaderForge.SFN_Lerp,id:8611,x:31600,y:33140,varname:node_8611,prsc:2|A-1134-UVOUT,B-399-OUT,T-2874-OUT;n:type:ShaderForge.SFN_ValueProperty,id:2874,x:31321,y:33331,ptovrint:False,ptlb:Heat Distortion,ptin:_HeatDistortion,varname:node_2874,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_ScreenPos,id:1134,x:31139,y:33210,varname:node_1134,prsc:2,sctp:2;n:type:ShaderForge.SFN_Tex2d,id:3422,x:31012,y:32990,ptovrint:False,ptlb:Mask,ptin:_Mask,varname:node_3422,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:3a5a96df060a5cf4a9cc0c59e13486b7,ntxv:1,isnm:False;n:type:ShaderForge.SFN_Multiply,id:399,x:31345,y:33019,varname:node_399,prsc:2|A-6074-R,B-3422-G;n:type:ShaderForge.SFN_Fresnel,id:1017,x:31976,y:33002,varname:node_1017,prsc:2|EXP-4297-OUT;n:type:ShaderForge.SFN_Vector1,id:4297,x:31756,y:33016,varname:node_4297,prsc:2,v1:1;n:type:ShaderForge.SFN_OneMinus,id:3592,x:32279,y:33068,varname:node_3592,prsc:2|IN-1017-OUT;n:type:ShaderForge.SFN_NormalVector,id:4388,x:32048,y:33219,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:1895,x:32391,y:33176,varname:node_1895,prsc:2|A-6074-RGB,B-4388-OUT;proporder:6074-6730-2874-3422;pass:END;sub:END;*/

Shader "Shader Forge/HeatHaze" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TimeMultiplier ("Time Multiplier", Range(0, 50)) = 0.2
        _HeatDistortion ("Heat Distortion", Float ) = 0.2
        _Mask ("Mask", 2D) = "gray" {}
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        GrabPass{ }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            ZWrite Off
            
            AlphaToMask On
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _MainTex; uniform float4 _MainTex_ST;
            uniform sampler2D _Mask; uniform float4 _Mask_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _HeatDistortion)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float4 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 uv0 : TEXCOORD0;
                float4 posWorld : TEXCOORD1;
                float3 normalDir : TEXCOORD2;
                float4 projPos : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_5780 = _Time;
                float2 node_6839 = (o.uv0+((_TimeMultiplier_var*node_5780.g)*2.0+-1.0)*float2(1,1));
                float4 _MainTex_var = tex2Dlod(_MainTex,float4(TRANSFORM_TEX(node_6839, _MainTex),0.0,0));
                v.vertex.xyz += (_MainTex_var.rgb*v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_5780 = _Time;
                float2 node_6839 = (i.uv0+((_TimeMultiplier_var*node_5780.g)*2.0+-1.0)*float2(1,1));
                float4 _MainTex_var = tex2D(_MainTex,TRANSFORM_TEX(node_6839, _MainTex));
                float4 _Mask_var = tex2D(_Mask,TRANSFORM_TEX(i.uv0, _Mask));
                float node_399 = (_MainTex_var.r*_Mask_var.g);
                float _HeatDistortion_var = UNITY_ACCESS_INSTANCED_PROP( Props, _HeatDistortion );
                float3 emissive = tex2D( _GrabTexture, lerp(sceneUVs.rg,float2(node_399,node_399),_HeatDistortion_var)).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,(1.0 - pow(1.0-max(0,dot(normalDirection, viewDirection)),1.0)));
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

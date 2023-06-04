// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:2,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32888,y:32567,varname:node_3138,prsc:2|diffpow-9992-OUT,emission-5862-OUT,alpha-1884-OUT,voffset-9388-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31838,y:32724,ptovrint:False,ptlb:TextureColor,ptin:_TextureColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5681818,c3:0,c4:0.1;n:type:ShaderForge.SFN_Tex2dAsset,id:8115,x:31465,y:32502,ptovrint:False,ptlb:TextureInput,ptin:_TextureInput,varname:node_8115,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:f2aee5fafb274e649b1070cbbe01b7bb,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:17,x:31861,y:32432,varname:node_17,prsc:2,tex:f2aee5fafb274e649b1070cbbe01b7bb,ntxv:0,isnm:False|UVIN-8834-UVOUT,TEX-8115-TEX;n:type:ShaderForge.SFN_Add,id:5858,x:32388,y:32649,varname:node_5858,prsc:2|A-6340-OUT,B-7241-RGB;n:type:ShaderForge.SFN_TexCoord,id:174,x:31299,y:32363,varname:node_174,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:8834,x:31617,y:32349,varname:node_8834,prsc:2,spu:1,spv:1|UVIN-174-UVOUT,DIST-2794-OUT;n:type:ShaderForge.SFN_Time,id:7558,x:31248,y:32083,varname:node_7558,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:2794,x:31798,y:32036,varname:node_2794,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1|IN-3845-OUT;n:type:ShaderForge.SFN_NormalVector,id:3190,x:31850,y:33038,prsc:2,pt:True;n:type:ShaderForge.SFN_Multiply,id:9603,x:32235,y:32931,varname:node_9603,prsc:2|A-17-RGB,B-3190-OUT;n:type:ShaderForge.SFN_Multiply,id:9388,x:32615,y:33184,varname:node_9388,prsc:2|A-6083-OUT,B-9603-OUT;n:type:ShaderForge.SFN_Slider,id:6083,x:31850,y:33236,ptovrint:False,ptlb:Vertex offset,ptin:_Vertexoffset,varname:node_6083,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1574405,max:1;n:type:ShaderForge.SFN_Multiply,id:3845,x:31585,y:32053,varname:node_3845,prsc:2|A-7009-OUT,B-7558-T;n:type:ShaderForge.SFN_Slider,id:7009,x:31248,y:32003,ptovrint:False,ptlb:Time Multiplier,ptin:_TimeMultiplier,varname:node_7009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:100;n:type:ShaderForge.SFN_Slider,id:4069,x:32174,y:32382,ptovrint:False,ptlb:Frenel Multiplier,ptin:_FrenelMultiplier,varname:node_4069,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:5,max:10;n:type:ShaderForge.SFN_Multiply,id:4712,x:32388,y:32829,varname:node_4712,prsc:2|A-17-A,B-405-OUT;n:type:ShaderForge.SFN_Step,id:6340,x:31990,y:32600,varname:node_6340,prsc:2|A-17-R,B-4699-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4699,x:31642,y:32648,ptovrint:False,ptlb:Step Threshold,ptin:_StepThreshold,varname:node_4699,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0;n:type:ShaderForge.SFN_Slider,id:1182,x:30758,y:32836,ptovrint:False,ptlb:LowerLerpOpacity,ptin:_LowerLerpOpacity,varname:node_1182,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Slider,id:9253,x:30723,y:32959,ptovrint:False,ptlb:UpperLerpOpacity,ptin:_UpperLerpOpacity,varname:node_9253,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:1;n:type:ShaderForge.SFN_Lerp,id:405,x:31413,y:32858,varname:node_405,prsc:2|A-1182-OUT,B-9253-OUT,T-9959-OUT;n:type:ShaderForge.SFN_Fmod,id:9959,x:31108,y:33076,varname:node_9959,prsc:2|A-4906-OUT,B-7034-OUT;n:type:ShaderForge.SFN_Vector1,id:7034,x:30830,y:33255,varname:node_7034,prsc:2,v1:2;n:type:ShaderForge.SFN_Sin,id:1157,x:30601,y:33128,varname:node_1157,prsc:2|IN-3845-OUT;n:type:ShaderForge.SFN_Clamp01,id:4906,x:30815,y:33106,varname:node_4906,prsc:2|IN-1157-OUT;n:type:ShaderForge.SFN_Fresnel,id:9992,x:32507,y:32355,varname:node_9992,prsc:2|NRM-3190-OUT,EXP-4069-OUT;n:type:ShaderForge.SFN_Multiply,id:5862,x:32669,y:32664,varname:node_5862,prsc:2|A-5858-OUT,B-7533-OUT;n:type:ShaderForge.SFN_Multiply,id:7533,x:32855,y:32420,varname:node_7533,prsc:2|A-9992-OUT,B-7241-RGB;n:type:ShaderForge.SFN_Slider,id:2837,x:31786,y:33402,ptovrint:False,ptlb:Depth Distance,ptin:_DepthDistance,varname:node_2837,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0,max:100;n:type:ShaderForge.SFN_Subtract,id:1155,x:32474,y:33362,varname:node_1155,prsc:2|A-2942-W,B-6966-OUT;n:type:ShaderForge.SFN_Smoothstep,id:5977,x:33016,y:33350,varname:node_5977,prsc:2|A-6658-OUT,B-1931-OUT,V-1155-OUT;n:type:ShaderForge.SFN_Vector1,id:6658,x:32843,y:33287,varname:node_6658,prsc:2,v1:0;n:type:ShaderForge.SFN_Vector1,id:1931,x:32788,y:33350,varname:node_1931,prsc:2,v1:1;n:type:ShaderForge.SFN_FragmentPosition,id:2942,x:32132,y:33279,varname:node_2942,prsc:2;n:type:ShaderForge.SFN_Add,id:1884,x:32521,y:32947,varname:node_1884,prsc:2|A-9992-OUT,B-5977-OUT;n:type:ShaderForge.SFN_SceneDepth,id:1247,x:32172,y:33436,varname:node_1247,prsc:2;n:type:ShaderForge.SFN_DepthBlend,id:6966,x:32079,y:33573,varname:node_6966,prsc:2|DIST-2837-OUT;proporder:7241-8115-6083-7009-4069-4699-1182-9253-2837;pass:END;sub:END;*/

Shader "Shader Forge/masochismWaveShader2" {
    Properties {
        _TextureColor ("TextureColor", Color) = (1,0.5681818,0,0.1)
        _TextureInput ("TextureInput", 2D) = "white" {}
        _Vertexoffset ("Vertex offset", Range(0, 1)) = 0.1574405
        _TimeMultiplier ("Time Multiplier", Range(0, 100)) = 1
        _FrenelMultiplier ("Frenel Multiplier", Range(0, 10)) = 5
        _StepThreshold ("Step Threshold", Float ) = 0
        _LowerLerpOpacity ("LowerLerpOpacity", Range(0, 1)) = 0
        _UpperLerpOpacity ("UpperLerpOpacity", Range(0, 1)) = 0
        _DepthDistance ("Depth Distance", Range(0, 100)) = 0
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _CameraDepthTexture;
            uniform sampler2D _TextureInput; uniform float4 _TextureInput_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TextureColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _FrenelMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _StepThreshold)
                UNITY_DEFINE_INSTANCED_PROP( float, _DepthDistance)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
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
                float _Vertexoffset_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Vertexoffset );
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float node_3845 = (_TimeMultiplier_var*node_7558.g);
                float2 node_8834 = (o.uv0+(node_3845*1.0+0.0)*float2(1,1));
                float4 node_17 = tex2Dlod(_TextureInput,float4(TRANSFORM_TEX(node_8834, _TextureInput),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_17.rgb*v.normal));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float4x4 bbmv = UNITY_MATRIX_MV;
                bbmv._m00 = -1.0/length(unity_WorldToObject[0].xyz);
                bbmv._m10 = 0.0f;
                bbmv._m20 = 0.0f;
                bbmv._m01 = 0.0f;
                bbmv._m11 = -1.0/length(unity_WorldToObject[1].xyz);
                bbmv._m21 = 0.0f;
                bbmv._m02 = 0.0f;
                bbmv._m12 = 0.0f;
                bbmv._m22 = -1.0/length(unity_WorldToObject[2].xyz);
                o.pos = mul( UNITY_MATRIX_P, mul( bbmv, v.vertex ));
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float sceneZ = max(0,LinearEyeDepth (UNITY_SAMPLE_DEPTH(tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(i.projPos)))) - _ProjectionParams.g);
                float partZ = max(0,i.projPos.z - _ProjectionParams.g);
////// Lighting:
////// Emissive:
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float node_3845 = (_TimeMultiplier_var*node_7558.g);
                float2 node_8834 = (i.uv0+(node_3845*1.0+0.0)*float2(1,1));
                float4 node_17 = tex2D(_TextureInput,TRANSFORM_TEX(node_8834, _TextureInput));
                float _StepThreshold_var = UNITY_ACCESS_INSTANCED_PROP( Props, _StepThreshold );
                float4 _TextureColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TextureColor );
                float _FrenelMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _FrenelMultiplier );
                float node_9992 = pow(1.0-max(0,dot(normalDirection, viewDirection)),_FrenelMultiplier_var);
                float3 node_7533 = (node_9992*_TextureColor_var.rgb);
                float3 emissive = ((step(node_17.r,_StepThreshold_var)+_TextureColor_var.rgb)*node_7533);
                float3 finalColor = emissive;
                float _DepthDistance_var = UNITY_ACCESS_INSTANCED_PROP( Props, _DepthDistance );
                float node_5977 = smoothstep( 0.0, 1.0, (i.posWorld.a-saturate((sceneZ-partZ)/_DepthDistance_var)) );
                return fixed4(finalColor,(node_9992+node_5977));
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            uniform sampler2D _TextureInput; uniform float4 _TextureInput_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD1;
                float4 posWorld : TEXCOORD2;
                float3 normalDir : TEXCOORD3;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                float _Vertexoffset_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Vertexoffset );
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float node_3845 = (_TimeMultiplier_var*node_7558.g);
                float2 node_8834 = (o.uv0+(node_3845*1.0+0.0)*float2(1,1));
                float4 node_17 = tex2Dlod(_TextureInput,float4(TRANSFORM_TEX(node_8834, _TextureInput),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_17.rgb*v.normal));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float4x4 bbmv = UNITY_MATRIX_MV;
                bbmv._m00 = -1.0/length(unity_WorldToObject[0].xyz);
                bbmv._m10 = 0.0f;
                bbmv._m20 = 0.0f;
                bbmv._m01 = 0.0f;
                bbmv._m11 = -1.0/length(unity_WorldToObject[1].xyz);
                bbmv._m21 = 0.0f;
                bbmv._m02 = 0.0f;
                bbmv._m12 = 0.0f;
                bbmv._m22 = -1.0/length(unity_WorldToObject[2].xyz);
                o.pos = mul( UNITY_MATRIX_P, mul( bbmv, v.vertex ));
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                i.normalDir = normalize(i.normalDir);
                i.normalDir *= faceSign;
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

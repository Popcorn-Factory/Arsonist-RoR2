// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:2,spmd:0,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:2,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32888,y:32567,varname:node_3138,prsc:2|diff-5858-OUT,diffpow-4069-OUT,emission-5858-OUT,alpha-6350-OUT,voffset-9388-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31941,y:32619,ptovrint:False,ptlb:TextureColor,ptin:_TextureColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5681818,c3:0,c4:0.1;n:type:ShaderForge.SFN_Tex2dAsset,id:8115,x:31782,y:32533,ptovrint:False,ptlb:TextureInput,ptin:_TextureInput,varname:node_8115,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9337cc1a2c6d9244bb19033e87030afc,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:17,x:32147,y:32459,varname:node_17,prsc:2,tex:9337cc1a2c6d9244bb19033e87030afc,ntxv:0,isnm:False|UVIN-8834-UVOUT,TEX-8115-TEX;n:type:ShaderForge.SFN_Slider,id:6350,x:31795,y:32829,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_exponent_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.3793022,max:1;n:type:ShaderForge.SFN_Add,id:5858,x:32412,y:32672,varname:node_5858,prsc:2|A-17-RGB,B-7241-RGB;n:type:ShaderForge.SFN_TexCoord,id:174,x:31644,y:32359,varname:node_174,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:8834,x:31968,y:32374,varname:node_8834,prsc:2,spu:1,spv:1|UVIN-174-UVOUT,DIST-2794-OUT;n:type:ShaderForge.SFN_Time,id:7558,x:31270,y:32244,varname:node_7558,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:2794,x:31755,y:32181,varname:node_2794,prsc:2,frmn:0,frmx:1,tomn:-1,tomx:1|IN-3119-OUT;n:type:ShaderForge.SFN_NormalVector,id:3190,x:31929,y:33038,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:9603,x:32235,y:32931,varname:node_9603,prsc:2|A-17-RGB,B-3190-OUT;n:type:ShaderForge.SFN_Multiply,id:9388,x:32531,y:33018,varname:node_9388,prsc:2|A-6083-OUT,B-9603-OUT;n:type:ShaderForge.SFN_Slider,id:6083,x:32157,y:33091,ptovrint:False,ptlb:Vertex offset,ptin:_Vertexoffset,varname:node_6083,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1022896,max:1;n:type:ShaderForge.SFN_Multiply,id:3845,x:31570,y:31987,varname:node_3845,prsc:2|A-7009-OUT,B-7558-T;n:type:ShaderForge.SFN_Slider,id:7009,x:31113,y:32075,ptovrint:False,ptlb:Time Multiplier,ptin:_TimeMultiplier,varname:node_7009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:300;n:type:ShaderForge.SFN_Sin,id:3119,x:31801,y:31989,varname:node_3119,prsc:2|IN-3845-OUT;n:type:ShaderForge.SFN_VertexColor,id:1582,x:32391,y:32245,varname:node_1582,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2188,x:32579,y:32319,varname:node_2188,prsc:2|A-1582-RGB,B-4069-OUT;n:type:ShaderForge.SFN_Slider,id:4069,x:32174,y:32382,ptovrint:False,ptlb:Lighting Multiplier,ptin:_LightingMultiplier,varname:node_4069,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.03489512,max:1;proporder:7241-8115-6350-6083-7009-4069;pass:END;sub:END;*/

Shader "Shader Forge/masochismWaveShader" {
    Properties {
        _TextureColor ("TextureColor", Color) = (1,0.5681818,0,0.1)
        _TextureInput ("TextureInput", 2D) = "black" {}
        _Opacity ("Opacity", Range(0, 1)) = 0.3793022
        _Vertexoffset ("Vertex offset", Range(0, 1)) = 0.1022896
        _TimeMultiplier ("Time Multiplier", Range(0, 300)) = 1
        _LightingMultiplier ("Lighting Multiplier", Range(0, 1)) = 0.03489512
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
            uniform float4 _LightColor0;
            uniform sampler2D _TextureInput; uniform float4 _TextureInput_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TextureColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Opacity)
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _LightingMultiplier)
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
                float2 node_8834 = (o.uv0+(sin((_TimeMultiplier_var*node_7558.g))*2.0+-1.0)*float2(1,1));
                float4 node_17 = tex2Dlod(_TextureInput,float4(TRANSFORM_TEX(node_8834, _TextureInput),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_17.rgb*v.normal));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
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
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = 1;
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float _LightingMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _LightingMultiplier );
                float3 directDiffuse = pow(max( 0.0, NdotL), _LightingMultiplier_var) * attenColor;
                float3 indirectDiffuse = float3(0,0,0);
                indirectDiffuse += UNITY_LIGHTMODEL_AMBIENT.rgb; // Ambient Light
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float2 node_8834 = (i.uv0+(sin((_TimeMultiplier_var*node_7558.g))*2.0+-1.0)*float2(1,1));
                float4 node_17 = tex2D(_TextureInput,TRANSFORM_TEX(node_8834, _TextureInput));
                float4 _TextureColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TextureColor );
                float3 node_5858 = (node_17.rgb+_TextureColor_var.rgb);
                float3 diffuseColor = node_5858;
                float3 diffuse = (directDiffuse + indirectDiffuse) * diffuseColor;
////// Emissive:
                float3 emissive = node_5858;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                float _Opacity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Opacity );
                return fixed4(finalColor,_Opacity_var);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
            Cull Off
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "AutoLight.cginc"
            #pragma multi_compile_fwdadd
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _TextureInput; uniform float4 _TextureInput_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TextureColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Opacity)
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _LightingMultiplier)
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
                LIGHTING_COORDS(3,4)
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
                float2 node_8834 = (o.uv0+(sin((_TimeMultiplier_var*node_7558.g))*2.0+-1.0)*float2(1,1));
                float4 node_17 = tex2Dlod(_TextureInput,float4(TRANSFORM_TEX(node_8834, _TextureInput),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_17.rgb*v.normal));
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                float3 lightColor = _LightColor0.rgb;
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
                TRANSFER_VERTEX_TO_FRAGMENT(o)
                return o;
            }
            float4 frag(VertexOutput i, float facing : VFACE) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float isFrontFace = ( facing >= 0 ? 1 : 0 );
                float faceSign = ( facing >= 0 ? 1 : -1 );
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
                float3 lightDirection = normalize(lerp(_WorldSpaceLightPos0.xyz, _WorldSpaceLightPos0.xyz - i.posWorld.xyz,_WorldSpaceLightPos0.w));
                float3 lightColor = _LightColor0.rgb;
////// Lighting:
                float attenuation = LIGHT_ATTENUATION(i);
                float3 attenColor = attenuation * _LightColor0.xyz;
/////// Diffuse:
                float NdotL = max(0.0,dot( normalDirection, lightDirection ));
                float _LightingMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _LightingMultiplier );
                float3 directDiffuse = pow(max( 0.0, NdotL), _LightingMultiplier_var) * attenColor;
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float2 node_8834 = (i.uv0+(sin((_TimeMultiplier_var*node_7558.g))*2.0+-1.0)*float2(1,1));
                float4 node_17 = tex2D(_TextureInput,TRANSFORM_TEX(node_8834, _TextureInput));
                float4 _TextureColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TextureColor );
                float3 node_5858 = (node_17.rgb+_TextureColor_var.rgb);
                float3 diffuseColor = node_5858;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float _Opacity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Opacity );
                return fixed4(finalColor * _Opacity_var,0);
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
                float2 node_8834 = (o.uv0+(sin((_TimeMultiplier_var*node_7558.g))*2.0+-1.0)*float2(1,1));
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

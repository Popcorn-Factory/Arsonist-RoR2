// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:2,spmd:0,trmd:0,grmd:0,uamb:False,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:2,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:4,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32888,y:32567,varname:node_3138,prsc:2|diff-5858-OUT,diffpow-4069-OUT,emission-5858-OUT,alpha-4712-OUT,voffset-9388-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:31936,y:32659,ptovrint:False,ptlb:TextureColor,ptin:_TextureColor,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5681818,c3:0,c4:0.1;n:type:ShaderForge.SFN_Tex2dAsset,id:8115,x:31258,y:32655,ptovrint:False,ptlb:ColourTexture,ptin:_ColourTexture,varname:node_8115,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:9337cc1a2c6d9244bb19033e87030afc,ntxv:3,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:17,x:31826,y:32349,varname:node_17,prsc:2,tex:9337cc1a2c6d9244bb19033e87030afc,ntxv:0,isnm:False|UVIN-8834-UVOUT,TEX-8115-TEX;n:type:ShaderForge.SFN_Slider,id:6350,x:31795,y:32829,ptovrint:False,ptlb:Opacity,ptin:_Opacity,varname:_exponent_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Add,id:5858,x:32331,y:32672,varname:node_5858,prsc:2|A-6340-OUT,B-7241-RGB;n:type:ShaderForge.SFN_TexCoord,id:174,x:31299,y:32363,varname:node_174,prsc:2,uv:0,uaff:False;n:type:ShaderForge.SFN_Panner,id:8834,x:31617,y:32349,varname:node_8834,prsc:2,spu:1,spv:1|UVIN-174-UVOUT,DIST-2794-OUT;n:type:ShaderForge.SFN_Time,id:7558,x:31248,y:32083,varname:node_7558,prsc:2;n:type:ShaderForge.SFN_RemapRange,id:2794,x:31798,y:32036,varname:node_2794,prsc:2,frmn:0,frmx:1,tomn:0,tomx:1|IN-3845-OUT;n:type:ShaderForge.SFN_NormalVector,id:3190,x:31929,y:33038,prsc:2,pt:False;n:type:ShaderForge.SFN_Multiply,id:9603,x:32235,y:32931,varname:node_9603,prsc:2|A-3204-RGB,B-3190-OUT;n:type:ShaderForge.SFN_Multiply,id:9388,x:32531,y:33018,varname:node_9388,prsc:2|A-6083-OUT,B-9603-OUT;n:type:ShaderForge.SFN_Slider,id:6083,x:32157,y:33091,ptovrint:False,ptlb:Vertex offset,ptin:_Vertexoffset,varname:node_6083,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.1574405,max:1;n:type:ShaderForge.SFN_Multiply,id:3845,x:31585,y:32053,varname:node_3845,prsc:2|A-7009-OUT,B-7558-T;n:type:ShaderForge.SFN_Slider,id:7009,x:31248,y:32003,ptovrint:False,ptlb:Time Multiplier,ptin:_TimeMultiplier,varname:node_7009,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:100;n:type:ShaderForge.SFN_VertexColor,id:1582,x:32391,y:32245,varname:node_1582,prsc:2;n:type:ShaderForge.SFN_Multiply,id:2188,x:32579,y:32319,varname:node_2188,prsc:2|A-1582-RGB,B-4069-OUT;n:type:ShaderForge.SFN_Slider,id:4069,x:32174,y:32382,ptovrint:False,ptlb:Lighting Multiplier,ptin:_LightingMultiplier,varname:node_4069,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:1,max:1;n:type:ShaderForge.SFN_Multiply,id:4712,x:32412,y:32804,varname:node_4712,prsc:2|A-17-A,B-6350-OUT;n:type:ShaderForge.SFN_Step,id:6340,x:32129,y:32496,varname:node_6340,prsc:2|A-17-R,B-4699-OUT;n:type:ShaderForge.SFN_ValueProperty,id:4699,x:31681,y:32735,ptovrint:False,ptlb:Step Threshold,ptin:_StepThreshold,varname:node_4699,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.2;n:type:ShaderForge.SFN_Tex2dAsset,id:7742,x:31347,y:32933,ptovrint:False,ptlb:Vertex Offset Tex,ptin:_VertexOffsetTex,varname:node_7742,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:bec67970a65e7284bb2c617657e228ae,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:3204,x:31577,y:32950,varname:node_3204,prsc:2,tex:bec67970a65e7284bb2c617657e228ae,ntxv:0,isnm:False|UVIN-8834-UVOUT,TEX-7742-TEX;proporder:7241-8115-6350-6083-7009-4069-4699-7742;pass:END;sub:END;*/

Shader "Shader Forge/FireballShader" {
    Properties {
        _TextureColor ("TextureColor", Color) = (1,0.5681818,0,0.1)
        _ColourTexture ("ColourTexture", 2D) = "bump" {}
        _Opacity ("Opacity", Range(0, 1)) = 1
        _Vertexoffset ("Vertex offset", Range(0, 1)) = 0.1574405
        _TimeMultiplier ("Time Multiplier", Range(0, 100)) = 1
        _LightingMultiplier ("Lighting Multiplier", Range(0, 1)) = 1
        _StepThreshold ("Step Threshold", Float ) = 0.2
        _VertexOffsetTex ("Vertex Offset Tex", 2D) = "white" {}
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
            Blend DstColor Zero
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform float4 _LightColor0;
            uniform sampler2D _ColourTexture; uniform float4 _ColourTexture_ST;
            uniform sampler2D _VertexOffsetTex; uniform float4 _VertexOffsetTex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TextureColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Opacity)
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _LightingMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _StepThreshold)
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
                float2 node_8834 = (o.uv0+((_TimeMultiplier_var*node_7558.g)*1.0+0.0)*float2(1,1));
                float4 node_3204 = tex2Dlod(_VertexOffsetTex,float4(TRANSFORM_TEX(node_8834, _VertexOffsetTex),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_3204.rgb*v.normal));
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
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
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
                float _TimeMultiplier_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TimeMultiplier );
                float4 node_7558 = _Time;
                float2 node_8834 = (i.uv0+((_TimeMultiplier_var*node_7558.g)*1.0+0.0)*float2(1,1));
                float4 node_17 = tex2D(_ColourTexture,TRANSFORM_TEX(node_8834, _ColourTexture));
                float _StepThreshold_var = UNITY_ACCESS_INSTANCED_PROP( Props, _StepThreshold );
                float4 _TextureColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TextureColor );
                float3 node_5858 = (step(node_17.r,_StepThreshold_var)+_TextureColor_var.rgb);
                float3 diffuseColor = node_5858;
                float3 diffuse = directDiffuse * diffuseColor;
////// Emissive:
                float3 emissive = node_5858;
/// Final Color:
                float3 finalColor = diffuse + emissive;
                float _Opacity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Opacity );
                return fixed4(finalColor,(node_17.a*_Opacity_var));
            }
            ENDCG
        }
        Pass {
            Name "FORWARD_DELTA"
            Tags {
                "LightMode"="ForwardAdd"
            }
            Blend One One
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
            uniform sampler2D _ColourTexture; uniform float4 _ColourTexture_ST;
            uniform sampler2D _VertexOffsetTex; uniform float4 _VertexOffsetTex_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _TextureColor)
                UNITY_DEFINE_INSTANCED_PROP( float, _Opacity)
                UNITY_DEFINE_INSTANCED_PROP( float, _Vertexoffset)
                UNITY_DEFINE_INSTANCED_PROP( float, _TimeMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _LightingMultiplier)
                UNITY_DEFINE_INSTANCED_PROP( float, _StepThreshold)
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
                float2 node_8834 = (o.uv0+((_TimeMultiplier_var*node_7558.g)*1.0+0.0)*float2(1,1));
                float4 node_3204 = tex2Dlod(_VertexOffsetTex,float4(TRANSFORM_TEX(node_8834, _VertexOffsetTex),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_3204.rgb*v.normal));
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
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
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
                float2 node_8834 = (i.uv0+((_TimeMultiplier_var*node_7558.g)*1.0+0.0)*float2(1,1));
                float4 node_17 = tex2D(_ColourTexture,TRANSFORM_TEX(node_8834, _ColourTexture));
                float _StepThreshold_var = UNITY_ACCESS_INSTANCED_PROP( Props, _StepThreshold );
                float4 _TextureColor_var = UNITY_ACCESS_INSTANCED_PROP( Props, _TextureColor );
                float3 node_5858 = (step(node_17.r,_StepThreshold_var)+_TextureColor_var.rgb);
                float3 diffuseColor = node_5858;
                float3 diffuse = directDiffuse * diffuseColor;
/// Final Color:
                float3 finalColor = diffuse;
                float _Opacity_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Opacity );
                return fixed4(finalColor * (node_17.a*_Opacity_var),0);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Back
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            uniform sampler2D _VertexOffsetTex; uniform float4 _VertexOffsetTex_ST;
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
                float2 node_8834 = (o.uv0+((_TimeMultiplier_var*node_7558.g)*1.0+0.0)*float2(1,1));
                float4 node_3204 = tex2Dlod(_VertexOffsetTex,float4(TRANSFORM_TEX(node_8834, _VertexOffsetTex),0.0,0));
                v.vertex.xyz += (_Vertexoffset_var*(node_3204.rgb*v.normal));
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
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float3 normalDirection = i.normalDir;
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

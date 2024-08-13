// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-9188-OUT,alpha-3458-OUT;n:type:ShaderForge.SFN_Color,id:7241,x:32008,y:32906,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0,c2:1,c3:0.5897269,c4:1;n:type:ShaderForge.SFN_Fresnel,id:5051,x:32132,y:32714,varname:node_5051,prsc:2|EXP-8221-OUT;n:type:ShaderForge.SFN_ValueProperty,id:8221,x:31960,y:32735,ptovrint:False,ptlb:fresnel exp,ptin:_fresnelexp,varname:node_8221,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Multiply,id:9188,x:32537,y:32822,varname:node_9188,prsc:2|A-7241-RGB,B-6989-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1876,x:31941,y:33111,ptovrint:False,ptlb:opacity fresnel exp,ptin:_opacityfresnelexp,varname:_node_8221_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:1;n:type:ShaderForge.SFN_Fresnel,id:4852,x:32137,y:33066,varname:node_4852,prsc:2|EXP-1876-OUT;n:type:ShaderForge.SFN_Multiply,id:3458,x:32468,y:32965,varname:node_3458,prsc:2|A-7241-A,B-3871-OUT;n:type:ShaderForge.SFN_Posterize,id:6989,x:32314,y:32724,varname:node_6989,prsc:2|IN-5051-OUT,STPS-5338-OUT;n:type:ShaderForge.SFN_ValueProperty,id:5338,x:31960,y:32833,ptovrint:False,ptlb:posterize steps,ptin:_posterizesteps,varname:node_5338,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:4;n:type:ShaderForge.SFN_Posterize,id:3871,x:32305,y:33066,varname:node_3871,prsc:2|IN-4852-OUT,STPS-5338-OUT;proporder:7241-8221-1876-5338;pass:END;sub:END;*/

Shader "Shader Forge/FresnelShader" {
    Properties {
        _Color ("Color", Color) = (0,1,0.5897269,1)
        _fresnelexp ("fresnel exp", Float ) = 1
        _opacityfresnelexp ("opacity fresnel exp", Float ) = 1
        _posterizesteps ("posterize steps", Float ) = 4
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
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _fresnelexp)
                UNITY_DEFINE_INSTANCED_PROP( float, _opacityfresnelexp)
                UNITY_DEFINE_INSTANCED_PROP( float, _posterizesteps)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 posWorld : TEXCOORD0;
                float3 normalDir : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.normalDir = UnityObjectToWorldNormal(v.normal);
                o.posWorld = mul(unity_ObjectToWorld, v.vertex);
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                i.normalDir = normalize(i.normalDir);
                float3 viewDirection = normalize(_WorldSpaceCameraPos.xyz - i.posWorld.xyz);
                float3 normalDirection = i.normalDir;
////// Lighting:
////// Emissive:
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float _fresnelexp_var = UNITY_ACCESS_INSTANCED_PROP( Props, _fresnelexp );
                float _posterizesteps_var = UNITY_ACCESS_INSTANCED_PROP( Props, _posterizesteps );
                float3 emissive = (_Color_var.rgb*floor(pow(1.0-max(0,dot(normalDirection, viewDirection)),_fresnelexp_var) * _posterizesteps_var) / (_posterizesteps_var - 1));
                float3 finalColor = emissive;
                float _opacityfresnelexp_var = UNITY_ACCESS_INSTANCED_PROP( Props, _opacityfresnelexp );
                return fixed4(finalColor,(_Color_var.a*floor(pow(1.0-max(0,dot(normalDirection, viewDirection)),_opacityfresnelexp_var) * _posterizesteps_var) / (_posterizesteps_var - 1)));
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

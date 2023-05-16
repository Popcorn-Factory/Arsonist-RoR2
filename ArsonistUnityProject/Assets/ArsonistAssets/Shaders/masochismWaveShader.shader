// Shader created with Shader Forge v1.40 
// Shader Forge (c) Freya Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.40;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,cpap:True,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:0,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:1,bsrc:4,bdst:1,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32888,y:32567,varname:node_3138,prsc:2|emission-1121-OUT,custl-7241-RGB,alpha-6350-OUT,olwid-1277-OUT,olcol-2019-RGB;n:type:ShaderForge.SFN_Color,id:7241,x:32462,y:32778,ptovrint:False,ptlb:Color,ptin:_Color,varname:node_7241,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5641353,c3:0,c4:1;n:type:ShaderForge.SFN_Tex2dAsset,id:8115,x:31925,y:32674,ptovrint:False,ptlb:texture,ptin:_texture,varname:node_8115,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:583a0a8dbd5df41c2b97e12a18e40330,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Tex2d,id:17,x:32174,y:32539,varname:node_17,prsc:2,tex:583a0a8dbd5df41c2b97e12a18e40330,ntxv:0,isnm:False|TEX-8115-TEX;n:type:ShaderForge.SFN_Slider,id:1277,x:32102,y:33051,ptovrint:False,ptlb:outline_width,ptin:_outline_width,varname:node_1277,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:0.05,max:1;n:type:ShaderForge.SFN_Dot,id:1121,x:32745,y:32629,varname:node_1121,prsc:2,dt:4|A-7364-OUT,B-7241-RGB;n:type:ShaderForge.SFN_Slider,id:6350,x:32171,y:32961,ptovrint:False,ptlb:exponent_copy,ptin:_exponent_copy,varname:_exponent_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,min:0,cur:100,max:100;n:type:ShaderForge.SFN_Sin,id:7364,x:32540,y:32600,varname:node_7364,prsc:2|IN-4842-OUT;n:type:ShaderForge.SFN_Time,id:3704,x:32203,y:32669,varname:node_3704,prsc:2;n:type:ShaderForge.SFN_Dot,id:4842,x:32378,y:32590,varname:node_4842,prsc:2,dt:2|A-17-RGB,B-3704-TSL;n:type:ShaderForge.SFN_Time,id:5266,x:31410,y:32539,varname:node_5266,prsc:2;n:type:ShaderForge.SFN_Time,id:9072,x:31410,y:32698,varname:node_9072,prsc:2;n:type:ShaderForge.SFN_Sin,id:7776,x:31626,y:32561,varname:node_7776,prsc:2|IN-5266-T;n:type:ShaderForge.SFN_Sin,id:1573,x:31626,y:32726,varname:node_1573,prsc:2|IN-9072-T;n:type:ShaderForge.SFN_Color,id:2019,x:32453,y:33097,ptovrint:False,ptlb:Color_copy,ptin:_Color_copy,varname:_Color_copy,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:1,c2:0.5641353,c3:0,c4:1;proporder:7241-8115-1277-6350-2019;pass:END;sub:END;*/

Shader "Shader Forge/masochismWaveShader" {
    Properties {
        _Color ("Color", Color) = (1,0.5641353,0,1)
        _texture ("texture", 2D) = "black" {}
        _outline_width ("outline_width", Range(0, 1)) = 0.05
        _exponent_copy ("exponent_copy", Range(0, 100)) = 100
        _Color_copy ("Color_copy", Color) = (1,0.5641353,0,1)
        [HideInInspector]_Cutoff ("Alpha cutoff", Range(0,1)) = 0.5
    }
    SubShader {
        Tags {
            "IgnoreProjector"="True"
            "Queue"="Transparent"
            "RenderType"="Transparent"
        }
        Pass {
            Name "Outline"
            Tags {
            }
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float, _outline_width)
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color_copy)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                float _outline_width_var = UNITY_ACCESS_INSTANCED_PROP( Props, _outline_width );
                o.pos = UnityObjectToClipPos( float4(v.vertex.xyz + v.normal*_outline_width_var,1) );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
                float4 _Color_copy_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color_copy );
                return fixed4(_Color_copy_var.rgb,0);
            }
            ENDCG
        }
        Pass {
            Name "FORWARD"
            Tags {
                "LightMode"="ForwardBase"
            }
            Blend DstColor Zero
            Cull Front
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma target 3.0
            uniform sampler2D _texture; uniform float4 _texture_ST;
            UNITY_INSTANCING_BUFFER_START( Props )
                UNITY_DEFINE_INSTANCED_PROP( float4, _Color)
                UNITY_DEFINE_INSTANCED_PROP( float, _exponent_copy)
            UNITY_INSTANCING_BUFFER_END( Props )
            struct VertexInput {
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                UNITY_VERTEX_INPUT_INSTANCE_ID
                float2 uv0 : TEXCOORD0;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                UNITY_SETUP_INSTANCE_ID( v );
                UNITY_TRANSFER_INSTANCE_ID( v, o );
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                UNITY_SETUP_INSTANCE_ID( i );
////// Lighting:
////// Emissive:
                float4 node_17 = tex2D(_texture,TRANSFORM_TEX(i.uv0, _texture));
                float4 node_3704 = _Time;
                float4 _Color_var = UNITY_ACCESS_INSTANCED_PROP( Props, _Color );
                float node_1121 = (0.5*dot(sin(min(0,dot(node_17.rgb,node_3704.r))),_Color_var.rgb)+0.5);
                float3 emissive = float3(node_1121,node_1121,node_1121);
                float3 finalColor = emissive + _Color_var.rgb;
                float _exponent_copy_var = UNITY_ACCESS_INSTANCED_PROP( Props, _exponent_copy );
                return fixed4(finalColor,_exponent_copy_var);
            }
            ENDCG
        }
        Pass {
            Name "ShadowCaster"
            Tags {
                "LightMode"="ShadowCaster"
            }
            Offset 1, 1
            Cull Front
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"
            #include "Lighting.cginc"
            #pragma fragmentoption ARB_precision_hint_fastest
            #pragma multi_compile_shadowcaster
            #pragma target 3.0
            struct VertexInput {
                float4 vertex : POSITION;
            };
            struct VertexOutput {
                V2F_SHADOW_CASTER;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.pos = UnityObjectToClipPos( v.vertex );
                TRANSFER_SHADOW_CASTER(o)
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                SHADOW_CASTER_FRAGMENT(i)
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

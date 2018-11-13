// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:1,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:0,bdst:0,dpts:2,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:True,aust:True,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0,fgcg:0,fgcb:0,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:True,fnfb:True,fsmp:False;n:type:ShaderForge.SFN_Final,id:4795,x:32716,y:32678,varname:node_4795,prsc:2|emission-2660-RGB,alpha-2688-OUT;n:type:ShaderForge.SFN_Tex2d,id:6074,x:31994,y:32477,ptovrint:False,ptlb:MainTex,ptin:_MainTex,varname:_MainTex,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:2393,x:32254,y:32669,varname:node_2393,prsc:2|A-6074-RGB,B-2053-RGB,C-797-RGB,D-9248-OUT;n:type:ShaderForge.SFN_VertexColor,id:2053,x:31994,y:32648,varname:node_2053,prsc:2;n:type:ShaderForge.SFN_Color,id:797,x:31994,y:32806,ptovrint:True,ptlb:Color,ptin:_TintColor,varname:_TintColor,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,c1:0.5,c2:0.5,c3:0.5,c4:1;n:type:ShaderForge.SFN_Vector1,id:9248,x:31994,y:32957,varname:node_9248,prsc:2,v1:2;n:type:ShaderForge.SFN_Multiply,id:798,x:32254,y:32799,varname:node_798,prsc:2|A-6074-A,B-2053-A,C-797-A;n:type:ShaderForge.SFN_ScreenPos,id:9954,x:31716,y:33311,varname:node_9954,prsc:2,sctp:2;n:type:ShaderForge.SFN_SceneColor,id:2660,x:32343,y:33153,varname:node_2660,prsc:2|UVIN-5642-OUT;n:type:ShaderForge.SFN_Tex2d,id:5997,x:31452,y:33063,ptovrint:False,ptlb:node_5997,ptin:_node_5997,varname:node_5997,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:c2d52e6d59aad8f40911197495835f61,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Add,id:2162,x:31994,y:33097,varname:node_2162,prsc:2|A-1991-OUT,B-9954-U;n:type:ShaderForge.SFN_Append,id:5642,x:32128,y:33153,varname:node_5642,prsc:2|A-2162-OUT,B-9170-OUT;n:type:ShaderForge.SFN_Add,id:9170,x:31994,y:33235,varname:node_9170,prsc:2|A-1755-OUT,B-9954-V;n:type:ShaderForge.SFN_Tex2d,id:212,x:31452,y:32874,ptovrint:False,ptlb:node_212,ptin:_node_212,varname:node_212,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:0000000000000000f000000000000000,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:1991,x:31689,y:32929,varname:node_1991,prsc:2|A-212-R,B-5997-R,C-1076-OUT;n:type:ShaderForge.SFN_Multiply,id:1755,x:31689,y:33063,varname:node_1755,prsc:2|A-212-R,B-5997-G,C-1076-OUT;n:type:ShaderForge.SFN_ValueProperty,id:1076,x:31452,y:32771,ptovrint:False,ptlb:node_1076,ptin:_node_1076,varname:node_1076,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_Vector1,id:2688,x:32283,y:33008,varname:node_2688,prsc:2,v1:1;proporder:6074-797-5997-212-1076;pass:END;sub:END;*/

Shader "Shader Forge/Distortion" {
    Properties {
        _MainTex ("MainTex", 2D) = "white" {}
        _TintColor ("Color", Color) = (0.5,0.5,0.5,1)
        _node_5997 ("node_5997", 2D) = "white" {}
        _node_212 ("node_212", 2D) = "white" {}
        _node_1076 ("node_1076", Float ) = 0.1
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
            Blend One One
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma multi_compile_fog
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _node_5997; uniform float4 _node_5997_ST;
            uniform sampler2D _node_212; uniform float4 _node_212_ST;
            uniform float _node_1076;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 projPos : TEXCOORD1;
                UNITY_FOG_COORDS(2)
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.pos = UnityObjectToClipPos( v.vertex );
                UNITY_TRANSFER_FOG(o,o.pos);
                o.projPos = ComputeScreenPos (o.pos);
                COMPUTE_EYEDEPTH(o.projPos.z);
                return o;
            }
            float4 frag(VertexOutput i) : COLOR {
                float2 sceneUVs = (i.projPos.xy / i.projPos.w);
                float4 sceneColor = tex2D(_GrabTexture, sceneUVs);
////// Lighting:
////// Emissive:
                float4 _node_212_var = tex2D(_node_212,TRANSFORM_TEX(i.uv0, _node_212));
                float4 _node_5997_var = tex2D(_node_5997,TRANSFORM_TEX(i.uv0, _node_5997));
                float3 emissive = tex2D( _GrabTexture, float2(((_node_212_var.r*_node_5997_var.r*_node_1076)+sceneUVs.r),((_node_212_var.r*_node_5997_var.g*_node_1076)+sceneUVs.g))).rgb;
                float3 finalColor = emissive;
                fixed4 finalRGBA = fixed4(finalColor,1.0);
                UNITY_APPLY_FOG(i.fogCoord, finalRGBA);
                return finalRGBA;
            }
            ENDCG
        }
    }
    CustomEditor "ShaderForgeMaterialInspector"
}

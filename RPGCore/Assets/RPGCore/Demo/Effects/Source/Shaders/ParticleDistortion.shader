// Shader created with Shader Forge v1.38 
// Shader Forge (c) Neat Corporation / Joachim Holmer - http://www.acegikmo.com/shaderforge/
// Note: Manually altering this data may prevent you from opening it in Shader Forge
/*SF_DATA;ver:1.38;sub:START;pass:START;ps:flbk:,iptp:0,cusa:False,bamd:0,cgin:,lico:0,lgpr:1,limd:0,spmd:1,trmd:0,grmd:0,uamb:True,mssp:True,bkdf:False,hqlp:False,rprd:False,enco:False,rmgx:True,imps:True,rpth:0,vtps:0,hqsc:True,nrmq:1,nrsp:0,vomd:0,spxs:False,tesm:0,olmd:1,culm:0,bsrc:3,bdst:7,dpts:6,wrdp:False,dith:0,atcv:False,rfrpo:True,rfrpn:Refraction,coma:15,ufog:False,aust:False,igpj:True,qofs:0,qpre:3,rntp:2,fgom:False,fgoc:False,fgod:False,fgor:False,fgmd:0,fgcr:0.5,fgcg:0.5,fgcb:0.5,fgca:1,fgde:0.01,fgrn:0,fgrf:300,stcl:False,atwp:False,stva:128,stmr:255,stmw:255,stcp:6,stps:0,stfa:0,stfz:0,ofsf:0,ofsu:0,f2p0:False,fnsp:False,fnfb:False,fsmp:False;n:type:ShaderForge.SFN_Final,id:3138,x:32719,y:32712,varname:node_3138,prsc:2|emission-3621-RGB,alpha-2686-A;n:type:ShaderForge.SFN_Tex2d,id:8641,x:31512,y:32832,ptovrint:False,ptlb:node_5997,ptin:_node_5997,varname:node_5997,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a01d3117e5fb3bb4e97608231b55e169,ntxv:2,isnm:False;n:type:ShaderForge.SFN_Add,id:1366,x:32073,y:32656,varname:node_1366,prsc:2|A-5739-OUT,B-8098-R;n:type:ShaderForge.SFN_Append,id:3301,x:32237,y:32711,varname:node_3301,prsc:2|A-1366-OUT,B-9211-OUT;n:type:ShaderForge.SFN_Add,id:9211,x:32073,y:32794,varname:node_9211,prsc:2|A-1295-OUT,B-8098-G;n:type:ShaderForge.SFN_Tex2d,id:9518,x:31512,y:32643,ptovrint:False,ptlb:node_212,ptin:_node_212,varname:node_212,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,tex:a01d3117e5fb3bb4e97608231b55e169,ntxv:0,isnm:False;n:type:ShaderForge.SFN_Multiply,id:5739,x:31768,y:32488,varname:node_5739,prsc:2|A-9518-R,B-8641-R,C-9368-OUT,D-2686-A;n:type:ShaderForge.SFN_Multiply,id:1295,x:31768,y:32622,varname:node_1295,prsc:2|A-9518-R,B-8641-G,C-9368-OUT,D-2686-A;n:type:ShaderForge.SFN_ValueProperty,id:9368,x:31512,y:32540,ptovrint:False,ptlb:node_1076,ptin:_node_1076,varname:node_1076,prsc:2,glob:False,taghide:False,taghdr:False,tagprd:False,tagnsco:False,tagnrm:False,v1:0.1;n:type:ShaderForge.SFN_VertexColor,id:2686,x:31512,y:32381,varname:node_2686,prsc:2;n:type:ShaderForge.SFN_ScreenPos,id:4415,x:31331,y:32997,varname:node_4415,prsc:2,sctp:0;n:type:ShaderForge.SFN_SceneColor,id:3621,x:32462,y:32833,varname:node_3621,prsc:2|UVIN-3301-OUT;n:type:ShaderForge.SFN_Vector1,id:5709,x:31318,y:33236,varname:node_5709,prsc:2,v1:1;n:type:ShaderForge.SFN_Add,id:8145,x:31548,y:33103,varname:node_8145,prsc:2|A-4415-UVOUT,B-5709-OUT;n:type:ShaderForge.SFN_Multiply,id:4953,x:31770,y:33203,varname:node_4953,prsc:2|A-8145-OUT,B-1907-OUT;n:type:ShaderForge.SFN_Vector1,id:1907,x:31588,y:33331,varname:node_1907,prsc:2,v1:0.5;n:type:ShaderForge.SFN_ComponentMask,id:8098,x:32019,y:33114,varname:node_8098,prsc:2,cc1:0,cc2:1,cc3:-1,cc4:-1|IN-4953-OUT;proporder:8641-9518-9368;pass:END;sub:END;*/

Shader "Shader Forge/ParticleDistortion" {
    Properties {
        _node_5997 ("node_5997", 2D) = "black" {}
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
            Blend SrcAlpha OneMinusSrcAlpha
            ZTest Always
            ZWrite Off
            
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #define UNITY_PASS_FORWARDBASE
            #include "UnityCG.cginc"
            #pragma multi_compile_fwdbase
            #pragma only_renderers d3d9 d3d11 glcore gles 
            #pragma target 3.0
            uniform sampler2D _GrabTexture;
            uniform sampler2D _node_5997; uniform float4 _node_5997_ST;
            uniform sampler2D _node_212; uniform float4 _node_212_ST;
            uniform float _node_1076;
            struct VertexInput {
                float4 vertex : POSITION;
                float2 texcoord0 : TEXCOORD0;
                float4 vertexColor : COLOR;
            };
            struct VertexOutput {
                float4 pos : SV_POSITION;
                float2 uv0 : TEXCOORD0;
                float4 vertexColor : COLOR;
                float4 projPos : TEXCOORD1;
            };
            VertexOutput vert (VertexInput v) {
                VertexOutput o = (VertexOutput)0;
                o.uv0 = v.texcoord0;
                o.vertexColor = v.vertexColor;
                o.pos = UnityObjectToClipPos( v.vertex );
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
                float2 node_8098 = (((sceneUVs * 2 - 1).rg+1.0)*0.5).rg;
                float3 emissive = tex2D( _GrabTexture, float2(((_node_212_var.r*_node_5997_var.r*_node_1076*i.vertexColor.a)+node_8098.r),((_node_212_var.r*_node_5997_var.g*_node_1076*i.vertexColor.a)+node_8098.g))).rgb;
                float3 finalColor = emissive;
                return fixed4(finalColor,i.vertexColor.a);
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
    CustomEditor "ShaderForgeMaterialInspector"
}

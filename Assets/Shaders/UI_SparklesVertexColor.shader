Shader "Custom/UI/Additive/Sparkles Vertex Color"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        [HideInInspector] _StencilComp ("Stencil Comparison", Float) = 8
        [HideInInspector] _Stencil ("Stencil ID", Float) = 0
        [HideInInspector] _StencilOp ("Stencil Operation", Float) = 0
        [HideInInspector] _StencilWriteMask ("Stencil Write Mask", Float) = 255
        [HideInInspector] _StencilReadMask ("Stencil Read Mask", Float) = 255
        [HideInInspector] _ColorMask ("Color Mask", Float) = 15
        [HideInInspector] [Toggle(UNITY_UI_ALPHACLIP)] _UseUIAlphaClip ("Use Alpha Clip", Float) = 0

        [Header(SPARKLES)]
        [NoScaleOffset] _spkTex("Texture", 2D) = "white" {}
        _spkBrigness("Brightness", Float) = 1
        _spkDist("Distortion", Float) = 0
        _spkTilling1("layer1 Tlling", Float) = 1
        _spkTilling2("layer2 Tlling", Float) = 1
        _spkPanner("Panner", Vector) = (0, 0, 0, 0)
        [Header(NOISE)]
        [NoScaleOffset] _noiseTex("Texture", 2D) = "white" {}
        _noiseTilling("Tilling", Float) = 1
        _noisePanner("Panner", Vector) = (0, 0, 0, 0)
    }

    SubShader
    {
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }

        Cull Back
        Lighting Off
        ZWrite Off
        ZTest [unity_GUIZTestMode]
        Blend One One
        ColorMask [_ColorMask]

        Pass
        {
            Name "Default"
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma target 2.0

            #include "UnityCG.cginc"
            #include "UnityUI.cginc"

            #pragma multi_compile __ UNITY_UI_CLIP_RECT
            #pragma multi_compile __ UNITY_UI_ALPHACLIP

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                float2 uv       : TEXCOORD0;
                float4 worldPos : TEXCOORD1;
                float4 panner1  : TEXCOORD2;
                float4 panner2  : TEXCOORD3;
                UNITY_VERTEX_INPUT_INSTANCE_ID
            };

            float _noiseTilling;
            float4 _noisePanner;
            float _spkTilling1;
            float _spkTilling2;
            float4 _spkPanner;

            v2f vert(appdata_t v)
            {
                v2f o;
                UNITY_SETUP_INSTANCE_ID(v);
                UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);
                o.worldPos = v.vertex;
                o.vertex = UnityObjectToClipPos(o.worldPos);
                o.uv = v.uv;
                o.color = v.color;

                o.panner1 = v.uv.xyxy * _noiseTilling + _Time.y * _noisePanner;
                float4 spkTime = _Time.y * _spkPanner;
                o.panner2 = float4(v.uv * _spkTilling1 + spkTime.xy, v.uv * _spkTilling2 + spkTime.zw);
                return o;
            }

            sampler2D _MainTex;
            sampler2D _spkTex;
            sampler2D _noiseTex;
            fixed4 _TextureSampleAdd;
            float4 _ClipRect;
            float _spkDist;
            float _spkBrigness;

            fixed4 frag(v2f i) : SV_Target
            {
                float noisePanner1 = tex2D(_noiseTex, i.panner1.xy).r;
                float noisePanner2 = tex2D(_noiseTex, i.panner1.zw).g;
                float noiseMask1 = saturate(noisePanner1 + noisePanner2);
                float noiseMask2 = saturate(noisePanner1 - noisePanner2);
                float2 distUv = float2(noisePanner1, noisePanner2) * _spkDist;
                float4 spriteTex = (tex2D(_MainTex, distUv + i.uv) + _TextureSampleAdd);
                float spkPanner1 = tex2D(_spkTex, i.panner2.xy + distUv).r;
                float spkPanner2 = tex2D(_spkTex, i.panner2.zw + distUv).r;
                float spkMask = saturate(((spkPanner1 - noiseMask1) + (spkPanner2 - noiseMask2)) * spriteTex.r);
                fixed4 color = spkMask * i.color * _spkBrigness * i.color.a;

                #ifdef UNITY_UI_CLIP_RECT
                    color.a *= UnityGet2DClipping(i.worldPos.xy, _ClipRect);
                #endif

                #ifdef UNITY_UI_ALPHACLIP
                    clip (color.a - 0.001);
                #endif

                return color;
            }
            ENDCG
        }
    }
}

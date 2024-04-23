Shader "Ozeg/UI/Vignette"
{
    Properties
    {
		[HideInInspector]
		_MainTex("Base (RGB)", 2D) = "white" {}
		_Color("color", Color) = (1,1,1,1)
		_Params ("vignette parameters", Vector) = (0,0,1,0)
    }
    SubShader
    {
        Blend SrcAlpha OneMinusSrcAlpha
        Tags
        {
            "Queue"="Transparent"
            "IgnoreProjector"="True"
            "RenderType"="Transparent"
            "PreviewType"="Plane"
        }
        Stencil
        {
            Ref [_Stencil]
            Comp [_StencilComp]
            Pass [_StencilOp]
            ReadMask [_StencilReadMask]
            WriteMask [_StencilWriteMask]
        }
        Fog 
        { 
            Mode Off 
        }
        LOD 100
        ZWrite Off

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #pragma target 3.5
            #include "UnityCG.cginc"
            #include "UnityUI.cginc"


            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
                fixed4 col : COLOR;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
                fixed4 col : COLOR;
            };

			fixed4 _Color;
			v2f vert (appdata v)
            {
                v2f o;
                o.col = v.col*_Color;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }
            float4 _Params;
            fixed4 frag (v2f i) : SV_Target
            {

                fixed4 col = i.col;
                float2 ratio = abs(float2(ddx(i.uv.x),ddy(i.uv.y)));
                float aspect = max(0.5,min(ratio.x,ratio.y)/max(ratio.x,ratio.y));
                float2 aRatio = float2(aspect,1);
                i.uv = abs(i.uv-.5)*2.;
                float4 box = float4(i.uv.xy,1.-i.uv.x,1.-i.uv.y);
                box.xy+=(_Params.x+_Params.y*.666667)*aRatio-.5;
                box.zw-=(_Params.x+_Params.y*.666667)*aRatio-.5;

                if(ratio.x>ratio.y)     box.xz *= aspect;
                else                    box.yw *= aspect;

                float2 d = box.xy-box.zw+.5;
                float distance = length( max( d, 0.0 )) + min( max( d.x, d.y ), 0.0 );
                distance = ( distance - .5 - _Params.y ) * _Params.z + .5;
                col.a *= _Params.w+smoothstep(0.,1.,distance);
                return col;
            }
            ENDCG
        }
    }
}

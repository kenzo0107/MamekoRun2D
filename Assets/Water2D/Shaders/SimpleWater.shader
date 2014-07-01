// Unlit
// Supports vertex color
// No lightmap
// Transparent



Shader "Water2D/Simple Water" {
    Properties

    {
        _Color ("Color", Color) = (1,1,1,1)
        _MainTex ("Base (RGB) Trans (A)", 2D ) = "white" {}
        _WiggleTex ("Base (RGB)", 2D) = "white" {}
		_WiggleStrength ("Wiggle Strength", Range (0.01, 0.2)) = 1
    }
    SubShader

    {
        Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha 
		
        Pass

        {

            CGPROGRAM
                #include "UnityCG.cginc"
                #pragma vertex vert
                #pragma fragment frag

                struct v2f

                {
                    fixed4 color : COLOR;
                    fixed4 pos : SV_POSITION;
                    fixed2 pack0 : TEXCOORD0;
                    //fixed2 tc2	 : TEXCOORD1;
                };
                sampler2D _MainTex;
                sampler2D _WiggleTex;
                fixed4 _MainTex_ST;
                fixed4 _Color;
                fixed _WiggleStrength;

                v2f vert(appdata_full v)

                {
                    v2f o;
                    o.pos = mul(UNITY_MATRIX_MVP, v.vertex);
                    o.pack0.xy = TRANSFORM_TEX(v.texcoord, _MainTex);
                    
                    o.color = v.color;
                    return o;

                }
                
                fixed4 frag(v2f i) : COLOR
                {

                    fixed2 tc2 = i.pack0;
                    tc2.x -= _SinTime;
                    tc2.y += _CosTime;
                    
                    fixed4 wiggle = tex2D(_WiggleTex, tc2);
                    
                    fixed2 newUVtext = i.pack0;
                    newUVtext.x -= wiggle.r * _WiggleStrength;
                    newUVtext.y += wiggle.b * _WiggleStrength*1.5f;
                    //i.pack0.x -= wiggle.r * _WiggleStrength;
                    //i.pack0.y += wiggle.b * _WiggleStrength*1.5f;
                    //i.pack0.x = wiggle.r * _WiggleStrength;
                    
                    fixed4 c = tex2D(_MainTex,newUVtext) * i.color *_Color;
                    return c;

                }

            ENDCG

        }

    }

}
/*
	float2 tc2 = IN.uv_WiggleTex;
	tc2.x -= _SinTime;
	tc2.y += _CosTime;
	float4 wiggle = tex2D(_WiggleTex, tc2);
	
	IN.uv_MainTex.x -= wiggle.r * _WiggleStrength;
	IN.uv_MainTex.y += wiggle.b * _WiggleStrength*1.5f;
	*/
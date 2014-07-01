// Per pixel bumped refraction.
// Uses a normal map to distort the image behind, and
// an additional texture to tint the color.

Shader "Water2D/Water PC" {
Properties {
	_Color ("Color", Color) = (1,1,1,1)
	_BumpAmt  ("Distortion", range (0,50)) = 10 
	_MainTex ("Tint Color (RGB)", 2D) = "white" {}
	_BumpMap ("Normalmap", 2D) = "bump" {}
}

	CGINCLUDE
	#pragma fragmentoption ARB_precision_hint_fastest
	#pragma fragmentoption ARB_fog_exp2
	#include "UnityCG.cginc"
	
	sampler2D _WaterGrabTexture : register(s0);
	float4 _GrabTexture_TexelSize;
	sampler2D _BumpMap : register(s1);
	sampler2D _MainTex;
	fixed4 _Color;
	//sampler2D _MainTex : register(s2);
	
	
	struct v2f {
		float4 vertex 	: POSITION;
		half2 uvmain 	: TEXCOORD0;
		half2 uvbump 	: TEXCOORD1;
		half4 uvgrab 	: TEXCOORD2;
		fixed4 color	: COLOR;
		
	};
	
	uniform fixed _BumpAmt;
	
	
	half4 frag( v2f i ) : COLOR
	{
		// calculate perturbed coordinates
		fixed2 bump = UnpackNormal(tex2D( _BumpMap, i.uvbump )).rg; // we could optimize this by just reading the x & y without reconstructing the Z
		fixed2 offset = bump * _BumpAmt * _GrabTexture_TexelSize.xy;
		
		i.uvgrab.xy = offset * i.uvgrab.z + i.uvgrab.xy;
		
		fixed4 distort = tex2Dproj( _WaterGrabTexture, i.uvgrab.xyw ) ;
		fixed4 tint = tex2D( _MainTex, i.uvmain ) * _Color;
		tint.rgb *= i.color.rgb;		
		fixed4 col;
		col = (distort + tint)*0.5;
		col.a = 1;
		return col;
		
	}
	ENDCG

Category {

	// We must be transparent, so other objects are drawn before this one.
	Tags { "Queue"="Transparent+100" "RenderType"="Opaque" }
	Cull off
	Fog {Mode Off}
	Blend SrcAlpha OneMinusSrcAlpha
	
	SubShader {

		// This pass grabs the screen behind the object into a texture.
		// We can access the result in the next pass as _GrabTexture
		GrabPass { 							
			"_WaterGrabTexture"
			Name "BASE"
			Tags { "LightMode" = "Always" }
 		}
 		
 		// Main pass: Take the texture grabbed above and use the bumpmap to perturb it
 		// on to the screen
		Pass {
			Name "BASE"
			Tags { "LightMode" = "Always" }
			
		CGPROGRAM
		#pragma vertex vert
		#pragma fragment frag
		#pragma fragmentoption ARB_precision_hint_fastest
		#pragma fragmentoption ARB_fog_exp2
		
		struct appdata_t {
			float4 	vertex 		: POSITION;
			float2 	texcoord	: TEXCOORD0;
			fixed4	color 		: COLOR;
		};
		
		float4 _MainTex_ST;
		float4 _BumpMap_ST;
		
		v2f vert (appdata_t v)
		{
			v2f o;
			o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
			o.uvgrab.xy = (float2(o.vertex.x, o.vertex.y) + o.vertex.w) * 0.5;
			o.uvgrab.zw = o.vertex.zw;
			//o.uvbump = MultiplyUV( UNITY_MATRIX_TEXTURE1, v.texcoord );
			
			o.uvbump = TRANSFORM_TEX(v.texcoord, _MainTex);
			o.uvmain = TRANSFORM_TEX(v.texcoord, _MainTex);
			
			//o.uvmain = MultiplyUV( UNITY_MATRIX_TEXTURE2, v.texcoord );
			o.color = v.color;
			return o;
		}
		ENDCG
		}
	}

}

}

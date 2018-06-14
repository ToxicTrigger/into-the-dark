Shader "Custom/alphatest" {

	Properties {
		_Color ("Color", Color) = (0,0,0,1)
		_Tex("Tex (RGB)", 2D) = "white" {}
	}
	SubShader {
	//Blend srcAlpha oneMinusSrcAlpha, zero one
		Tags { "RenderType"="opaque" }
		CGPROGRAM
		//Blend One OneMinusDstAlpha
		#pragma surface surf Lambert 
		
		fixed4  _Color;
		sampler2D _Tex;

		struct Input {
			float2 uv_MainTex;
		};
		
		void surf (Input IN, inout SurfaceOutput  o) {

			fixed4 c = tex2D(_Tex , IN.uv_MainTex);

			o.Albedo = c;
		}
		ENDCG
	}
	FallBack "Diffuse"
}

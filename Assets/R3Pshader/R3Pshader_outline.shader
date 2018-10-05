Shader "Custom/R3Pshader_outline" {
	Properties {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Rimrange("림굵기", range(1,0)) = 0.8 //
		[HDR]_Color("Albedo color", Color) = (1,1,1,1)
		[HDR]_Rimcolor("Rim color", Color) = (1,1,1,1)
		_Color2("Outline color", Color) = (0,0,0,1)
		_Outlinerange("아웃라인 굵기", range(0,0.01)) = 0.0025 //
	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		CGPROGRAM
		#pragma surface surf r3p fullforwardshadows

		sampler2D _MainTex;

		struct Input {
			float2 uv_MainTex;
		};

		fixed4 _Color,_Rimcolor;
		float _Rimrange, _Outlinerange;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
		}

		float4 Lightingr3p(inout SurfaceOutput s, float3 lightDir, float3 viewDir, float atten) {
		
			float NdotL = dot(s.Normal, lightDir);

			if (NdotL > 0.4) NdotL = 1;
			else NdotL = 0.5;

			float rim = dot(s.Normal, viewDir + float3(0, +0.15, 0));

			rim = pow(1 - rim, 5);

			if (rim > _Rimrange) rim = 0.9;
			else rim = 0;
			float3 rimcol = rim * _Rimcolor*saturate(s.Albedo * 50);

			float4 finalcolor;
			finalcolor.rgb = s.Albedo*NdotL + rimcol;
			finalcolor.a = 1;

			return finalcolor;

		
		}


		ENDCG



		cull front
		
		CGPROGRAM
		#pragma surface surf r4p vertex:vert noambient

		sampler2D _MainTex;

	
		struct Input {
			float2 uv_MainTex;
		};

		float4 _Color2;

		void surf (Input IN, inout SurfaceOutput o) {
			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);

			o.Albedo = c.rgb;
			o.Alpha = 1;
		}

		float _Outlinerange;

		float4 Lightingr4p(inout SurfaceOutput s, float3 lightDir, float3 viewDir, float atten){ 
			return  _Color2;

		}

		void vert(inout appdata_full v){
			
			v.vertex.xyz += v.normal*_Outlinerange;//*(cos(_Time.y*5)*0.5+0.5)*0.008;

		}

		ENDCG
			

	}
	FallBack "Diffuse"
}

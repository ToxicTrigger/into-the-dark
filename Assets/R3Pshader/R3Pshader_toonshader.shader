Shader "R3Pshader/toonshader" {
	Properties {
		_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_RampMaskTex ("영역 마스킹", 2D) = "white" {}
		_RampTex ("램프텍스쳐", 2D) = "black" {}
		_RimTex ("림텍스쳐", 2D) = "white" {}
		_Bumpmap("Normal",2D)= "bump"{}
		_Bumppower("범프세기",float) = 1
		_Rimrange("림굵기", range(1,0)) = 0.8 //
		[HDR]_Color ("Color", Color) = (1,1,1,1)
		_Color2 ("Color", Color) = (0,0,0,1)
		[Toggle] _IfSpec("스펙큘러를 사용할까요?", float) = 0 //스펙큘러 사용시 온(금속, 플라스틱 등 반짝반짝함이 필요한 재질)
		_Outlinerange("아웃라인 굵기", range(0,0.005)) = 0.0025 //



	}
	SubShader {
		Tags { "RenderType"="Opaque" }

		cull back

		CGPROGRAM
		#pragma surface surf r3p  fullforwardshadows 
		#pragma target 3.0

		sampler2D _MainTex;
		sampler2D _RampTex;
		sampler2D _RampMaskTex;
		sampler2D _RimTex;
		sampler2D _Bumpmap;

		struct Input {
			float2 uv_MainTex;
			float2 uv_RampMaskTex;
			float2 uv_Bumpmap;
			float3 lightDir;
			float3 viewDir;
			float atten;
		};

		struct SurfaceOutputCustom
		{
			float3 Albedo;
			float3 Normal;
			float3 Emission;
			float Alpha;

			float3 Diff;
			float4 Masktexx;


		};

		float _Rimrange, _Bumppower, _IfSpec;
		float4 _Color;

		void surf (Input IN, inout SurfaceOutputCustom o) {

			fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
			fixed4 masktexx = tex2D (_RampMaskTex, IN.uv_RampMaskTex);

			o.Masktexx = masktexx;

			o.Diff = c.rgb;
			o.Alpha = 1;
			o.Albedo = 0;
			o.Normal = UnpackNormal(tex2D(_Bumpmap, IN.uv_Bumpmap))*_Bumppower;


		}

		float4 Lightingr3p(inout SurfaceOutputCustom s, float3 lightDir, float3 viewDir, float atten){
			
			
			float3 H = normalize(lightDir+viewDir);
			float NdotH = dot(s.Normal, H);
			float spec;

			if(NdotH>0.9) spec = 1;
			else spec = 0;
			


			float NdotL =  (dot( s.Normal , lightDir) *0.5 + 0.5)* atten;
			//float NdotV = dot(s.Normal, viewDir);
			
			//램프텍스쳐를 써보자~~
			//float4 rimtex = tex2D(_RimTex, float2(NdotL, NdotV));

			float4 ramptexR = tex2D( _RampTex, float2(NdotL, 0.85));//위에서부터 1칸
			float4 ramptexG = tex2D( _RampTex, float2(NdotL, 0.65));//2칸
			float4 ramptexB = tex2D( _RampTex, float2(NdotL, 0.35));//3칸
			float4 ramptexA = tex2D( _RampTex, float2(NdotL, 0.15));//4칸


			s.Albedo = lerp(s.Albedo, ramptexR, s.Masktexx.r);
			s.Albedo = lerp(s.Albedo, ramptexG, s.Masktexx.g);
			s.Albedo = lerp(s.Albedo, ramptexB, s.Masktexx.b);
			s.Albedo = lerp(s.Albedo, ramptexA, s.Masktexx.a);


			s.Albedo = s.Albedo*s.Diff;

			float rim = dot(s.Normal, viewDir+float3(0, +0.15, 0));

			rim = pow(1-rim, 5);

			if (rim > _Rimrange) rim = 0.9;
			else rim = 0;
			float3 rimcol = rim*_Color*saturate(s.Albedo*50);

			float3 speccol;
			speccol = spec*saturate(s.Albedo*3)*3*tex2D( _RampTex, float2(0.25, 0.85)).rgb;

			float4 finalcolor;
			finalcolor.rgb = s.Albedo+rimcol+speccol*_IfSpec;
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

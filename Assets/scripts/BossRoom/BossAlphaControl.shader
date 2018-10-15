Shader "Unlit/BossAlphaControl"
{
	Properties
	{ 
		_DiffuseColor("DiffuseColor", COLOR) = (0,0,1,1) 
		_AmbientColor("AmbientColor", COLOR) = (0,0,1,1)
		_Alpha("Alpha", COLOR) = (0,0,0,1) 
		_MainTex("MainTex", 2D) = "white"
	} 
	SubShader
	{ 
		Pass 
		{ 
		//Blend SrcAlpha OneMinusDstAlpha
			Material 
			{ 
				Diffuse[_DiffuseColor] 
				Ambient[_AmbientColor] 
			}
		Blend SrcAlpha OneMinusSrcAlpha
			Lighting On 
			SetTexture[_MainTex] 
			{ 
				constantColor[_Alpha]
				Combine texture * primary DOUBLE, texture * constant
			}

		} 
	}

}



//
//Properties
//{
//	_MainTex("Texture", 2D) = "white" {}
//}
//SubShader
//{
//	Tags { "RenderType" = "Opaque" }
//	LOD 100
//
//	Pass
//	{
//		CGPROGRAM
//		#pragma surface surf SimpleLambert
//		//#pragma vertex vert
//		//#pragma fragment frag
//		// make fog work
//		//#pragma multi_compile_fog
//
//		//#include "UnityCG.cginc"
//
//		struct appdata
//		{
//			float4 vertex : POSITION;
//			float2 uv : TEXCOORD0;
//		};
//
//		struct v2f
//		{
//			float2 uv : TEXCOORD0;
//			UNITY_FOG_COORDS(1)
//			float4 vertex : SV_POSITION;
//		};
//
//		sampler2D _MainTex;
//		float4 _MainTex_ST;
//
//		v2f vert(appdata v)
//		{
//			v2f o;
//			o.vertex = UnityObjectToClipPos(v.vertex);
//			o.uv = TRANSFORM_TEX(v.uv, _MainTex);
//			UNITY_TRANSFER_FOG(o,o.vertex);
//			return o;
//		}
//
//		fixed4 frag(v2f i) : SV_Target
//		{
//			// sample the texture
//			fixed4 col = tex2D(_MainTex, i.uv);
//		// apply fog
//		UNITY_APPLY_FOG(i.fogCoord, col);
//		return col;
//	}
//	ENDCG
//}
//}
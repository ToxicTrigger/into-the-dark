Shader "Unlit/Fill_amount"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Color("Tint", Color) = (1,1,1,1)
		_Amount("Amount", Range(-1, 0)) = 0
	}
	SubShader
	{
		Tags 
		{ 
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
			"PreviewType" = "Plane"
			"CanUseSpriteAtlas" = "True"
		}
		LOD 100
		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			//#pragma multi_compile_fog
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
				float4 color : COLOR;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				fixed4 color : COLOR;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed4 _Color;
			float4 _MainTex_ST;
			float _Amount;

			fixed4 SampleSpriteTex(float2 uv)
			{
				fixed4 color = tex2D(_MainTex, uv);
#if UNITY_TEXTURE_ALPHASPLIT_ALLOWED
				if (_AlphaSplitEnabled)
					color.a = tex2D(_AlphaTex, uv).r;
#endif //UNITY_TEXTURE_ALPHASPLIT_ALLOWED

				return color;
			}
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				//o.texcoord = v.texcoord;
				o.color = v.color * _Color;
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = SampleSpriteTex(i.uv) * i.color;
				col.rgb *= col.a;
				float amount = _Amount * 360 + 360;
				float angle = atan2(i.uv.r - 0.5, i.uv.g - 0.5);
				angle = degrees(angle);

				if (angle < 0)
				{
					angle = angle + 360;
				}

				
				
				if (angle < amount)
				{
					col.a = 0;
					col.rgb = col.a;
					return col;
				}
				else 
				{

					return col;
				}
				
				
			}
			ENDCG
		}
	}
}

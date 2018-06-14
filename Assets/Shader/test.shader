

Shader "Solid White"{

	Properties{
		_Color("Main Color",COLOR) = (1,0,0,0)
		_MainTex("Main Texture",2D) = "white"{}
	}
		SubShader{

		Tags{ "Queue" = "Transparent" }

			Pass{

			Blend SrcAlpha OneMinusSrcAlpha

					Material{
						Diffuse[_Color]
						Ambient[_Color]
					}
					Lighting On
		}
	}
	
		Fallback "Diffuse"
}
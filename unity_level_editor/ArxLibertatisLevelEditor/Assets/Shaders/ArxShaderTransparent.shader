Shader "Arx/ArxShaderTransparent"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			CGPROGRAM
		//shader variants
		#pragma multi_compile_local __ WATER
		#pragma multi_compile_local __ GLOW
		#pragma multi_compile_local __ LAVA
		#pragma multi_compile_local __ FALL

		#pragma vertex vert
		#pragma fragment frag
		// make fog work
		#pragma multi_compile_fog

		#define TRANSPARENT

		#include "ArxShaderInc.cginc"
			ENDCG
		}
	}
}

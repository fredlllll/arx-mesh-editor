Shader "Arx/ArxShaderDoubleSided"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }

			Cull Off

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

		#include "ArxShaderInc.cginc"
			ENDCG
		}
	}
}

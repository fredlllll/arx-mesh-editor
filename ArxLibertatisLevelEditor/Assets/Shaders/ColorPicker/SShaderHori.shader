Shader "ColorPicker/SShaderHori"
{
	Properties
	{
		_Hue("Hue",Float) = 0
		_Value("Value",Float) = 0
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			// make fog work
			#pragma multi_compile_fog

			#include "UnityCG.cginc"

			float _Hue;
			float _Value;

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			float3 Hue(float H)
			{
				float R = abs(H * 6 - 3) - 1;
				float G = 2 - abs(H * 6 - 2);
				float B = 2 - abs(H * 6 - 4);
				return saturate(float3(R, G, B));
			}

			float3 HSVToRGB(in float3 HSV)
			{
				return float3(((Hue(HSV.x) - 1) * HSV.y + 1) * HSV.z);
			}

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float3 col = lerp(HSVToRGB(float3(_Hue,0,_Value)),HSVToRGB(float3(_Hue,1,_Value)),i.uv.x);
				return fixed4(col,1);
			}
			ENDCG
		}
	}
}

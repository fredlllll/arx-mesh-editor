Shader "Unlit/GridShader"
{
	Properties
	{
		_MainCol("Color", Color) = (0,0,0,1)
	}
		SubShader
	{
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent"}
		ZWrite On
		Blend SrcAlpha OneMinusSrcAlpha
		Cull Off
		LOD 100

		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float4 vertex : SV_POSITION;
				float2 uv: TEXCOORD0;
			};

			float4 _MainCol;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = mul(unity_ObjectToWorld,v.vertex ).xz;
				return o;
			}

			//source https://iquilezles.org/www/articles/filterableprocedurals/filterableprocedurals.htm
			float filteredGrid(in float2 p, in float2 dpdx, in float2 dpdy)
			{
				const float N = 25.0;
				float2 w = max(abs(dpdx), abs(dpdy));
				float2 a = p + 0.5 * w;
				float2 b = p - 0.5 * w;
				float2 i = (floor(a) + min(frac(a) * N, 1.0) -
					floor(b) - min(frac(b) * N, 1.0)) / (N * w);
				return (1.0 - i.x) * (1.0 - i.y);
			}

			fixed4 frag(v2f i) : SV_Target
			{
				float2 uv = i.uv;
				fixed4 col = _MainCol;
			
				float2 ddx_uv = ddx(uv);
				float2 ddy_uv = ddy(uv);

				col.a = 1-filteredGrid(uv, ddx_uv, ddy_uv);
				if (col.a == 0) {
					discard;
				}
				return col;
			}
			ENDCG
		}
	}
}

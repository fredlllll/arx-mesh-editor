Shader "Arx/Gizmo"
{
	Properties
	{
		_Color("Color",Color) =(0.7,0.7,0.7,1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" "Queue" = "Overlay" }
		LOD 100

		Pass
		{
			Tags { "LightMode" = "ForwardBase" }
			ZTest Always
			CGPROGRAM

		#pragma vertex vert
		#pragma fragment frag

		#include "UnityCG.cginc"

		struct appdata
		{
			float4 vertex : POSITION;
			float4 color : COLOR;
		};

		struct v2f
		{
			float4 vertex : SV_POSITION;
			float4 color : COLOR;
		};

		float4 _Color;

		v2f vert(appdata v)
		{
			v2f o;
			o.vertex = UnityObjectToClipPos(v.vertex);
			o.color = v.color;
			return o;
		}

		fixed4 frag(v2f i) : SV_Target
		{
			fixed4 col = i.color;//vertex color
			col *= _Color;
			return col;
		}
			ENDCG
		}
	}
}

#include "UnityCG.cginc"

struct appdata
{
	float4 vertex : POSITION;
	float3 normal : NORMAL;
	float4 uv : TEXCOORD0;
	float4 color : COLOR;
};

struct v2f
{
	float4 vertex : SV_POSITION;
	float3 normal: NORMAL;
	float2 uv : TEXCOORD0;
	UNITY_FOG_COORDS(1)
	float4 color:COLOR;
};

sampler2D _MainTex;
float4 _MainTex_ST;

float2 getWaterFxUvOffset(const float3 pos) {
	float watereffect = ((_Time.y * 1000) % (3.14159 / 0.00025)) * 0.0005;
	return float2(sin(watereffect /*+ pos.x*/), cos(watereffect /*+ pos.z*/)); //taking out pos cause it causes weird issues atm, gotta fix this in the future
}

v2f vert(appdata v)
{
	v2f o;
	o.vertex = UnityObjectToClipPos(v.vertex);
	o.normal = v.normal;
	o.uv = TRANSFORM_TEX(v.uv, _MainTex);
#if defined(WATER) || defined(LAVA)
	o.uv += getWaterFxUvOffset(v.vertex);
#endif
#if defined(WATER) && defined(FALL)
	o.uv.y += _Time.y;
#endif
#if defined(LAVA) && defined(FALL)
	o.uv.y += _Time.y / 12;
#endif

#ifdef GLOW
	o.color = float4(1, 1, 1, 1); //self illumination
	//TODO: if we add proper lighting we have to do this in pixel shader instead of lighting
#else
	o.color = v.color;
#endif
	UNITY_TRANSFER_FOG(o, o.vertex);
	return o;
}

fixed4 frag(v2f i) : SV_Target
{
	half3 worldNormal = UnityObjectToWorldNormal(i.normal);

	fixed4 col = tex2D(_MainTex, i.uv);
	col *= i.color; //vertex color

#ifdef TRANSPARENT
	float collAdd = col.r + col.g + col.b;
	if (collAdd == 0) {
		col.a = 0;//TODO: transparency types ugh
	}
#endif

	// apply fog
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
}
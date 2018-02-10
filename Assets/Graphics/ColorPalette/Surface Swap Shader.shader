
Shader "ColorSwapper/Surface Swap"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Base (RGB) Trans (A)", 2D) = "white" {}
	_Cutoff("Alpha cutoff", Range(0,1)) = 0.5

		_Color1("Color1", Color) = (1.00,0.00,0.00,1.0)
		_Color2("Color2", Color) = (1.00,1.00,0.00,1.0)
		_Color3("Color3", Color) = (0.00,1.00,0.00,1.0)
		_Color4("Color4", Color) = (0.00,1.00,1.00,1.0)
		_Color5("Color5", Color) = (0.00,0.00,1.00,1.0)
		_Color6("Color6", Color) = (0.00,0.70,0.70,1.0)
		_Color7("Color7", Color) = (0.80,0.20,0.00,1.0)
		_Color8("Color8", Color) = (0.80,0.20,0.00,1.0)
	}
		SubShader
	{
		Tags
	{
		"Queue" = "AlphaTest"
		"IgnoreProjector" = "True"
		"RenderType" = "TransparentCutout"
	}
		LOD      100
		Lighting Off
		Cull Off

		CGPROGRAM
	#pragma surface surf Lambert alphatest:_Cutoff

		sampler2D _MainTex;
	fixed4 _Color;

	float4 _Color1;
	float4 _Color2;
	float4 _Color3;
	float4 _Color4;
	float4 _Color5;
	float4 _Color6;
	float4 _Color7;
	float4 _Color8;

	struct Input
	{
		float2 uv_MainTex;
	};

	void surf(Input IN, inout SurfaceOutput o)
	{
		fixed4 col = tex2D(_MainTex, IN.uv_MainTex);

		float4 newCol = _Color1;

		if (col.b > 1 / 8.0) //> 32
			newCol = _Color2;
		if (col.b > 2.0 / 8.0)//> 64
			newCol = _Color3;
		if (col.b > 3.0 / 8.0)//> 96
			newCol = _Color4;
		if (col.b > 4.0 / 8.0)//> 128
			newCol = _Color5;
		if (col.b > 5.0 / 8.0)//> 160
			newCol = _Color6;
		if (col.b > 6.0 / 8.0)//> 192
			newCol = _Color7;
		if (col.b > 7.0 / 8.0)//> 234
			newCol = _Color8;

		newCol.rgb = newCol.rgb*col.r; //The black lines

		col = col * _Color;

		o.Emission = newCol.rgb;
		//o.Emission = col.rgb;
		o.Alpha = col.a;
	}
	ENDCG
	}

	Fallback "Transparent/Cutout/VertexLit"
}
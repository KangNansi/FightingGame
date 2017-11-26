Shader "Custom/ColorSwapper"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
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
		Tags{ "RenderType" = "Transparent" }
		
		Cull Off ZWrite Off ZTest Always
		
		Blend SrcAlpha OneMinusSrcAlpha


		Pass
	{
		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

#include "UnityCG.cginc"

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

	v2f vert(appdata v)
	{
		v2f o;
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.uv = v.uv;
		return o;
	}

	sampler2D _MainTex;
	float4 _Color1;
	float4 _Color2;
	float4 _Color3;
	float4 _Color4;
	float4 _Color5;
	float4 _Color6;
	float4 _Color7;
	float4 _Color8;

	fixed4 frag(v2f i) : SV_Target
	{
		fixed4 col = tex2D(_MainTex, i.uv);
	
		//float4 newCol = tex2D(_Ramp, float2(col.b, 0));
	float4 newCol = _Color1;
	
	if (col.b > 0.5 / 8.0) //> 32
		newCol = _Color2;
	if (col.b > 1.5 / 8.0)//> 64
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
		
	//newCol = col.b;

		newCol.a = col.a; //L'alpha de la texture
		newCol.rgb = newCol.rgb*col.r; //Le trait
		
		return newCol;
	}
		ENDCG
	}
	}
}
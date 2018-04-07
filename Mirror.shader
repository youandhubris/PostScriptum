Shader "Hidden/PostScriptum/Mirror"
{
	HLSLINCLUDE

	#include "PostProcessing/Shaders/StdLib.hlsl"
	#include "../HLSL/AA/AAStep.hlsl"
	#include "../HLSL/Color/ConvertCMYK.hlsl"
	#include "../HLSL/NoiseAshima.hlsl"
	#include "../HLSL/Transform/Rotate.hlsl"

	/*
	REFERENCE
	--------------
	None         0
	Left         1
	Right        2
	Bottom       3
	Top          4
	Bottom-Left  5
	Top-Left     6
	Bottom-Right 7
	Top-Right    8
	*/

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	int side;

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		if (side == 0);
		else if (side == 1 && i.texcoord.x > 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
		else if (side == 2 && i.texcoord.x < 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
		else if (side == 3 && i.texcoord.y > 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		else if (side == 4 && i.texcoord.y < 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		else if (side == 5)
		{
			if (i.texcoord.x > 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
			if (i.texcoord.y > 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		}
		else if (side == 6)
		{
			if (i.texcoord.x > 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
			if (i.texcoord.y < 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		}
		else if (side == 7)
		{
			if (i.texcoord.x < 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
			if (i.texcoord.y > 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		}
		else if (side == 8)
		{
			if (i.texcoord.x < 0.5) i.texcoord.x = 1.0 - i.texcoord.x;
			if (i.texcoord.y < 0.5) i.texcoord.y = 1.0 - i.texcoord.y;
		}

		return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);
	}

		ENDHLSL

		SubShader
	{
		Cull Off ZWrite Off ZTest Always

			Pass
		{
			HLSLPROGRAM

#pragma vertex VertDefault
#pragma fragment Frag

			ENDHLSL
		}
	}
}
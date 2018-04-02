Shader "Hubris/PostProcess/Kaleido"
{
	HLSLINCLUDE

	#include "PostProcessing/Shaders/StdLib.hlsl"
	#include "../HLSL/Variables.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float3 kaleidoSettings; // ratio, sides, angle
	float4 kaleidoPositions; // screenOrigin, screenPosition

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		// Avoid division by 0
		if (kaleidoSettings.y == 0) kaleidoSettings.y = HUBRIS_EPSILON;

		//---> Window correction
		float2 offset = (i.texcoord - kaleidoPositions.zw) * float2(kaleidoSettings.x, 1.0);

		float radius = length(offset);

		float angle = atan2(offset.y, offset.x) + kaleidoSettings.z;

		angle = fmod(angle, HUBRIS_TWO_PI / kaleidoSettings.y);

		float2 uv = radius * float2(cos(angle), sin(angle));
		uv *= float2(1.0, kaleidoSettings.x);
		uv += kaleidoPositions.xy;

		return SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, uv);
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
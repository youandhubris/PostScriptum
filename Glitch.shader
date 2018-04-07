Shader "Hidden/PostScriptum/Glitch"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"
#include "../HLSL/Math/Random.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	
	float _Intensity;
	float _ColorIntensity;

	half4 direction;

	float filterRadius;
	float flip_up, flip_down;
	float displace;
	float scale;

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		// half4 normal = tex2D(_DispTex, i.texcoord.xy * scale);
		half2 normal = float2(Hubris::Random(i.texcoord, 1.0), Hubris::Random(i.texcoord + float2(5.0, 7.0), 1.0));

		i.texcoord.y -= (1 - (i.texcoord.y + flip_up)) * step(i.texcoord.y, flip_up) + (1 - (i.texcoord.y - flip_down)) * step(flip_down, i.texcoord.y);

		i.texcoord.xy += (normal.xy - 0.5) * displace * _Intensity;

		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy);
		half4 redcolor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy + direction.xy * 0.01 * filterRadius * _ColorIntensity);
		half4 greencolor = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord.xy - direction.xy * 0.01 * filterRadius * _ColorIntensity);

		color += half4(redcolor.r, redcolor.b, redcolor.g, 1) *  step(filterRadius, -0.001);
		color *= 1 - 0.5 * step(filterRadius, -0.001);

		color += half4(greencolor.g, greencolor.b, greencolor.r, 1) *  step(0.001, filterRadius);
		color *= 1 - 0.5 * step(0.001, filterRadius);

		return color;
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
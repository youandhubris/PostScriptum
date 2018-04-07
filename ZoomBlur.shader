Shader "Hidden/PostScriptum/ZoomBlur"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

#include "../HLSL/Math/Random.hlsl"
#include "../HLSL/Variables.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float4 zoomBlurSettings; // center (xy), iterations, strength

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		half4 color = 0.0;
		float total = 0.0;
		float2 convergionPoint = i.texcoord - zoomBlurSettings.xy;
		// Randomize the lookup values to hide the fixed number of samples
		float offset = Hubris::Random(i.texcoord, 1.0);

		for (float t = 0.0; t <= zoomBlurSettings.z; t++)
		{
			float percent = (t + offset) / zoomBlurSettings.z;
			float weight = 4.0 * (percent - percent * percent);
			half4 fragment = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord + convergionPoint * percent * zoomBlurSettings.w);
			// Switch to pre-multiplied alpha to correctly blur transparent images
			fragment.rgb *= fragment.a;

			color += fragment * weight;
			total += weight;
		}

		color /= total;
		color.rgb /= color.a + HUBRIS_EPSILON;
		
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
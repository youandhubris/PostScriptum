Shader "Hidden/PostScriptum/HalfTone"
{
	HLSLINCLUDE

	#include "PostProcessing/Shaders/StdLib.hlsl"
	#include "../HLSL/AA/AAStep.hlsl"
	#include "../HLSL/Color/ConvertCMYK.hlsl"
	#include "../HLSL/NoiseAshima.hlsl"
	#include "../HLSL/Transform/Rotate.hlsl"

	// REFERENCE
	// http://the-print-guide.blogspot.pt/2009/05/halftone-screen-angles.html
	// 15C, 45M, 0Y, 75K [Standard 4/C European]

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	float3 halfToneSettings; // ratio, frequency, scale

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		half4 color = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, i.texcoord);	
		float2 uv = (i.texcoord - 0.5) * halfToneSettings.z * float2(halfToneSettings.x, 1.0);

		float fractalNoise = 0.1 * Hubris::noiseSimplex(uv * 200.0);
		fractalNoise += 0.05 * Hubris::noiseSimplex(uv * 400.0);
		fractalNoise += 0.025 * Hubris::noiseSimplex(uv * 800.0);

		float4 baseCMYK = Hubris::RGB2CMYK(color.rgb);

		float2 colorUV = halfToneSettings.y *  Hubris::Rotate(uv, 0.261799);
		colorUV = frac(colorUV) - 0.5;
		float c = Hubris::AAStep(0.0, sqrt(baseCMYK.x) - 2.0 * length(colorUV) + fractalNoise);

		colorUV = halfToneSettings.y *  Hubris::Rotate(uv, 0.785398);
		colorUV = frac(colorUV) - 0.5;
		float m = Hubris::AAStep(0.0, sqrt(baseCMYK.y) - 2.0 * length(colorUV) + fractalNoise);

		colorUV = halfToneSettings.y *  uv;
		colorUV = frac(colorUV) - 0.5;
		float y = Hubris::AAStep(0.0, sqrt(baseCMYK.z) - 2.0 * length(colorUV) + fractalNoise);

		colorUV = halfToneSettings.y *  Hubris::Rotate(uv, 1.309);
		colorUV = frac(colorUV) - 0.5;
		float k = Hubris::AAStep(0.0, sqrt(baseCMYK.w) - 2.0 * length(colorUV) + fractalNoise);

		half3 rgbScreen = 1.0 - half3(c, m, y);
		rgbScreen = lerp(rgbScreen, 0.0, 0.85 * k + 0.3 * fractalNoise);
		

		// Denoise high frequency
		float2 fw = fwidth(uv);
		float blend = smoothstep(0.7, 1.4, 200.0 * max(fw.x, fw.y));
		return half4(lerp(rgbScreen, color.rgb, blend), color.a);
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
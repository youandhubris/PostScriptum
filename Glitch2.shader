Shader "Hidden/PostScriptum/Glitch2"
{
	HLSLINCLUDE

#include "PostProcessing/Shaders/StdLib.hlsl"

#include "../HLSL/Math/Random.hlsl"

	TEXTURE2D_SAMPLER2D(_MainTex, sampler_MainTex);
	
	float amount;
	float size;
	float angle;
	float seed;
	float seed_x;
	float seed_y;
	float distortion_x;
	float distortion_y;
	float col_s;

	half4 Frag(VaryingsDefault i) : SV_Target
	{
		float2 p = i.texcoord;
		float xs = i.texcoord.x * 1280.0 / size;
		float ys = i.texcoord.y * 720.0 / size;
		//based on staffantans glitch shader for unity https://github.com/staffantan/unityglitch
		// half4 normal = texture(dispMap, p*seed*seed);
		half2 normal = float2(Hubris::Random(i.texcoord, 1.0), Hubris::Random(i.texcoord + float2(5.0, 7.0), 1.0));

		if (p.y<distortion_x + col_s && p.y>distortion_x - col_s * seed) {
			if (seed_x>0.) {
				p.y = 1. - (p.y + distortion_y);
			}
			else {
				p.y = distortion_y;
			}
		}

		if (p.x<distortion_y + col_s && p.x>distortion_y - col_s * seed) {
			if (seed_y>0.) {
				p.x = distortion_x;
			}
			else {
				p.x = 1. - (p.x + distortion_x);
			}
		}

		p.x += normal.x*seed_x*(seed / 5.);
		p.y += normal.y*seed_y*(seed / 5.);
		//base from RGB shift shader
		float2 offset = amount * float2(cos(angle), sin(angle));
		half4 cr = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p + offset);
		half4 cga = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p);
		half4 cb = SAMPLE_TEXTURE2D(_MainTex, sampler_MainTex, p - offset);

		half4 outputColor = half4(cr.r, cga.g, cb.b, cga.a);
		//add noise
		half4 snow = 200. * amount * Hubris::Random(float2(xs * seed,ys * seed*50.0), 1.0) * 0.2;
		outputColor = outputColor + snow;

		return outputColor;
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
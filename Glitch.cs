using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(GlitchRenderer), PostProcessEvent.AfterStack, "Hubris/Glitch")]
public sealed class Glitch : PostProcessEffectSettings
{

    public float glitchup, glitchdown, flicker,
            glitchupTime = 0.05f, glitchdownTime = 0.05f, flickerTime = 0.5f;

    [Range(0, 1)]
    public FloatParameter intensity = new FloatParameter();

    [Range(0, 1)]
    public FloatParameter flipIntensity = new FloatParameter();

    [Range(0, 1)]
    public FloatParameter colorIntensity = new FloatParameter();


    public readonly int zoomBlurSettingsID = Shader.PropertyToID("zoomBlurSettings");


    //[Range(-Arithmos.PI, Arithmos.PI)]
    //public FloatParameter angle = new FloatParameter { value = -Arithmos.PI };



    //public Vector2Parameter screenPosition = new Vector2Parameter { value = new Vector2(0.28f, 0.73f) };
    //public Vector2Parameter screenOrigin = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    //public readonly int kaleidoPositionsID = Shader.PropertyToID("kaleidoPositions");
}

public sealed class GlitchRenderer : PostProcessEffectRenderer<Glitch>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/Glitch"));

        sheet.properties.SetFloat("_Intensity", settings.intensity);
        sheet.properties.SetFloat("_ColorIntensity", settings.colorIntensity);
        // sheet.properties.SetTexture("_DispTex", displacementMap);

        settings.flicker += Time.deltaTime * settings.colorIntensity;
        if (settings.flicker > settings.flickerTime)
        {
            sheet.properties.SetFloat("filterRadius", UnityEngine.Random.Range(-3f, 3f) * settings.colorIntensity);
            sheet.properties.SetVector("direction", Quaternion.AngleAxis(UnityEngine.Random.Range(0, 360) * settings.colorIntensity, Vector3.forward) * Vector4.one);
            settings.flicker = 0;
            settings.flickerTime = UnityEngine.Random.value;
        }

        if (settings.colorIntensity == 0)
            sheet.properties.SetFloat("filterRadius", 0);

        settings.glitchup += Time.deltaTime * settings.flipIntensity;
        if (settings.glitchup > settings.glitchupTime)
        {
            if (UnityEngine.Random.value < 0.1f * settings.flipIntensity)
                sheet.properties.SetFloat("flip_up", UnityEngine.Random.Range(0, 1f) * settings.flipIntensity);
            else
                sheet.properties.SetFloat("flip_up", 0);

            settings.glitchup = 0;
            settings.glitchupTime = UnityEngine.Random.value / 10f;
        }

        if (settings.flipIntensity == 0)
            sheet.properties.SetFloat("flip_up", 0);


        settings.glitchdown += Time.deltaTime * settings.flipIntensity;
        if (settings.glitchdown > settings.glitchdownTime)
        {
            if (UnityEngine.Random.value < 0.1f * settings.flipIntensity)
                sheet.properties.SetFloat("flip_down", 1 - UnityEngine.Random.Range(0, 1f) * settings.flipIntensity);
            else
                sheet.properties.SetFloat("flip_down", 1);

            settings.glitchdown = 0;
            settings.glitchdownTime = UnityEngine.Random.value / 10f;
        }

        if (settings.flipIntensity == 0)
            sheet.properties.SetFloat("flip_down", 1);

        if (UnityEngine.Random.value < 0.05 * settings.intensity)
        {
            sheet.properties.SetFloat("displace", UnityEngine.Random.value * settings.intensity);
            sheet.properties.SetFloat("scale", 1 - UnityEngine.Random.value * settings.intensity);
        }
        else
            sheet.properties.SetFloat("displace", 0);

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

using Hubris;

[Serializable]
[PostProcess(typeof(Glitch2Renderer), PostProcessEvent.AfterStack, "Hubris/Glitch2")]
public sealed class Glitch2 : PostProcessEffectSettings
{
    [Range(0, 1)]
    public FloatParameter amount = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter size = new FloatParameter();
    [Range(0, Arithmos.PI)]
    public FloatParameter angle = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter seed = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter seed_x = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter seed_y = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter distortion_x = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter distortion_y = new FloatParameter();
    [Range(0, 1)]
    public FloatParameter col_s = new FloatParameter();


    public readonly int zoomBlurSettingsID = Shader.PropertyToID("zoomBlurSettings");


    //[Range(-Arithmos.PI, Arithmos.PI)]
    //public FloatParameter angle = new FloatParameter { value = -Arithmos.PI };



    //public Vector2Parameter screenPosition = new Vector2Parameter { value = new Vector2(0.28f, 0.73f) };
    //public Vector2Parameter screenOrigin = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    //public readonly int kaleidoPositionsID = Shader.PropertyToID("kaleidoPositions");
}

public sealed class Glitch2Renderer : PostProcessEffectRenderer<Glitch2>
{
    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/Glitch2"));

        sheet.properties.SetFloat("amount", UnityEngine.Random.Range(-settings.amount, settings.amount));
        sheet.properties.SetFloat("size", UnityEngine.Random.Range(-settings.size, settings.size));
        sheet.properties.SetFloat("angle", UnityEngine.Random.Range(-settings.angle, settings.angle));
        sheet.properties.SetFloat("seed", UnityEngine.Random.Range(-settings.seed, settings.seed));
        sheet.properties.SetFloat("seed_x", UnityEngine.Random.Range(-settings.seed_x, settings.seed_x));
        sheet.properties.SetFloat("seed_y", UnityEngine.Random.Range(-settings.seed_y, settings.seed_y));
        sheet.properties.SetFloat("distortion_x", UnityEngine.Random.Range(-settings.distortion_x, settings.distortion_x));
        sheet.properties.SetFloat("distortion_y", UnityEngine.Random.Range(-settings.distortion_y, settings.distortion_y));
        sheet.properties.SetFloat("col_s", UnityEngine.Random.Range(-settings.col_s, settings.col_s));

        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
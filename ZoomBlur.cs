using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
[PostProcess(typeof(ZoomBlurRenderer), PostProcessEvent.AfterStack, "Hubris/ZoomBlur")]
public sealed class ZoomBlur : PostProcessEffectSettings
{
    public Vector2Parameter convergion = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    [Range(20, 80)]
    public IntParameter iterations = new IntParameter { value = 50 };

    [Range(-1f, 1f)]
    public FloatParameter strength = new FloatParameter();

    public readonly int zoomBlurSettingsID = Shader.PropertyToID("zoomBlurSettings");


    //[Range(-Arithmos.PI, Arithmos.PI)]
    //public FloatParameter angle = new FloatParameter { value = -Arithmos.PI };



    //public Vector2Parameter screenPosition = new Vector2Parameter { value = new Vector2(0.28f, 0.73f) };
    //public Vector2Parameter screenOrigin = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    //public readonly int kaleidoPositionsID = Shader.PropertyToID("kaleidoPositions");
}

public sealed class ZoomBlurRenderer : PostProcessEffectRenderer<ZoomBlur>
{
    public override void Render(PostProcessRenderContext context)
    {

        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/ZoomBlur"));
        sheet.properties.SetVector(settings.zoomBlurSettingsID, new Vector4(settings.convergion.value.x, settings.convergion.value.y, settings.iterations, settings.strength));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
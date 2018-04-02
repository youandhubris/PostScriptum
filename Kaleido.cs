using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

using Hubris;

[Serializable]
[PostProcess(typeof(KaleidoRenderer), PostProcessEvent.AfterStack, "Hubris/Kaleido")]
public sealed class Kaleido : PostProcessEffectSettings
{

    [Tooltip("Kaleido effect intensity.")]
    public FloatParameter ratio = new FloatParameter();

    [Range(0, 50)]
    public IntParameter sides = new IntParameter { value = 3 };

    [Range(-Arithmos.PI, Arithmos.PI)]
    public FloatParameter angle = new FloatParameter { value = -Arithmos.PI };

    public readonly int kaleidoSettingsID = Shader.PropertyToID("kaleidoSettings");


    public Vector2Parameter screenPosition = new Vector2Parameter { value = new Vector2(0.28f, 0.73f) };
    public Vector2Parameter screenOrigin = new Vector2Parameter { value = new Vector2(0.5f, 0.5f) };

    public readonly int kaleidoPositionsID = Shader.PropertyToID("kaleidoPositions");


    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value;
    }
}

public sealed class KaleidoRenderer : PostProcessEffectRenderer<Kaleido>
{
    //TODO: NOT WORKING ON BETA, GETS OVERRIDDEN
    //public override void Init()
    //{
    //    settings.ratio.value.x = 16f / 9f;
    //    settings.ratio.value.y = 1f;
    //}

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/Kaleido"));
        sheet.properties.SetVector(settings.kaleidoSettingsID, new Vector4(settings.ratio, settings.sides, settings.angle));
        sheet.properties.SetVector(settings.kaleidoPositionsID, new Vector4(settings.screenPosition.value.x, settings.screenPosition.value.y, settings.screenOrigin.value.x, settings.screenOrigin.value.y));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

using Hubris;

[Serializable]
[PostProcess(typeof(HalfToneRenderer), PostProcessEvent.AfterStack, "Hubris/HalfTone")]
public sealed class HalfTone : PostProcessEffectSettings
{
    [Tooltip("HalfTone effect intensity.")]
    public FloatParameter ratio = new FloatParameter();

    [Range(0f, 200f), Tooltip("HalfTone effect intensity.")]
    public FloatParameter frequency = new FloatParameter { value = 70f };

    [Range(0f, 10f), Tooltip("HalfTone effect intensity.")]
    public FloatParameter scale = new FloatParameter { value = 2f };

    public readonly int halfToneSettingsID = Shader.PropertyToID("halfToneSettings");


    public override bool IsEnabledAndSupported(PostProcessRenderContext context)
    {
        return enabled.value;
    }
}

public sealed class HalfToneRenderer : PostProcessEffectRenderer<HalfTone>
{
    //TODO: NOT WORKING ON BETA, GETS OVERRIDDEN
    //public override void Init()
    //{
    //    settings.ratio.value.x = 16f / 9f;
    //    settings.ratio.value.y = 1f;
    //}

    public override void Render(PostProcessRenderContext context)
    {
        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/HalfTone"));
        sheet.properties.SetVector(settings.halfToneSettingsID, new Vector4(settings.ratio, settings.frequency, settings.scale));
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
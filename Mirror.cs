using System;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

[Serializable]
public sealed class MirrorParameter : ParameterOverride<Hubris.HLSL.Mirror> { }

[Serializable]
[PostProcess(typeof(MirrorRenderer), PostProcessEvent.AfterStack, "Hubris/Mirror")]
public sealed class Mirror : PostProcessEffectSettings
{
    [Tooltip("Mirror effect intensity.")]
    public MirrorParameter side = new MirrorParameter { value = Hubris.HLSL.Mirror.None };
    public readonly int Side = Shader.PropertyToID("side");
}

public sealed class MirrorRenderer : PostProcessEffectRenderer<Mirror>
{
    public override void Render(PostProcessRenderContext context)
    {

        var sheet = context.propertySheets.Get(Shader.Find("Hubris/PostProcess/Mirror"));
        sheet.properties.SetFloat(settings.Side, (float) settings.side.value);
        context.command.BlitFullscreenTriangle(context.source, context.destination, sheet, 0);
    }
}
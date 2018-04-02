using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(HalfTone))]
public sealed class HalfToneeditor : PostProcessEffectEditor<HalfTone>
{
    SerializedParameterOverride ratio;
    SerializedParameterOverride frequency;
    SerializedParameterOverride scale;

    public override void OnEnable()
    {
        ratio = FindParameterOverride(x => x.ratio);
        frequency = FindParameterOverride(x => x.frequency);
        scale = FindParameterOverride(x => x.scale);
    }

    public override void OnInspectorGUI()
    {
        PropertyField(ratio, new UnityEngine.GUIContent("window ratio"));
        PropertyField(frequency);
        PropertyField(scale);
    }
}
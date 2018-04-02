using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(Mirror))]
public sealed class MirrorEditor : PostProcessEffectEditor<Mirror>
{
    SerializedParameterOverride side;

    public override void OnEnable()
    {
        side = FindParameterOverride(x => x.side);
    }

    public override void OnInspectorGUI()
    {
        PropertyField(side, new UnityEngine.GUIContent("Side to Mirror"));
    }
}
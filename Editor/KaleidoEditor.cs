using UnityEngine.Rendering.PostProcessing;
using UnityEditor.Rendering.PostProcessing;

[PostProcessEditor(typeof(Kaleido))]
public sealed class KaleidoEditor : PostProcessEffectEditor<Kaleido>
{
    SerializedParameterOverride ratio;
    SerializedParameterOverride sides;
    SerializedParameterOverride angle;

    SerializedParameterOverride screenPosition;
    SerializedParameterOverride screenOrigin;

    public override void OnEnable()
    {
        ratio = FindParameterOverride(x => x.ratio);
        sides = FindParameterOverride(x => x.sides);
        angle = FindParameterOverride(x => x.angle);

        screenPosition = FindParameterOverride(x => x.screenPosition);
        screenOrigin = FindParameterOverride(x => x.screenOrigin);
    }

    public override void OnInspectorGUI()
    {
        PropertyField(ratio);
        PropertyField(sides);
        PropertyField(angle);

        PropertyField(screenPosition);
        PropertyField(screenOrigin);
    }
}
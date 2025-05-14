using UnityEngine;

[CreateAssetMenu(fileName = "ComponentInfo", menuName = "Electronics/Component Info")]
public class ComponentInfo : ScriptableObject
{
    public string ComponentName;

    [TextArea(3, 8)]
    public string Description;

    public float MinOperativeVoltage;
    public float MaxOperativeVoltage;
    public ComponentType ComponentType;
}
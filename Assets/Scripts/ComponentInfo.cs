using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ComponentInfo", menuName = "Electronics/Component Info")]
public class ComponentInfo : ScriptableObject
{
    public string componentName;
    public string componentManufacturer;

    [TextArea(3, 8)]
    public string description;

    public float minOperativeVoltage;
    public float maxOperativeVoltage;
    public ComponentType componentType;

    public List<ComponentInterfaceType> availableInterfaces = new();
}
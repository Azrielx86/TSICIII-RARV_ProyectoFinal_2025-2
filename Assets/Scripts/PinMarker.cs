using System;
using UnityEngine;

[RequireComponent(typeof(MeshRenderer))]
public class PinMarker : MonoBehaviour
{
    public MeshRenderer sphereRenderer;
    public Guid ConnectionId;

    public void SetColor(Color c) => sphereRenderer.material.color = c;
}

using JetBrains.Annotations;
using UnityEngine;

public class ConnectionLine : MonoBehaviour
{
    [CanBeNull]
    public GameObject pointA;

    [CanBeNull]
    public GameObject pointB;

    public LineRenderer lineRenderer;

    void Start()
    {
        lineRenderer.positionCount = 2;
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
    }

    void Update()
    {
        if (pointA is not null)
            lineRenderer.SetPosition(0, pointA.transform.position);
        if (pointB is not null)
            lineRenderer.SetPosition(1, pointB.transform.position);
    }
}
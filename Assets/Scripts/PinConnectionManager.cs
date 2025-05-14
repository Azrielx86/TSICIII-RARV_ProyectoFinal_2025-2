using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class PinConnectionManager : MonoBehaviour
{
    public GameObject markerPrefab;

    [CanBeNull]
    private GpioPin _connectionA;

    [CanBeNull]
    private GpioPin _connectionB;

    private readonly List<GameObject> _activeMarkers = new();

    public readonly List<PinConnection> ActiveConnections = new();

    public Material lineMaterial;

    public void SelectPin(GpioPin pin)
    {
        if (_connectionA is null)
        {
            _connectionA = pin;
            MarkPin(_connectionA);
        }
        else if (_connectionB is null)
        {
            _connectionB = pin;
            MarkPin(_connectionB);
        }
        else
        {
            Debug.LogWarning("Already selected two pins. Please create a connection or reset.");
        }
    }
    
    private void MarkPin(GpioPin pin)
    {
        var marker = Instantiate(markerPrefab, pin.connectionPount.position, Quaternion.identity);
        marker.transform.SetParent(pin.transform, worldPositionStays: true);
        _activeMarkers.Add(marker);
    }

    public void CreateConnection()
    {
        if (_connectionA is null || _connectionB is null)
            return;

        Debug.Log($"Creating connection between {_connectionA.id} and {_connectionB.id}");

        // TODO : Validate Connections

        var connectionId = Guid.NewGuid();
        var color = GenerateRandomColor(Color.white, 0.8f);

        // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
        foreach (var marker in _activeMarkers)
        {
            var pm = marker.GetComponent<PinMarker>();
            pm.ConnectionId = connectionId;
            pm.SetColor(color);
        }

        ActiveConnections.Add(
            new PinConnection
            {
                ID = connectionId,
                ConnectionA = new ConnectionInfo
                {
                    ConnectionPoint = _connectionA,
                    Origin = _connectionA.transform.parent.GetComponentInParent<ElectronicComponent>()
                },
                ConnectionB = new ConnectionInfo
                {
                    ConnectionPoint = _connectionB,
                    Origin = _connectionB.transform.parent.GetComponentInParent<ElectronicComponent>()
                },
                Markers = _activeMarkers.ToArray(),
                Color = color
            }
        );

        _connectionA = null;
        _connectionB = null;
        _activeMarkers.Clear();
    }

    public void RemoveConnection(PinConnection connection)
    {
        foreach (var marker in connection.Markers)
            Destroy(marker);

        ActiveConnections.Remove(connection);

    }

    public void ShowConnection(PinConnection connection)
    {
        var lineGameObject = new GameObject();
        var conn = lineGameObject.AddComponent<ConnectionLine>();
        var line = lineGameObject.AddComponent<LineRenderer>();
        conn.lineRenderer = line;
        
        var diffuseMat = new Material(Shader.Find("Standard"))
        {
            color = connection.Color
        };
        line.material = diffuseMat;

        conn.pointA = connection.ConnectionA.ConnectionPoint.gameObject;
        conn.pointB = connection.ConnectionB.ConnectionPoint.gameObject;
        
        //
        // line.SetPosition(0, connection.ConnectionA.ConnectionPoint.transform.position);
        // line.SetPosition(1, connection.ConnectionB.ConnectionPoint.transform.position);
        //
        Destroy(lineGameObject, 5);
        Destroy(diffuseMat, 5);
    }

    private static Color GenerateRandomColor(Color mix, float mixRatio)
    {
        var randRed = Random.Range(0, 256);
        var randGreen = Random.Range(0, 256);
        var randBlue = Random.Range(0, 256);

        var red = (randRed + mix.r) * mixRatio;
        var green = (randGreen + mix.g) * mixRatio;
        var blue = (randBlue + mix.b) * mixRatio;

        return new Color(red / 256.0f, green / 256.0f, blue / 256.0f);
    }
}
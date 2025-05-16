using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;

public class PinConnectionManager : MonoBehaviour
{
    public GameObject markerPrefab;
    public Transform currentConnectionsPanel;

    [CanBeNull]
    private GpioPin _connectionA;

    [CanBeNull]
    private GpioPin _connectionB;

    private readonly List<GameObject> _activeMarkers = new();

    public readonly List<PinConnection> ActiveConnections = new();

    private void Start()
    {
        currentConnectionsPanel.gameObject.SetActive(false);
        currentConnectionsPanel.Find("Source").GetComponent<TMP_Text>().text = "";
        currentConnectionsPanel.Find("Destination").GetComponent<TMP_Text>().text = "";
    }

    public void SelectPin(GpioPin pin)
    {
        currentConnectionsPanel.gameObject.SetActive(true);
        if (_connectionA is null)
        {
            _connectionA = pin;
            MarkPin(_connectionA);

            var sourceText = currentConnectionsPanel.Find("Source").GetComponent<TMP_Text>();
            var src = pin.transform.parent.GetComponentInParent<ElectronicComponent>().name;
            sourceText.text = $"{src} - {pin.id}";
        }
        else if (_connectionB is null)
        {
            _connectionB = pin;
            MarkPin(_connectionB);
            var destination = currentConnectionsPanel.Find("Destination").GetComponent<TMP_Text>();
            var dst = pin.transform.parent.GetComponentInParent<ElectronicComponent>().name;
            destination.text = $"{dst} - {pin.id}";
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

        var candidateConnectionA = new ConnectionInfo
        {
            ConnectionPoint = _connectionA,
            Origin = _connectionA.transform.parent.GetComponentInParent<ElectronicComponent>()
        };
        var pinAIndex = candidateConnectionA.Origin.GetPinIndex(_connectionA);

        var candidateConnectionB = new ConnectionInfo
        {
            ConnectionPoint = _connectionB,
            Origin = _connectionB.transform.parent.GetComponentInParent<ElectronicComponent>()
        };
        var pinBIndex = candidateConnectionB.Origin.GetPinIndex(_connectionB);

        var validA = candidateConnectionA.Origin.ValidateConnection(candidateConnectionB, pinAIndex);
        var validB = candidateConnectionB.Origin.ValidateConnection(candidateConnectionA, pinBIndex);

        if (validA && validB)
        {
            var connectionId = Guid.NewGuid();
            var color = GenerateRandomColor(Color.white, 0.8f);

            // ReSharper disable once ForeachCanBePartlyConvertedToQueryUsingAnotherGetEnumerator
            foreach (var marker in _activeMarkers)
            {
                var pm = marker.GetComponent<PinMarker>();
                pm.ConnectionId = connectionId;
                pm.SetColor(color);
            }

            candidateConnectionA.Origin.AddValidPin(candidateConnectionA.ConnectionPoint);
            candidateConnectionB.Origin.AddValidPin(candidateConnectionB.ConnectionPoint);

            var pinConnection = new PinConnection
            {
                ID = connectionId,
                ConnectionA = candidateConnectionA,
                ConnectionB = candidateConnectionB,
                Markers = _activeMarkers.ToArray(),
                Color = color
            };

            if (candidateConnectionA.Origin.canBeActivated)
                candidateConnectionA.Origin.ActivateComponent();

            if (candidateConnectionB.Origin.canBeActivated)
                candidateConnectionB.Origin.ActivateComponent();

            ActiveConnections.Add(pinConnection);
        }
        else
        {
            foreach (var marker in _activeMarkers)
                Destroy(marker);
            Debug.LogWarning("Invalid connection!");
        }

        currentConnectionsPanel.gameObject.SetActive(false);
        currentConnectionsPanel.Find("Source").GetComponent<TMP_Text>().text = "";
        currentConnectionsPanel.Find("Destination").GetComponent<TMP_Text>().text = "";
        CleanConnections();
    }

    public void RemoveConnection(PinConnection connection)
    {
        foreach (var marker in connection.Markers)
            Destroy(marker);

        var connA = connection.ConnectionA;
        var connB = connection.ConnectionB;

        connA.Origin.RemoveConnection(connA.ConnectionPoint);
        connB.Origin.RemoveConnection(connB.ConnectionPoint);

        if (connA.Origin.canBeActivated)
            connA.Origin.DeactivateComponent();

        if (connB.Origin.canBeActivated)
            connB.Origin.DeactivateComponent();

        ActiveConnections.Remove(connection);
    }

    public void ShowAllConnections() => ActiveConnections.ForEach(ShowConnection);

    public void CleanConnections()
    {
        currentConnectionsPanel.gameObject.SetActive(false);
        currentConnectionsPanel.Find("Source").GetComponent<TMP_Text>().text = "";
        currentConnectionsPanel.Find("Destination").GetComponent<TMP_Text>().text = "";
        _connectionA = null;
        _connectionB = null;
        _activeMarkers.Clear();
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

        Destroy(lineGameObject, 5);
        Destroy(diffuseMat, 5);
    }

    public void ResetScene() => SceneManager.LoadScene(SceneManager.GetActiveScene().name);

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
using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using NUnit.Framework;
using UnityEngine;

public class PinConnectionManager : MonoBehaviour
{
    public GameObject markerPrefab;

    [CanBeNull]
    private GpioPin _connectionA;
    [CanBeNull]
    private GpioPin _connectionB;
    
    private readonly List<GameObject> _activeMarkers = new();
    private readonly List<Tuple<GpioPin, GpioPin>> _activeConnections = new();

    
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
        _activeMarkers.Add(marker);
    }

    public void CreateConnection()
    {
        if (_connectionA is null || _connectionB is null)
            return;
        
        Debug.Log($"Creating connection between {_connectionA.id} and {_connectionB.id}");
        
        // TODO : Validate Connections
        
        _activeConnections.Add(new Tuple<GpioPin, GpioPin>(_connectionA, _connectionB));
        
        _connectionA = null;
        _connectionB = null;

        foreach (var marker in _activeMarkers)
            Destroy(marker);
        
        _activeMarkers.Clear();
    }
}

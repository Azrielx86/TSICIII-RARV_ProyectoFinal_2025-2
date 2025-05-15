using System;
using System.Collections.Generic;
using NUnit.Framework;
using UnityEngine;

public class GpioPin : MonoBehaviour
{
    public string id;
    public List<PinType> type;
    public Transform connectionPount;

    public List<PinType> compatiblePins = new() { PinType.Digital };

    public List<GpioPin> connectedPins = new();

    private void Start()
    {
        connectionPount = transform;
    }
}
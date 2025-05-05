using System;
using System.Collections.Generic;
using UnityEngine;

public struct ConnectionInfo
{
    public GpioPin ConnectionPoint;
    public DevBoard Origin;
}

public class PinConnection
{
    public Guid ID;
    public ConnectionInfo ConnectionA;
    public ConnectionInfo ConnectionB;
    public GameObject[] Markers;
    public Color Color;
}
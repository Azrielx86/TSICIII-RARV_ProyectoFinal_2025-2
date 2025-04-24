using System;
using System.Collections.Generic;
using UnityEngine;

public class GpioPin : MonoBehaviour
{
    public string id;
    public List<PinType> type;
    public Transform connectionPount;
    private void Start()
    {
        connectionPount = transform;
    }
}
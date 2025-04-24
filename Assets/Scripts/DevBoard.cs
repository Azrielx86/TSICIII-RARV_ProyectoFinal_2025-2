using System;
using System.Collections.Generic;
using UnityEngine;

public class DevBoard : MonoBehaviour
{
    public new string name;
    public List<GpioPin> pins = new();

    private void OnMouseDown()
    {
        Debug.Log("Board clicked.");
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

public class DevBoard : MonoBehaviour
{
    public new string name;
    public List<GpioPin> pins = new();

    [Header("Selector UI")]
    public PinSelectorUI selectorUI;

    private void OnMouseDown()
    {
        selectorUI.ShowPins(pins);
    }
}

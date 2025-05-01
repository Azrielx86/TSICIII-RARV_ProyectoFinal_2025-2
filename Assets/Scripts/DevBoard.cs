using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DevBoard : MonoBehaviour
{
    public new string name;
    public List<GpioPin> pins = new();

    [Header("Selector UI")]
    public PinSelectorUI selectorUI;

    private void OnMouseDown()
    {
        if (EventSystem.current.IsPointerOverGameObject())
            return;
        
        selectorUI.ShowPins(pins);
    }
}

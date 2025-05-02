using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// TODO : Change name to ElectonicComponent
/// </summary>
public class DevBoard : MonoBehaviour
{
    public new string name;
    public List<GpioPin> pins = new();

    [Header("Selector UI")]
    public PinSelectorUI selectorUI;

    private void OnMouseDown()
    {
#if !PLATFORM_ANDROID || UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        selectorUI.ShowPins(pins);
#endif
    }

    
    private void Update()
    {
#if (PLATFORM_ANDROID || UNITY_ANDROID) && !UNITY_EDITOR
        foreach (var touch in Input.touches)
        {
            if (touch.phase != TouchPhase.Began) continue;

            if (EventSystem.current.IsPointerOverGameObject(touch.fingerId))
                return;

            if (Camera.main is null) continue;

            var ray = Camera.main.ScreenPointToRay(touch.position);
            if (!Physics.Raycast(ray, out RaycastHit hit)) continue;
            if (hit.transform == transform)
                selectorUI.ShowPins(pins);
        }
#endif
    }
}

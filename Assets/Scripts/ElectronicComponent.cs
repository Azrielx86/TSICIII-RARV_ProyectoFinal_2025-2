using System;
using System.Collections.Generic;
using System.Linq;
using ConnectionActions;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;

public class ElectronicComponent : MonoBehaviour
{
    [Header("Component info")]
    public new string name;

    public ComponentInfo componentInfo;
    public List<GpioPin> pins = new();

    [Header("Selector UI")]
    public PinSelectorUI selectorUI;

    [Header("Component interactions")]
    public bool canBeActivated = false;

    public List<GpioPin> requiredActivatePins = new();

    private void Start()
    {
        if (!canBeActivated) return;
        var activator = GetComponent<IComponentAction>();
        activator.OnInvalidConnection();
    }

    private void OnMouseDown()
    {
#if !PLATFORM_ANDROID || UNITY_EDITOR
        if (EventSystem.current.IsPointerOverGameObject())
            return;

        selectorUI.ShowPins(pins);
#endif
    }

    public void ActivateComponent()
    {
        if (!canBeActivated) return;

        if (requiredActivatePins.Any()) return;

        var activator = GetComponent<IComponentAction>();
        activator.OnValidConnection();
    }

    public void DeactivateComponent()
    {
        if (!canBeActivated) return;

        var activator = GetComponent<IComponentAction>();
        activator.OnInvalidConnection();
    }

    /// <summary>
    /// TODO
    /// </summary>
    public void RemoveConnection(GpioPin pin)
    {
        requiredActivatePins.Add(pin);
    }

    public void AddValidPin(GpioPin pin)
    {
        requiredActivatePins.Remove(pin);
    }
    
    public bool ValidateConnection(ConnectionInfo target, int gpioPinTargetIndex)
    {
        var pin = pins[gpioPinTargetIndex];

        // requiredActivatePins.Remove(pin);
        
        return pin.compatiblePins.Any(pinType => target.ConnectionPoint.type.Contains(pinType));
    }

    public int GetPinIndex(GpioPin pin) => pins.IndexOf(pin);
    
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
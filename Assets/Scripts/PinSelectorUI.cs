using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PinSelectorUI : MonoBehaviour
{
    public GameObject pinSelectorItemPrefab;
    public Transform contentContainer;

    public void ShowPins(List<GpioPin> pins)
    {
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var pin in pins)
        {
            var pinItem = Instantiate(pinSelectorItemPrefab, contentContainer);

            pinItem.transform.Find("PinName").GetComponent<TMP_Text>().text = pin.id;

            var typesContainer = pinItem.transform.Find("TypesContainer");
            foreach (var pinType in pin.type)
            {
               var pinTypeObject = new GameObject(pinType.ToString());
               pinTypeObject.transform.SetParent(typesContainer, false);
               
               // Add an Image component and set its color
               var image = pinTypeObject.AddComponent<UnityEngine.UI.Image>();
               var pinColor = GetColorFromPinType(pinType);
               image.color = pinColor;
               
               // Create a child GameObject for the pin name
               var pinNameObject = new GameObject("TypeName");
               pinNameObject.transform.SetParent(pinTypeObject.transform, false);
               
               // Add a TextMeshPro component and set the pin name
               var textMeshPro = pinNameObject.AddComponent<TextMeshProUGUI>();
               textMeshPro.text = pinType.ToString();
               textMeshPro.fontSize = 72;
               textMeshPro.alignment = TextAlignmentOptions.Center;
               
               image.rectTransform.sizeDelta = new Vector2(textMeshPro.preferredWidth + 24, textMeshPro.preferredHeight + 24);
               
               // TODO : Fix Later
            }
            
            // Set Button Action
        }
    }

    private Color GetColorFromPinType(PinType pin) => pin switch
    {
        PinType.Analog => Color.cyan,
        PinType.Digital => Color.green,
        PinType.Gnd => Color.gray,
        PinType.Vcc3V => new Color(1.0f, 1.0f, 1.0f),
        PinType.Vcc5V => Color.red,
        PinType.Vin => Color.magenta,
        PinType.Pwm => new Color(1.0f, 1.0f, 1.0f),
        PinType.I2CScl => new Color(1.0f, 1.0f, 1.0f),
        PinType.I2CSda => new Color(1.0f, 1.0f, 1.0f),
        PinType.UartTx => new Color(1.0f, 1.0f, 1.0f),
        PinType.UartRx => new Color(1.0f, 1.0f, 1.0f),
        PinType.SpiMosi => new Color(1.0f, 1.0f, 1.0f),
        PinType.SpiMiso => new Color(1.0f, 1.0f, 1.0f),
        PinType.SpiSck => new Color(0.5f, 1.0f, 1.0f),
        PinType.SpiCs => new Color(1.0f, 1.0f, 1.0f),
        _ => throw new ArgumentOutOfRangeException(nameof(pin), pin, null)
    };
}
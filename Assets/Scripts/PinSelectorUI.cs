using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PinSelectorUI : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    public GameObject pinSelectorItemPrefab;
    public Transform contentContainer;

    public Animator uiAnimator;

    public void ShowPins(List<GpioPin> pins)
    {
        uiAnimator.SetBool(IsOpen, true);
        
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
                var image = pinTypeObject.AddComponent<Image>();
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
                textMeshPro.color = LumaText(pinColor);
                textMeshPro.fontStyle = FontStyles.Bold;
                textMeshPro.rectTransform.anchorMin = Vector2.zero;
                textMeshPro.rectTransform.anchorMax = Vector2.one;

                image.rectTransform.sizeDelta =
                    new Vector2(textMeshPro.preferredWidth + 32, textMeshPro.preferredHeight + 32);

                // TODO : Fix Later
            }

            // Set Button Action
            var button = pinItem.transform.Find("PinSelect").GetComponent<Button>();
            button.onClick.AddListener(() =>
            {
                Debug.Log($"Pin {pin.id} clicked");
                FindFirstObjectByType<PinConnectionManager>().SelectPin(pin);
                uiAnimator.SetBool(IsOpen, false);
            });
        }
    }

    private static Color LumaText(Color color) =>
        0.299f * color.r + 0.587f * color.g + 0.114f * color.b > 0.5f ? Color.black : Color.white;

    private static Color GetColorFromPinType(PinType pin) => pin switch
    {
        PinType.Analog => new Color(0.980f, 0.702f, 0.529f),
        PinType.Digital => new Color(0.455f, 0.780f, 0.925f),
        PinType.Gnd => new Color(0.424f, 0.439f, 0.525f),
        PinType.Vcc3V => new Color(0.953f, 0.545f, 0.659f),
        PinType.Vcc5V => new Color(0.953f, 0.545f, 0.659f),
        PinType.Vin => new Color(0.953f, 0.545f, 0.659f),
        PinType.Pwm => new Color(0.961f, 0.878f, 0.863f),
        PinType.I2CScl => new Color(0.537f, 0.706f, 0.980f),
        PinType.I2CSda => new Color(0.537f, 0.706f, 0.980f),
        PinType.UartTx => new Color(0.651f, 0.890f, 0.631f),
        PinType.UartRx => new Color(0.651f, 0.890f, 0.631f),
        PinType.SpiMosi => new Color(0.796f, 0.651f, 0.969f),
        PinType.SpiMiso => new Color(0.796f, 0.651f, 0.969f),
        PinType.SpiSck => new Color(0.796f, 0.651f, 0.969f),
        PinType.SpiCs => new Color(0.796f, 0.651f, 0.969f),
        PinType.Reset => new Color(0.976f, 0.886f, 0.686f),
        PinType.ARef => new Color(0.922f, 0.627f, 0.675f),
        _ => throw new ArgumentOutOfRangeException(nameof(pin), pin, null)
    };
}
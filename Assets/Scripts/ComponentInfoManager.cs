using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ComponentInfoManager : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");

    public bool infoViewMode;

    [Header("UI Controls")]
    public Animator uiAnimator;

    public Transform componentUI;

    [Header("Button mode management")]
    public Image modeBtnIcon;

    public Sprite infoModeImage;
    public Sprite connectionModeImage;


    public void ToggleInfoAndConnectionMode()
    {
        infoViewMode = !infoViewMode;
        modeBtnIcon.sprite = infoViewMode ? connectionModeImage : infoModeImage;
    }

    public void OpenUIForComponent(ElectronicComponent component)
    {
        uiAnimator.SetBool(IsOpen, true);
        var info = component.componentInfo;

        var container = componentUI.Find("Container");

        var title = container.Find("Header").Find("ComponentName");
        var type = container.Find("Header").Find("ComponentType");
        var about = container.Find("Info").Find("AboutContent");
        var interfacesContainer = container.Find("Header").Find("InterfacesContainer");

        title.GetComponent<TMP_Text>().text = info.componentName;
        type.GetComponent<TMP_Text>().text = info.componentType.ToString();
        about.GetComponent<TMP_Text>().text = info.description;

        foreach (Transform child in interfacesContainer)
            Destroy(child.gameObject);

        foreach (var itype in info.availableInterfaces)
        {
            var pinTypeObject = new GameObject(itype.ToString());
            pinTypeObject.transform.SetParent(interfacesContainer, false);

            // Add an Image component and set its color
            var image = pinTypeObject.AddComponent<Image>();
            var pinColor = GetColorFromInterfaceType(itype);
            image.color = pinColor;

            // Create a child GameObject for the pin name
            var pinNameObject = new GameObject("TypeName");
            pinNameObject.transform.SetParent(pinTypeObject.transform, false);

            // Add a TextMeshPro component and set the pin name
            var textMeshPro = pinNameObject.AddComponent<TextMeshProUGUI>();
            textMeshPro.text = itype.ToString();
            textMeshPro.fontSize = 72;
            textMeshPro.alignment = TextAlignmentOptions.Center;
            textMeshPro.color = PinSelectorUI.LumaText(pinColor);
            textMeshPro.fontStyle = FontStyles.Bold;
            textMeshPro.rectTransform.anchorMin = Vector2.zero;
            textMeshPro.rectTransform.anchorMax = Vector2.one;

            image.rectTransform.sizeDelta =
                new Vector2(textMeshPro.preferredWidth + 32, textMeshPro.preferredHeight + 32);

            // TODO : Fix Later
        }
    }

    public void CloseMenu() => uiAnimator.SetBool(IsOpen, false);

    public static Color GetColorFromInterfaceType(ComponentInterfaceType interfaceType) => interfaceType switch
    {
        ComponentInterfaceType.I2C => new Color(0.537f, 0.706f, 0.980f),
        ComponentInterfaceType.Uart => new Color(0.651f, 0.890f, 0.631f),
        ComponentInterfaceType.Spi => new Color(0.796f, 0.651f, 0.969f),
        _ => throw new ArgumentOutOfRangeException(nameof(interfaceType), interfaceType, null)
    };
}
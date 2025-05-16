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

        title.GetComponent<TMP_Text>().text = info.componentName;
        type.GetComponent<TMP_Text>().text = info.componentType.ToString();
        about.GetComponent<TMP_Text>().text = info.description;
    }
    
    public void CloseMenu() => uiAnimator.SetBool(IsOpen, false);
}
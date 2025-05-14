using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class ConnectionsUI : MonoBehaviour
{
    private static readonly int IsOpen = Animator.StringToHash("IsOpen");
    public GameObject connectionInfoPrefab;
    public Transform contentContainer;
    public PinConnectionManager connectionManager;
    public Animator uiAnimator;

    public void ShowMenu()
    {
        uiAnimator.SetBool(IsOpen, true);
        
        foreach (Transform child in contentContainer)
        {
            Destroy(child.gameObject);
        }

        foreach (var activeConnection in connectionManager.ActiveConnections)
        {
            var info = Instantiate(connectionInfoPrefab, contentContainer);

            var connA = info.transform.Find("ConnectionA").GetComponent<TMP_Text>();
            var connB = info.transform.Find("ConnectionB").GetComponent<TMP_Text>();
            var srcA = info.transform.Find("SourceA").GetComponent<TMP_Text>();
            var srcB = info.transform.Find("SourceB").GetComponent<TMP_Text>();
            var connColor = info.transform.Find("ConnectionColor").GetComponent<Image>();

            connA.text = activeConnection.ConnectionA.ConnectionPoint.id;
            connB.text = activeConnection.ConnectionB.ConnectionPoint.id;
            srcA.text = activeConnection.ConnectionA.Origin.name;
            srcB.text = activeConnection.ConnectionB.Origin.name;
            connColor.color = activeConnection.Color;

            // TODO : Add button action
            var btnRemove = info.transform.Find("BtnRemove").GetComponent<Button>();
            btnRemove.onClick.AddListener(() =>
            {
                Debug.Log("Removing Connection.");
                FindFirstObjectByType<PinConnectionManager>().RemoveConnection(activeConnection);
                Destroy(info);
            });
        }
    }

    public void CloseMenu() => uiAnimator.SetBool(IsOpen, false);
}
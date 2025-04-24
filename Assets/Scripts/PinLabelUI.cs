using UnityEngine;

public class PinLabelUI : MonoBehaviour
{
    private void Update()
    {
        if (Camera.main is not null)
        {
            transform.LookAt(Camera.main.transform.forward);
        }
    }
}

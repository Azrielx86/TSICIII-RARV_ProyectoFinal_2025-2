using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuActions : MonoBehaviour
{
    [SerializeField]
    public Object mainARScene;

    public void GotoARMode()
    {
        SceneManager.LoadScene(mainARScene.name);
    }
}
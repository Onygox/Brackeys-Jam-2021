using UnityEngine;

public class EndMenuUiManager : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
    }

    public void ReturnToMenu() {
        Destroy(GameObject.Find("DontDestroyOnLoad"));
        PersistentManager.Instance.LoadSceneByIndex(0);
    }
}

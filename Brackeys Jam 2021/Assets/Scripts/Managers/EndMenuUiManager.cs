using UnityEngine;

public class EndMenuUiManager : MonoBehaviour
{
    public void QuitGame() {
        Application.Quit();
    }

    public void ReturnToMenu() {
        PersistentManager.Instance.LoadSceneByIndex(0);
    }
}

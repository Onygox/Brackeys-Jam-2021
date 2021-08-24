using UnityEngine;
using UnityEngine.SceneManagement;
public class StartMenuUiManager : MonoBehaviour
{
    public int GameSceneIndex;
    public void LoadGameScene() {
        SceneManager.LoadScene(GameSceneIndex);
    }
    public void QuitGame() {
        Application.Quit();
    }
}

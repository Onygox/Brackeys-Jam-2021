using UnityEngine;

public class UiActionManager : MonoBehaviour
{
    
    public void OnHover() {
        PersistentManager.Instance.soundManager.PlaySound(11 , 1.0f , 1);
    }

    public void OnClick() {
        PersistentManager.Instance.soundManager.PlaySound(12 , 1.0f , 1);
    }

    public void OnBack() {
        PersistentManager.Instance.soundManager.PlaySound(10 , 1.0f , 1);
    }

}

using UnityEngine;

public class UiActionManager : MonoBehaviour
{

    public AudioClip hoverSfx;
    public AudioClip clickSfx;
    public AudioClip backSfx;
    
    public void OnHover() {
        PersistentManager.Instance.soundManager.PlaySound(11 , 1.0f , 0.0f);
    }

    public void OnClick() {
        PersistentManager.Instance.soundManager.PlaySound(12 , 0.4f , 0.0f);
    }

    public void OnBack() {
        PersistentManager.Instance.soundManager.PlaySound(10 , 0.4f , 0.0f);
    }

}

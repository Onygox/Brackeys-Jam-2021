using UnityEngine;

public class UiActionManager : MonoBehaviour
{

    public int hoverSfx;
    public int clickSfx;
    public int backSfx;
    
    public void OnHover() {
        PersistentManager.Instance.soundManager.PlaySound(hoverSfx , 0.4f , 0.0f);
    }

    public void OnClick() {
        PersistentManager.Instance.soundManager.PlaySound(clickSfx , 0.4f , 0.0f);
    }

    public void OnBack() {
        PersistentManager.Instance.soundManager.PlaySound(backSfx , 0.4f , 0.0f);
    }

}

using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class UiActionManager : MonoBehaviour
{

    public AudioClip hoverSfx;
    public AudioClip clickSfx;
    public AudioClip backSfx;
    public AudioSource hoverSfxPlayer;
    public AudioSource clickSfxPlayer;
    public AudioSource backSfxPlayer;

    private void Awake() {
        hoverSfxPlayer.clip = hoverSfx;
        clickSfxPlayer.clip = clickSfx;
        backSfxPlayer.clip = backSfx;
    }
    public void OnHover() {
        hoverSfxPlayer.Play();
    }

    public void OnClick() {
        clickSfxPlayer.Play();
    }

    public void OnBack() {
        backSfxPlayer.Play();
    }

}

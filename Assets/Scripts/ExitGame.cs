using UnityEngine;

public class ExitGame : MonoBehaviour
{
    public void Exit()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ClickSFX);
        Application.Quit();
    }
}

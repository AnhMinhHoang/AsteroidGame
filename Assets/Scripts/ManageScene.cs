using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ManageScene : MonoBehaviour
{
    [SerializeField] private string sceneName;
    [SerializeField] private string gameplayScene;

    public void ChangeScene()
    {
        AudioManager.Instance.PlaySFX(AudioManager.Instance.ClickSFX);
        SceneManager.LoadScene(sceneName);
        if (sceneName.Equals(gameplayScene)) {
            AudioManager.Instance.musicSorce.clip = AudioManager.Instance.Background;
            // AudioManager.Instance.musicSorce.Play();
        }
    }
}

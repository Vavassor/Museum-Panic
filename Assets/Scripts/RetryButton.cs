using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RetryButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        ReloadCurrentScene();
    }

    private void ReloadCurrentScene()
    {
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
    }
}

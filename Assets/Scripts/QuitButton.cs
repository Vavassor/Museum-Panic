using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class QuitButton : MonoBehaviour
{
    void Start()
    {
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    private void OnClick()
    {
        SceneManager.LoadScene("Title");
    }
}

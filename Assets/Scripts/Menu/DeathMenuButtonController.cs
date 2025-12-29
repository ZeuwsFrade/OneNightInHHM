using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class DeathMenuButtonController : MonoBehaviour
{
    [Header("Кнопки")]
    public Button restartButton;
    public Button menuButton;

    [Header("Настройки сцены")]
    public string ResSceneName;
    public string MenuSceneName;

    void Start()
    {
        SetupButtonListeners();
    }

    void Update()
    {
    }

    private void SetupButtonListeners()
    {
        if (restartButton != null)
        {
            restartButton.onClick.RemoveAllListeners();
            restartButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("Restart button not found");
        }
        if (menuButton != null)
        {
            menuButton.onClick.RemoveAllListeners();
            menuButton.onClick.AddListener(OnMenuButtonClicked);
        }
        else
        {
            Debug.LogError("Restart button not found");
        }

    }

    public void OnPlayButtonClicked()
    {
        Debug.Log("Restart button clicked. Loading scene: " + ResSceneName);

        StartCoroutine(LoadGameScene(ResSceneName));
    }

    public void OnMenuButtonClicked()
    {
        Debug.Log("Restart button clicked. Loading scene: " + MenuSceneName);

        StartCoroutine(LoadGameScene(MenuSceneName));
    }

    private System.Collections.IEnumerator LoadGameScene(string gameSceneName)
    {
        yield return null;

        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is not set");
        }
    }

    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }
}

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

public class MenuButtonsController : MonoBehaviour
{
    [Header("Кнопки")]
    public Button playButton;
    public Button optionsButton;
    public Button exitButton;

    [Header("Настройки сцены")]
    public string gameSceneName = "GameScene";

    [Header("Настройки опций")]
    public GameObject optionsPopup;
    public bool closeOptionsWithESC = true;

    void Start()
    {
        if (playButton == null || optionsButton == null || exitButton == null)
        {
            FindButtonsAutomatically();
        }

        SetupButtonListeners();

        if (optionsPopup != null)
        {
            optionsPopup.SetActive(false);
        }
    }

    void Update()
    {
        // Закрытие опций по ESC
        if (closeOptionsWithESC && Input.GetKeyDown(KeyCode.Escape))
        {
            CloseOptions();
        }
    }

    private void FindButtonsAutomatically()
    {
        Button[] childButtons = GetComponentsInChildren<Button>();

        foreach (Button button in childButtons)
        {
            string buttonName = button.gameObject.name.ToLower();

            if (buttonName.Contains("play"))
                playButton = button;
            else if (buttonName.Contains("options"))
                optionsButton = button;
            else if (buttonName.Contains("exit"))
                exitButton = button;
        }
    }

    private void SetupButtonListeners()
    {
        if (playButton != null)
        {
            playButton.onClick.RemoveAllListeners();
            playButton.onClick.AddListener(OnPlayButtonClicked);
        }
        else
        {
            Debug.LogError("Play button not found!");
        }

        if (optionsButton != null)
        {
            optionsButton.onClick.RemoveAllListeners();
            optionsButton.onClick.AddListener(OnOptionsButtonClicked);
        }
        else
        {
            Debug.LogError("Options button not found!");
        }

        if (exitButton != null)
        {
            exitButton.onClick.RemoveAllListeners();
            exitButton.onClick.AddListener(OnExitButtonClicked);
        }
        else
        {
            Debug.LogError("Exit button not found!");
        }
    }


    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked! Loading scene: " + gameSceneName);

        StartCoroutine(LoadGameScene());
    }

    private System.Collections.IEnumerator LoadGameScene()
    {
        // Добавьте здесь эффекты (затемнение, анимация и т.д.)

        // Ждём завершения кадра
        yield return null;

        // Загружаем сцену
        if (!string.IsNullOrEmpty(gameSceneName))
        {
            SceneManager.LoadScene(gameSceneName);
        }
        else
        {
            Debug.LogError("Game scene name is not set!");
        }
    }

    public void OnOptionsButtonClicked()
    {
        Debug.Log("Options button clicked!");

        if (optionsPopup != null)
        {
            optionsPopup.SetActive(true);
        }
        else
        {
            Debug.LogWarning("Options popup is not assigned!");
        }
    }

    public void OnExitButtonClicked()
    {
        Debug.Log("Exit button clicked!");

        QuitGame();
    }

    // Закрытие окна опций
    public void CloseOptions()
    {
        if (optionsPopup != null && optionsPopup.activeSelf)
        {
            optionsPopup.SetActive(false);
        }
    }


    // Методы для внешнего вызова (например, из других скриптов)
    public void LoadScene(string sceneName)
    {
        if (!string.IsNullOrEmpty(sceneName))
        {
            SceneManager.LoadScene(sceneName);
        }
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }

}
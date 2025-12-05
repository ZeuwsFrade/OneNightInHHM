using UnityEngine;
using UnityEngine.SceneManagement;

public class EscapeScript : MonoBehaviour
{
    public string gameSceneName = "MainMenu";
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Menu");
            StartCoroutine(LoadGameScene());
        }
    }

    private System.Collections.IEnumerator LoadGameScene()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Debug.Log("Help");
        // Ждём завершения кадра
        yield return null;
        Debug.Log("pobeda");
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
}

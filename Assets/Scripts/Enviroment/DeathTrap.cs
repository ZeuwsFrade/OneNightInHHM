using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrap : TrapBase
{
    [Header("Настройки смерти")]
    [SerializeField] private string deathSceneName = "MainMenu";
    [SerializeField] private string deathMessage = "Вас убили!";

    protected override void ActivateTrapEffect()
    {
        Debug.Log(deathMessage);

        // Загружаем главное меню
        if (!string.IsNullOrEmpty(deathSceneName))
        {
            SceneManager.LoadScene(deathSceneName);
        }
        else
        {
            Debug.LogError("DeathTrap: Не указано имя сцены для загрузки!");
        }
    }
}
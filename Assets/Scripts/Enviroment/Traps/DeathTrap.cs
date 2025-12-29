using UnityEngine;
using UnityEngine.SceneManagement;

public class DeathTrap : TrapBase
{
    [Header("Настройки смерти")]
    [SerializeField] private string deathMessage;

    protected override void ActivateTrapEffect()
    {
        PlayerInteraction playerDeath = playerObject.GetComponent<PlayerInteraction>();
        if (playerDeath != null)
        {
            playerDeath.Death();
        }
    }
}
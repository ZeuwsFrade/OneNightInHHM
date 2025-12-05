using UnityEngine;

public class TorchController : MonoBehaviour
{
    private Light torch;
    void Start()
    {
        torch = GetComponent<Light>();
        torch.enabled = false; // Изначально фонарик выключен  
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F)) // Задать нужную клавишу по выбору {  
            torch.enabled = !torch.enabled; // Включить/выключить фонарик  
    }
}



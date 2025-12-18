using UnityEngine;


public class TorchController : MonoBehaviour
{
    private Light torch;
    [SerializeField] private Transform playerCamera;
    [SerializeField] private Vector3 positionOffset = new Vector3(0.3f, -0.2f, 0.5f);
    [SerializeField] private bool smoothFollow = true;
    [SerializeField] private float smoothSpeed = 10f;

    void Start()
    {
        torch = GetComponent<Light>();
        if (torch == null) torch = GetComponentInChildren<Light>();
        torch.enabled = false;

        if (playerCamera == null)
        {
            playerCamera = Camera.main?.transform;
            if (playerCamera == null)
            {
                Debug.LogError("Camera not found!");
            }
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            torch.enabled = !torch.enabled;
        }

        if (playerCamera != null && torch.enabled)
        {
            UpdateTorch();
        }
    }

    void UpdateTorch()
    {
        // Обновляем позицию
        Vector3 targetPosition = playerCamera.position +
                                playerCamera.right * positionOffset.x +
                                playerCamera.up * positionOffset.y +
                                playerCamera.forward * positionOffset.z;

        if (smoothFollow)
        {
            transform.position = Vector3.Lerp(transform.position, targetPosition, smoothSpeed * Time.deltaTime);
            transform.rotation = Quaternion.Slerp(transform.rotation, playerCamera.rotation, smoothSpeed * Time.deltaTime);
        }
        else
        {
            transform.position = targetPosition;
            transform.rotation = playerCamera.rotation;
        }
    }
}



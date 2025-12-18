using UnityEngine;

using UnityEngine;

public class DoorController : MonoBehaviour
{
    [Header("Door Animation Settings")]
    public float openAngle = 90f;
    public float animationTime = 1f;
    public bool isOpen = false;

    [Header("Interaction Settings")]
    public KeyCode interactionKey = KeyCode.E;

    [Header("Audio Settings")]
    public AudioSource doorAudioSource;
    public AudioClip openSound;
    public AudioClip closeSound;
    public float audioVolume = 1f;
    public bool playSoundOnStart = true;

    private Transform pivotTransform;
    private Quaternion closedRotation;
    private Quaternion openRotation;   
    private float animationProgress = 0f;
    private bool isAnimating = false;
    private bool soundPlayed = false;
    void Start()
    {
        pivotTransform = transform.parent;
        if (pivotTransform == null)
        {
            Debug.LogError("DoorController: Дверь должна быть дочерним объектом pivot точки!");
            return;
        }

        closedRotation = pivotTransform.localRotation;
        openRotation = closedRotation * Quaternion.Euler(0f, openAngle, 0f);

        if (doorAudioSource == null)
        {
            doorAudioSource = gameObject.AddComponent<AudioSource>();
            doorAudioSource.spatialBlend = 1f; // 3D звук
            doorAudioSource.volume = audioVolume;
        }
    }

    void Update()
    {
        if (isAnimating && pivotTransform != null)
        {
            animationProgress += Time.deltaTime / animationTime;


            if (playSoundOnStart && !soundPlayed && doorAudioSource != null)
            {
                PlayDoorSound();
                soundPlayed = true;
            }

            if (isOpen)
            {
                pivotTransform.localRotation = Quaternion.Slerp(closedRotation, openRotation, animationProgress);
            }
            else
            {
                pivotTransform.localRotation = Quaternion.Slerp(openRotation, closedRotation, animationProgress);
            }

            if (animationProgress >= 1f)
            {
                if (!playSoundOnStart && doorAudioSource != null)
                {
                    PlayDoorSound();
                }

                isAnimating = false;
                animationProgress = 0f;
                soundPlayed = false;

                pivotTransform.localRotation = isOpen ? openRotation : closedRotation;
            }
        }
    }

    public void Interact()
    {
        isOpen = !isOpen;
        isAnimating = true;
        animationProgress = 0f;
        soundPlayed = false;
    }

    private void PlayDoorSound()
    {
        if (doorAudioSource == null) return;

        AudioClip clipToPlay = isOpen ? openSound : closeSound;

        if (clipToPlay != null)
        {
            doorAudioSource.PlayOneShot(clipToPlay, audioVolume);
        }
    }

    public void OpenDoor(bool playSound = true)
    {
        if (!isOpen)
        {
            isOpen = true;
            isAnimating = true;
            animationProgress = 0f;
            soundPlayed = false;

            if (playSound && doorAudioSource != null && openSound != null)
            {
                doorAudioSource.PlayOneShot(openSound, audioVolume);
            }
        }
    }

    public void CloseDoor(bool playSound = true)
    {
        if (isOpen)
        {
            isOpen = false;
            isAnimating = true;
            animationProgress = 0f;
            soundPlayed = false;

            if (playSound && doorAudioSource != null && closeSound != null)
            {
                doorAudioSource.PlayOneShot(closeSound, audioVolume);
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.yellow;
        Transform targetTransform = pivotTransform != null ? pivotTransform : transform;
        Gizmos.matrix = targetTransform.localToWorldMatrix;
        Gizmos.DrawWireCube(Vector3.zero, Vector3.one);
    }

    public void ConfigureAudioSource(float volume = 1f, float spatialBlend = 1f, float minDistance = 1f, float maxDistance = 20f)
    {
        if (doorAudioSource != null)
        {
            doorAudioSource.volume = volume;
            doorAudioSource.spatialBlend = spatialBlend;
            doorAudioSource.minDistance = minDistance;
            doorAudioSource.maxDistance = maxDistance;
        }
    }
}
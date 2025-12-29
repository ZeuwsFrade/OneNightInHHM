using UnityEngine;

public abstract class LockedDoorController : DoorController
{
    [Header("Lock Settings")]
    public bool isLocked = true;
    public string lockId = "";
    public AudioClip unlockSound;
    public AudioClip lockedSound;

    [Header("UI Settings")]
    public string lockedMessage = "Дверь заперта";

    public virtual bool TryUnlock()
    {
        if (!isLocked) return true;

        if (CanUnlock())
        {
            isLocked = false;
            PlayUnlockSound();
            OnUnlocked();
            return true;
        }
        else
        {
            PlayLockedSound();
            ShowLockedMessage();
            return false;
        }
    }

    protected abstract bool CanUnlock();

    protected virtual void OnUnlocked()
    {
        Debug.Log($"Дверь {gameObject.name} открыта!");
    }

    private void PlayUnlockSound()
    {
        if (doorAudioSource != null && unlockSound != null)
        {
            doorAudioSource.PlayOneShot(unlockSound, audioVolume);
        }
    }

    private void PlayLockedSound()
    {
        if (doorAudioSource != null && lockedSound != null)
        {
            doorAudioSource.PlayOneShot(lockedSound, audioVolume);
        }
    }

    private void ShowLockedMessage()
    {
        // Потом переделать
        Debug.Log(lockedMessage);
    }

    public override void Interact()
    {
        if (isLocked)
        {
            TryUnlock();
        }
        else
        {
            base.Interact();
        }
    }
}
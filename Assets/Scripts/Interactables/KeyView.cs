using UnityEngine;

public class KeyView : MonoBehaviour, IInteractable
{
    public void Interact()
    {
        int currentKeys = GameService.Instance.GetPlayerController().KeysEquipped;

        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.KeyPickUp);
        
        currentKeys++; // invoke after updated key held
        EventService.Instance.OnKeyPickedUp.InvokeEvent(currentKeys);
        gameObject.SetActive(false);
    }
}

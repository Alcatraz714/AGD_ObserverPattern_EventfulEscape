using UnityEngine;
using System.Collections;

public class RotateObjectByEvent : MonoBehaviour
{
    [SerializeField] private int keysRequiredToTrigger;
    [SerializeField] private SoundType soundToPlay;
    [SerializeField] private Transform targetObject;
    [SerializeField] private float rotationAngle = 90f; // Rotation angle on Z-axis
    [SerializeField] private float rotationDuration = 1f; // Time taken to rotate

    private bool isRotating = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerView>() != null && GameService.Instance.GetPlayerController().KeysEquipped == keysRequiredToTrigger && !isRotating)
        {
            EventService.Instance.OnPaintingRotateEvent.InvokeEvent();
            StartCoroutine(RotateOverTime(targetObject, rotationAngle, rotationDuration));
            GameService.Instance.GetSoundView().PlaySoundEffects(soundToPlay);
            GetComponent<Collider>().enabled = false;
        }
    }

    private IEnumerator RotateOverTime(Transform obj, float angle, float duration)
    {
        isRotating = true;

        Quaternion startRotation = obj.rotation;
        Quaternion targetRotation = startRotation * Quaternion.Euler(0, 0, angle);
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            obj.rotation = Quaternion.Slerp(startRotation, targetRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        obj.rotation = targetRotation;
        isRotating = false;
    }
}

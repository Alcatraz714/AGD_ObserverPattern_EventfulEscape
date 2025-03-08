using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    [SerializeField] private SoundType soundType;
     [SerializeField] private float flickerDuration = 2f; // Total duration of flickering
    [SerializeField] private float flickerInterval = 0.1f; // Time between flickers
    private SwitchState currentState;
    private Coroutine flickerRoutine;

    private void OnEnable()
    {
        EventService.Instance.OnLightSwitchToggled.AddListener(onLightsToggled);
        EventService.Instance.OnLightsOffByGhostEvent.AddListener(onLightsOffByGhostEvent);
        EventService.Instance.OnLightsFlickerByGhostEvent.AddListener(StartFlickeringLights);
    }

    private void OnDisable()
    {
        EventService.Instance.OnLightSwitchToggled.RemoveListener(onLightsToggled);
        EventService.Instance.OnLightsOffByGhostEvent.RemoveListener(onLightsOffByGhostEvent);
        EventService.Instance.OnLightsFlickerByGhostEvent.RemoveListener(StartFlickeringLights);
    }

    private void Start()
    {
        currentState = SwitchState.Off;
    }
    public void Interact()
    {
        GameService.Instance.GetInstructionView().HideInstruction();
        EventService.Instance.OnLightSwitchToggled.InvokeEvent();
    }
    private void toggleLights()
    {
        bool lights = false;

        switch (currentState)
        {
            case SwitchState.On:
                currentState = SwitchState.Off;
                lights = false;
                break;
            case SwitchState.Off:
                currentState = SwitchState.On;
                lights = true;
                break;
            case SwitchState.Unresponsive:
                break;
        }
        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }

    private void setLights(bool lights)
    {
        if (lights)
            currentState = SwitchState.On;
        else
            currentState = SwitchState.Off;

        foreach (Light lightSource in lightsources)
        {
            lightSource.enabled = lights;
        }
    }
    private void onLightsOffByGhostEvent()
    {
        GameService.Instance.GetSoundView().PlaySoundEffects(soundType);
        setLights(false);
    }
    private void onLightsToggled()
    {
        toggleLights();
        GameService.Instance.GetSoundView().PlaySoundEffects(soundType);
    }

    // Flickering effect function
    public void StartFlickeringLights(float random)
    {
        if (flickerRoutine != null)
            StopCoroutine(flickerRoutine);

        flickerRoutine = StartCoroutine(FlickerLightsRoutine());
    }

    private IEnumerator FlickerLightsRoutine()
    {
        float elapsedTime = 0f;
        bool lightsOn = false;

        while (elapsedTime < flickerDuration)
        {
            lightsOn = !lightsOn;
            foreach (Light lightSource in lightsources)
            {
                lightSource.enabled = lightsOn;
            }

            GameService.Instance.GetSoundView().PlaySoundEffects(soundType); // Play flickering sound if needed
            elapsedTime += flickerInterval;
            yield return new WaitForSeconds(flickerInterval);
        }

        setLights(false); // Ensure lights are off at the end of flickering
    }
}

using System.Collections.Generic;
using UnityEngine;

public class LightSwitchView : MonoBehaviour, IInteractable
{
    [SerializeField] private List<Light> lightsources = new List<Light>();
    private SwitchState currentState;

    public delegate void LightSwitchDelegate(); // main signature for light switch delegate
    public static LightSwitchDelegate lightSwitch; // instance for the light switch

    private void OnEnable() => lightSwitch = OnLightSwitchToggled; // 1st method call in stack for light switch delegate

    private void Start() => currentState = SwitchState.Off;

    public void Interact()
    {
        //Todo - Implement Interaction
        lightSwitch.Invoke();
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

    // wrapper func for the interaction
    private void OnLightSwitchToggled()
    {
        toggleLights();
        GameService.Instance.GetInstructionView().HideInstruction();
        GameService.Instance.GetSoundView().PlaySoundEffects(SoundType.SwitchSound);
    }
}

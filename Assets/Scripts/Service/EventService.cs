using UnityEngine.UIElements;

public class EventService
{
    private static EventService instance;
    public static EventService Instance
    {
        get
        {
            if (instance == null)
            {
                instance = new EventService();
            }
            return instance;
        }
    }

    // Event Controllers - Inside Event controller - called by event service
    public EventController OnLightSwitchToggled {get; private set;}

    public EventService()
    {
        // init for all actions
        OnLightSwitchToggled = new EventController(); // creation of controller at first call for event service
    }
}

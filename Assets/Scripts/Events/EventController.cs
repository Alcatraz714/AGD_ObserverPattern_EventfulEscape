using System;

public class EventController
{
    public Action baseEvent;

    public void AddListner(Action listner) => baseEvent += listner;

    public void RemoveListner(Action listner) => baseEvent -= listner;

    public void InvokeEvent() =>  baseEvent?.Invoke(); //check for null first
}

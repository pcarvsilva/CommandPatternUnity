using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToggleCommandSubscriber : MonoBehaviour
{
    public Command command;

    public void SetSubscription(bool on)
    {
        if (on)
            CommandController.instance.Subscribe(command);
        else
            CommandController.instance.Unsubscribe(command);
    }
    
}

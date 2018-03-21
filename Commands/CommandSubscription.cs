using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System;

[Serializable]
public class CommandSubscription
{
    [Header("Subscribing")]
    public int priority;
    public EventTriggerType subscribe;
    public EventTriggerType unsubscribe;
    public bool unsubscribeOnClick = true;
    [Header("Cursor")]
    public Texture2D cursorTexture;
    public Vector2 hotSpot;
    public CursorMode cursorMode;
    [Header("Mask")]
    public LayerMask layerMask;
    [Header("Subscribing")]

    [HideInInspector]
    public ICommand command;
    [HideInInspector]
    public string commandType = null;



    public void Subscribe(GameObject owner)
    {
        command = (ICommand)Activator.CreateInstance(Type.GetType(commandType));

        EventTrigger trigger = owner.GetComponent<EventTrigger>();
        EventTrigger.Entry enter = new EventTrigger.Entry();
        enter.eventID = subscribe;
        enter.callback.AddListener((data) => { CommandController.instance.Subscribe(this); });
        trigger.triggers.Add(enter);


        EventTrigger.Entry exit = new EventTrigger.Entry();
        exit.eventID = unsubscribe;
        exit.callback.AddListener((data) => {
            CommandController.instance.Unsubscribe(this);
        });
        trigger.triggers.Add(exit);
    }

}
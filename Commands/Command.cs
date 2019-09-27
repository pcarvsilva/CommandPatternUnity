using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public abstract class Command : ScriptableObject
{
    public abstract void OnClick(GameObject agent, RaycastHit hit);

    public EventTriggerType subscribe;

    public EventTriggerType unsubscribe;
    public int priority;
    public Texture2D cursorTexture;
    public Vector2 hotSpot;
    public CursorMode cursorMode;
    public LayerMask layerMask;
    public bool unsubscribeOnClick;

    public void Subscribe(GameObject caller) {
        EventTrigger trigger = caller.GetComponent<EventTrigger>();

        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = subscribe;
        entry.callback.AddListener((data) => { CommandController.instance.Subscribe(this); });
        trigger.triggers.Add(entry);

        EventTrigger.Entry entry2 = new EventTrigger.Entry();
        entry2.eventID = unsubscribe;
        entry2.callback.AddListener((data) => { CommandController.instance.Unsubscribe(this); });
        trigger.triggers.Add(entry2);
    }
}

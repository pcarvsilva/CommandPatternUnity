using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

[RequireComponent(typeof(EventTrigger))]
public class CommandSubscriber : MonoBehaviour {

    public List<Command> subscriptions;

    private bool inside = false;

    // Use this for initialization
    void Awake() {
	    foreach(Command sub in subscriptions)
        {
            sub.Subscribe(gameObject);
        }
	}

    public void SelectSelf(bool condition)
    {
        if (condition)
        {
            CommandController.instance.selectAgent(gameObject);
        }
    }


    void OnMouseEnter()
    {
        inside = true;
        EventTrigger enter = GetComponent<EventTrigger>();
        enter.OnPointerEnter(null);
    }

    void OnMouseExit()
    {
        if (inside)
        {
            EventTrigger enter = GetComponent<EventTrigger>();
            enter.OnPointerExit(null);
        }
        inside = false;
    }

}

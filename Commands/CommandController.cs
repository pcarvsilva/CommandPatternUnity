using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CommandController : MonoBehaviour {

    public static CommandController instance = null;
    public List<CommandSubscription> commands = new List<CommandSubscription>();
    [HideInInspector]
    public GameObject agent;

    public Coroutine runningCommand = null;

    public void clearAllCommands()
    {
        commands = new List<CommandSubscription>();
    }

    public void Subscribe(CommandSubscription subscriber)
    {
        if (commands.Contains(subscriber)) return;
        int index = 0;
        for(index=0;index < commands.Count;index++)
        {
            if(subscriber.priority > commands[index].priority)
            {
                break;
            }
        }
        commands.Insert(index,subscriber);
        if (index == 0)
        {
            refreshCommandInterface();
        }
    }

    public void Unsubscribe(CommandSubscription subscriber)
    {
        bool refreshAfter = false;
        if (commands.Count >0 && subscriber == commands[0])
        {
            refreshAfter = true;
        }
        commands.Remove(subscriber);
        if (refreshAfter) refreshCommandInterface();
    }

    public void refreshCommandInterface()
    {
        if(commands.Count != 0)
            Cursor.SetCursor(commands[0].cursorTexture,commands[0].hotSpot,commands[0].cursorMode);
        else
            Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }

    void Awake()
    {
        instance = this;
        agent = null;
    }

    void Start()
    {
        clearAllCommands();
       // selectAgent(GameObject.FindWithTag("Player"));
    }

    public void selectAgent(GameObject selected)
    {
        if (agent != null)
            agent.GetComponent<EventTrigger>().OnDeselect(null);
        agent = selected;
        agent.GetComponent<EventTrigger>().OnSelect(null);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && commands.Count != 0 && runningCommand == null)
        {
            RaycastHit hit;
            CommandSubscription subscription = commands[0];
            ICommand command = subscription.command;
            LayerMask layerMask = commands[0].layerMask;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100,layerMask) && EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (hit.collider != null)
                {
                    command.OnClick(agent, hit);
                    if (subscription.unsubscribeOnClick)
                    {
                        Unsubscribe(subscription);
                    }
                }
            }
        }   
    }

}

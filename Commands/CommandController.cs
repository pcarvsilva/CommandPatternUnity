using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using System;

public class CommandController : MonoBehaviour {

    public static CommandController instance = null;
    public List<Command> commands = new List<Command>();
    [HideInInspector]
    public GameObject agent;

    public GameObject startingAgent;

    public Coroutine runningCommand = null;

    public void clearAllCommands()
    {
        commands = new List<Command>();
    }

    public void Subscribe(Command subscriber)
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

    public void Unsubscribe(Command subscriber)
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
        if(startingAgent != null)
            selectAgent(startingAgent);
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
            Command command = commands[0];
            LayerMask layerMask = command.layerMask;

            if (Physics.Raycast(Camera.main.ScreenPointToRay(Input.mousePosition), out hit, 100) &&
                EventSystem.current.IsPointerOverGameObject() == false)
            {
                if (hit.collider != null)
                {
                    command.OnClick(agent, hit);
                    if (command.unsubscribeOnClick)
                    {
                        Unsubscribe(command);
                    }
                }
            }
        }
    }
}

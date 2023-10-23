using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
using UnityEngine.EventSystems;

public class InteractionData : MonoBehaviour
{
    public string shownText;
    public string EventName;
    public bool showDebug = false;

    public bool IsSetMachine = false;
    public int MachineID = 0;

    public void ExecuteEvent()
    {
        ExecuteEventByName(EventName);
    }

    public void ExecuteEventByName(string name)
    {
        GameEventMessage.SendEvent(name);
    }

    private void OnMouseDown()
    {
        if (IsSetMachine)
            GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine = MachineID;

        if (showDebug)
            Debug.Log(gameObject.name + ": Executing Event: " + EventName);

        if(GameMaster.Instance.GetComponent<PlayerData>().playerTarget == this.gameObject && GameMaster.Instance.GetComponent<PlayerData>().CursorLocked) 
            ExecuteEvent();
    }

    public void SetEventName(string _eventName) {

        EventName = _eventName;
    }

}

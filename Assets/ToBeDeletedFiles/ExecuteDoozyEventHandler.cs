using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;

public class ExecuteDoozyEventHandler : MonoBehaviour
{
    public string EventName;

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
        if (PlayerObjectHolder.MissionTextObj!=null)
            PlayerObjectHolder.MissionTextObj.AddText(gameObject.name, "Executing Event: " + EventName);
        
        this.gameObject.SendMessage("ExecuteEvent");
        GameMaster.Instance.GetComponent<PlayerData>().CursorLocked = false;
    }
}

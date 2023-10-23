using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetMissionButton : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnMouseEnter()
    {
        //Debug.Log("Entered");
    }

    private void OnMouseExit()
    {
        //Debug.Log("Exited");
    }
    private void OnMouseDown()
    {
        PlayerObjectHolder.MissionTextObj.AddText(gameObject.name,"Got Mission 1!");
        //Set active mission in the player data
        //GameMaster.Instance.GetComponent<PlayerData>().ActiveMissionID = 1;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerObjectHolder : MonoBehaviour
{
    public ChatLogHandler missionTextObj;
    public static ChatLogHandler MissionTextObj;
    // Start is called before the first frame update
    void Start()
    {
        MissionTextObj = missionTextObj;
        MissionTextObj.AddText("System","Welcome to our game!");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

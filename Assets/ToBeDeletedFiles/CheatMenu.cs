using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheatMenu : MonoBehaviour
{
    /* Cheat List:
     * Numpad 0: Get New Mission List
     * Numpad 1: Increase Milling and Turning stages
     * 
     * */

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Keypad0))
        {
            this.GetComponent<MissionManager>().GetNewMissions();
        }else if (Input.GetKeyDown(KeyCode.Keypad1))
        {
            this.GetComponent<PlayerData>().MillingStage++;
            this.GetComponent<PlayerData>().TurningStage++;
        }

    }
}

using Doozy.Engine.UI.Input;

using UnityEngine;
using TMPro;
using System;
using Doozy.Engine;
using Doozy.Engine.UI;

public class MissionExpandableButton : MonoBehaviour
{


    public TextMeshProUGUI title,description;
    public ActiveMissionData mission;
    public MissionWindow MissionWindow;
    private int index;


    // Start is called before the first frame update
    void Start()
    {

    }

    public void Init(ActiveMissionData _mission, MissionWindow _missionWindow,int _indexInTheList)
    {
        
        mission = _mission;
        title.text = mission.Template.Name.ToString();
       // description.text = mission.Template.Description.ToString().Substring(0,30)+"......";
        MissionWindow = _missionWindow;
        index = _indexInTheList;
    }


    public void ShowMission()
    {
       
            MissionWindow.Init(mission);
       
    }

    

    public void TakeMission() {
        GameMaster.Instance.GetComponent<MissionManager>().SetActiveMission(index);

        //Colse the Mission Board After Taking the Mission
        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("HideAfterPopUp");

        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_Achievement("Καλή επιτυχία με την " + mission.Template.Name); //to palio itan mission.missionIndex+1



    }

  

    public void ShowPopUP()
    {
        string[] tit = title.text.Split(' ');
        
       //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Νέα Αποστολή", "Αποδοχή Αποστολής "+ tit[1] +"?", "Ναι", "Όχι", "", "");
       //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(TakeMission, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

   }


}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine;
public class Gcode_View : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI topTextView,bottomTextView;
    public TMP_InputField inputCodeView;
    public GameObject MissionView;
    private ActiveMissionData activeMission;
    private string missionID;
    private string lastMissionID = "";
    public GameObject ShowMessageOverlay;
    public GameObject CheckedOK,ButtonCheck;
    public int FirstTimeTutorialState_Index = 6;


    public string TopText{set { topTextView.text = value;}}
    public string BottomText{ set{ bottomTextView.text = value;}}
    public string InputText { get { return inputCodeView.text;}}

    /*
    private void Awake()
    {
        activeMission = new ActiveMissionData(false,-1);  
    }*/


    public void Init()
    {

        activeMission = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission;

        if (activeMission.MissionIndex >=0  && activeMission.Template != null)
        {
            ShowMessageOverlay.SetActive(false);
            missionID = activeMission.IsMilling.ToString() + activeMission.Template.ID;

            if (string.IsNullOrEmpty(lastMissionID) || !string.Equals(lastMissionID, missionID))
            {
                lastMissionID = activeMission.IsMilling.ToString() + activeMission.Template.ID;
                inputCodeView.text = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.WrittenCode;
                // inputCodeView.text = "";
                //  GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.WrittenCode = inputCodeView.text;
            }
            else
            {
                inputCodeView.text = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.WrittenCode;

            }


            MissionView.GetComponent<MissionWindow>().Init(activeMission);
            TopText = activeMission.Template.TopCode;
            BottomText = activeMission.Template.BottomCode;


        }
        else {

            ShowMessageOverlay.SetActive(true);
        }

     

    }


    public void OnHide() { 
            GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.WrittenCode = inputCodeView.text;
    }


    public void OnShow()
    {
        GcodeButtonChecked();
    }
    public void Check() { 
        GameMaster.Instance.GetComponent<MissionManager>().CheckSolution(inputCodeView.text);
        GcodeButtonChecked();
        }
    public void ImportSolution() {
      //  TextMeshProUGUI tmp = new TextMeshProUGUI();
     //   tmp.SetText(activeMission.Template.MiddleCode);

        this.inputCodeView.text = activeMission.Template.MiddleCode;

        GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.RewardExpPerMission = 0;
        GameMaster.Instance.GetComponent<PlayerData>().PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + "Mission View: Solve Mission was pressed. \n";

    }

    public void GcodeButtonChecked() {
        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.IsCodeChecked)
        {
            ButtonCheck.GetComponent<Button>().interactable = false;
            inputCodeView.interactable = false;
            CheckedOK.SetActive(true);
        }
        else {
            ButtonCheck.GetComponent<Button>().interactable = true;
            inputCodeView.interactable = true;
            CheckedOK.SetActive(false);
        }
    }


    //pop up test methods TEMPORARY

    public void ShowPopUP() {
        Debug.Log("Try to show popup");
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("MyTitle", "Do you realy want to exit", "Yes", "No", "", "");
        
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();
       




    }


    public void ActivateNextLevelStageTutorial()
    {
        if (GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState == -2) { return; }
        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.MissionIndex ==0  )
        {
            if (FirstTimeTutorialState_Index == GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState)
            {
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("Nextstage");
            }
        }


    }


    public void ShowPopUP_Solution()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Χρήση λύσης", "Είστε σίγουροι οτι θέλετε να συμπληρωθεί η λύση? Δεν θα λάβετε εμπειρία για αυτή την αποστολή.", "Ναι", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(ImportSolution, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }


}

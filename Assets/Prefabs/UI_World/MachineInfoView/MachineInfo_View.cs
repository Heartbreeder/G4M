using TMPro;
using UnityEngine;
using DG.Tweening;
using Doozy.Engine.Progress;
using Doozy.Engine.UI;

public class MachineInfo_View : MonoBehaviour
{
    public static MachineInfo_View _instance;
    [Header("UI fields")]
    public TextMeshProUGUI MachineName;
    public TextMeshProUGUI MissionName;
    public TextMeshProUGUI GcodeReady;
    public TextMeshProUGUI Progression;
    public TextMeshProUGUI IsComplete;
    [Header("Progress GM")]
    public Progressor progresson;
    public ProgressTargetTextMeshPro progressTargetText;

    private ActiveMissionData activeMission;
    private string messageForNotification;
    private bool cutCompleted = true;

    private void Awake() => _instance = this;
   
    void Start()
    {
        Init();
    }
    public void Init()
    {
        activeMission = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission;

        if (activeMission.MissionIndex < 0)
        {
            transform.GetComponentInChildren<UIView>().Hide(true);
            return;
        }
        else {
            transform.GetComponentInChildren<UIView>().Show(true); 
        }

        

        IsCompletedTrigger(activeMission.CompletedQuantity);


        MissionName.text = activeMission.Template.Name;
        MachineName.text = GameMaster.Instance.GetComponent<PlayerData>().GetMachineData(1)?.MachineName;
        //IsComplete.alpha = 0;

        GcodeReady.text = (activeMission.IsCodeChecked == true) ? "<color=green>ΝΑΙ</color>" : "<color=red>ΟΧΙ</color>";

        progressTargetText.Suffix = "/" + activeMission.RequestedQuantity.ToString();
        progresson.AnimateValue = false;
        progresson.SetMax(activeMission.RequestedQuantity);
        progresson.SetValue(activeMission.CompletedQuantity);
        progressTargetText.Multiplier = activeMission.RequestedQuantity;
        progresson.AnimateValue = true;






    }

    public void SetAnimation()
    {
        Invoke("Message", activeMission.Delay);

        var quantatyCutted = activeMission.CompletedQuantity + activeMission.ExecutionCarveQuantity;
        IsCompletedTrigger(quantatyCutted);

        progresson.AnimationDuration = activeMission.Delay;
        progresson.SetValue(activeMission.CompletedQuantity + activeMission.ExecutionCarveQuantity + 0.01f);
    }


    private void IsCompletedTrigger(int quantatyCutted)
    {
        IsComplete.gameObject.GetComponent<ProgressTargetAction>().ResetTrigger();
        

        IsComplete.gameObject.GetComponent<ProgressTargetAction>().TriggerValue = quantatyCutted;

        if (quantatyCutted == activeMission.RequestedQuantity)
        {
            //it finnishes
            messageForNotification = "<color=green>ΟΛΟΚΛΗΡΩΘΗΚΕ</color> \n <color=white><size=14>Η επόμενη αποστολή είναι διαθέσιμη </size></color> ";
            IsComplete.text = messageForNotification ;
            cutCompleted = true;

        } else if (quantatyCutted == 0)
        {
            messageForNotification = "<color=white>AΝΑΜΟΝΗ</color> \n <color=white><size=14>Φορτώστε στην μηχανή για να ξεκινήσετε</size></color> ";
            IsComplete.text = messageForNotification;
            cutCompleted = false;
        }

        else
        {
            int totalElapsed = (activeMission.RequestedQuantity - quantatyCutted);

            //not finished, and remains some other to cut
            messageForNotification = "<color=orange>ΟΛΟΚΛΗΡΩΘΗΚΕ ΜΕΡΙΚΩΣ</color>\n<color=white><size=18>Έχουν κοπεί <color=orange>"+ ((activeMission.RequestedQuantity -totalElapsed)) + "</color>/" + activeMission.RequestedQuantity + " τεμάχια." + "\nΑπομένει να κοπούν <color=orange>" + (activeMission.RequestedQuantity - quantatyCutted) + "</color> τεμάχια</size></color>";
            IsComplete.text = messageForNotification;
            cutCompleted = false;
        }

    }


    private void Message()
    {
        CheckStatusEventMessage();
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext("Η κατεργασία ολοκληρώθηκε", ChatboxCategories.General);
        
    }



    public string GetStatus() { //this i use in QuestManager To initialize status when starting the game

        var quantatyCutted = activeMission.CompletedQuantity + activeMission.ExecutionCarveQuantity;


        if (cutCompleted)
        {
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MachineCutFinished");
            return "";
        }
        else 
        {
            if (quantatyCutted == 0) {
                return null; // it means that mission as accepted but machine never used
            }
               
            else
            {
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MachineCutFinished", new GameObject(messageForNotification)); ;
                return messageForNotification;
            }
        }
        

    }
    public void CheckStatusEventMessage() {
        var quantatyCutted = activeMission.CompletedQuantity + activeMission.ExecutionCarveQuantity;


        if (cutCompleted)
        {
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MachineCutFinished");
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_SimplePopUp(messageForNotification);
        }
       
        else {

            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MachineCutFinished", new GameObject(messageForNotification));
            GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_SimplePopUp(messageForNotification);
        }



    }
}

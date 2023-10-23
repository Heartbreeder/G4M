using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;


public class QuestManager : MonoBehaviour
{
    public static QuestManager _instance;
    // Start is called before the first frame update
    [SerializeField] private QuestItem[] quests;
    private GameObject tabSign;

    [SerializeField] private CanvasGroup MissionCompleteUI;
    private bool ready = false;

    private void Awake() => _instance = this;
    
    void Start()
    {
        _instance.tabSign = GameObject.FindGameObjectWithTag("Tab");
        _instance.ShowNotification();
        _instance.MissionCompleteUI.DOFade(0f, .5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo).SetId("MissionComplete");
        _instance.tabSign.GetComponent<Image>().DOFade(0f, .5f).SetEase(Ease.InOutCubic).SetLoops(-1, LoopType.Yoyo).SetId("Tab");

        _instance.ResetQuestsOnNewMissionTake();
        ready = true;
        Invoke("LoadSavedQuests",0.5f);
    }
    
    public void ResetQuestsOnNewMissionTake() {
        foreach (QuestItem item in _instance.GetQuests())
        {
            item.ResetToggle();
        }

        
    }

    public QuestItem[] GetQuests() { return quests; }
    public QuestItem GetQuest(int index) { return quests[index]; }
    public void HideNotification()
    {

        if (_instance.tabSign != null) {
            _instance.tabSign.GetComponent<Image>().DOPause();
            _instance.tabSign.SetActive(false);
           
            }
    }

    public void ShowNotification()
    {
        if (_instance.tabSign != null) {
            _instance.tabSign.SetActive(true);
            _instance.tabSign.GetComponent<Image>().DOPlay();
        }
    }


    public void OnPlayerInventoryUpdated() {

        if (!ready) return;


        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission?.MissionIndex == -1) return;


        string materialAvaliability = GameMaster.Instance.GetComponent<MissionManager>().CheckMaterialAvailability();
        string toolAvaliability = GameMaster.Instance.GetComponent<MissionManager>().CheckToolAvailability();




        if (materialAvaliability != null)
        {
            if (_instance.GetQuest(1).questToggle.isOn==true) { _instance.GetQuest(1).ResetToggle(); }
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MaterialLoaded", new GameObject(materialAvaliability));
          
        }
        else {
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("MaterialLoaded");
             }

        if (toolAvaliability != null)
        {
            if (_instance.GetQuest(2).questToggle.isOn == true) { _instance.GetQuest(2).ResetToggle(); }
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("ToolLoaded", new GameObject(toolAvaliability));
            
        }
        else
        {
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("ToolLoaded");
        }

        ShowNotification();
    }
    

    public void LoadSavedQuests() {
        bool Gcode = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.IsCodeChecked;
        bool NewMissionGenerated  = GameMaster.Instance.GetComponent<PlayerData>().NextMissionList.Count == 0 ? true:false ;
        bool MissionAccepted = GameMaster.Instance.GetComponent<PlayerData>().ActiveMission?.MissionIndex !=-1? true : false;

      
        //Set QuestItem - Gcode
        if (GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState == -2 )
        {
            _instance.OnPlayerInventoryUpdated();

            if (Gcode && MissionAccepted)
            {
                _instance.quests[0].questToggle.isOn = true;
              
            }

            //check for tools
           

            //check if mission is completed an if yes then make quest[3].isOn true.
            string message = MachineInfo_View._instance.GetStatus();

            if (message != null)
            {
              
            }
            else {
                _instance.HideNotification();
            }
        }
        



       



        //Set Pointer
        if (Gcode && MissionAccepted)
        {
            PointerController._instance.Show_MachineAndShops();
        }

        if (!Gcode && MissionAccepted)
        {
            PointerController._instance.Show_GcodeAndTutorials();
        }

        if (!MissionAccepted)
        {
            PointerController._instance.Show_MissionBoard();
        }
    }
}

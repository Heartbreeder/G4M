using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Examples;
using System.Linq;
public class MissionBoard : MonoBehaviour
{

    public Transform TabContainer;
    private ToggleGroup toggleGroupInstance;
    public GameObject TogglePrefab;
    public MissionWindow missionWindow;
    private List<ActiveMissionData> missionList;
    public GameObject ShowMessage;
    public int FirstTimeTutorialState_Index = 5;

    void Start()
    {

       
    }

    public void ActivateNextLevelStageTutorial() {
        if (GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState == -2) { return; } //no other tutorials

        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.MissionIndex == 0)
        {
            if (FirstTimeTutorialState_Index == GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState)
            {
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("Nextstage");
            }
        }
        
        
    }
    public void Show() {
        ClearAllButtons();

        toggleGroupInstance = TabContainer.gameObject.GetComponent<ToggleGroup>();
        missionList = GameMaster.Instance.GetComponent<PlayerData>().NextMissionList;


        if (missionList.Count==0)
        {
            Debug.Log("MissionBoard -- No Missions In The ActiveMissionData List");
            ShowMessage.SetActive(true);
            return;
        }
        else {
            ShowMessage.SetActive(false);
        }



      for (int i = 0; i < missionList.Count; i++){
            if (!string.IsNullOrEmpty(missionList[i].Template.Name)) {
                 GameObject toggle = Instantiate(TogglePrefab, TabContainer);
                 toggle.GetComponent<MissionExpandableButton>().Init(missionList[i],missionWindow,i );
                if (i == 0) { toggle.GetComponent<Button>().onClick.Invoke(); }
            }
        }
       
        
    
    
    }

    public void ClearAllButtons()
    {
        foreach (Transform child in TabContainer.transform)
        {

            GameObject.Destroy(child.gameObject);
        }
    }


    public void GetNewMissionsRefresh() {
        GameMaster.Instance.GetComponent<MissionManager>().GetNewMissions();
        Show();

    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;


public class Tool_Spec_View : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI View_ToolName, View_PriceUnity,  View_ToolPrice, View_ToolDescription;
    [SerializeField]
    private TMP_Dropdown View_ToolDimensions;



    private ToolTemplate _tool;
    private string toolID;

    [SerializeField]
    private Image View_ToolImage;

    [SerializeField]
    private Slider slider;
    [SerializeField]
    private Animator ImageAnimator;
    // Init
    public GameObject OverlayMessage;
    public int FirstTimeTutorialState_Index = 8;


    public void Init(ToolTemplate tool)
    {

        _tool = tool;
       OverlayMessage.SetActive(false);
        
       View_ToolName.text = tool.ToolName;


       toolID = tool.ID;
        //Create a new option for the Dropdown menu which reads "Option 1" and add to messages List
        View_ToolDimensions.ClearOptions();
     
        List<string> options = new List<string>();
        for (int i = 0; i < tool.Dimensions.Length; i++)
        {
            
            string option = tool.Dimensions[i];
            options.Add(option);
        }
        
        View_ToolDimensions.AddOptions(options);

        if (tool.Dimensions.Length >= 1)
            View_ToolDimensions.value = 1;
        View_ToolDimensions.RefreshShownValue();
      


       View_ToolPrice.text = tool.Price.ToString();
       View_PriceUnity.text = tool.Price.ToString();
       View_ToolDescription.text = tool.Decription;
       View_ToolImage.sprite = tool.ItemImage;
        
       ImageAnimator.Play(tool.ID);




    }
    
    

    public void Buy() {

        GameMaster.Instance.GetComponent<PlayerData>().BuyTool(toolID, View_ToolDimensions.options[View_ToolDimensions.value].text);
        AddMessage("Αγορά: " + toolID + " - " + View_ToolDimensions.options[View_ToolDimensions.value].text );

    }


    public void ShowPopUP()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Αγορά Εργαλείου", "Θέλετε να αγοράσετε το εργαλείο " + _tool.ToolName + " ?", "Ναι", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Buy, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }
    public void AddMessage(string message)
    {
        GameMaster.Instance.GetComponent<PopUpMessagesSender>().Test_ChatBox(message);
    }
    public void ActivateNextLevelStageTutorial()
    {

        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.MissionIndex == 0)
        {
            if (FirstTimeTutorialState_Index == GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState)
            {
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("Nextstage");
            }
        }


    }
}
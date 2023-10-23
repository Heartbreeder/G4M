using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Material_Spec_View : MonoBehaviour
{
    private string materialID;
    private MaterialTemplate material;
    [SerializeField]
    private TextMeshProUGUI View_ItemMetalType,  View_ItemPrice, View_ItemDescription;
    [SerializeField]
    private TMP_Dropdown View_itemMetalDimensions;
    [SerializeField]
    private Image View_ItemImage;

    [SerializeField]
    private Slider slider;
    public GameObject OverlayMessage;
    public int FirstTimeTutorialState_Index = 7;

    // Init



    public void Init(MaterialTemplate mat)
    {

        OverlayMessage.SetActive(false);

        material = mat;
        View_ItemMetalType.text = mat.MetalType;


        View_itemMetalDimensions.ClearOptions();
        //Create a new option for the Dropdown menu which reads "Option 1" and add to messages List
        List<string> options = new List<string>();
        for (int i = 0; i < mat.Dimensions.Length; i++)
        {

            string option = mat.Dimensions[i];
            options.Add(option);

        }



        View_itemMetalDimensions.AddOptions(options);
        
        View_itemMetalDimensions.RefreshShownValue();


        View_ItemPrice.text = mat.Price.ToString();
        View_ItemDescription.text = mat.Decription;
        View_ItemImage.sprite = mat.ItemImage;
        materialID = mat.ID;
        slider.GetComponent<SliderValue>().SetValue();

    }

    public void Buy()
    {
        if (slider.value > 0) {
            Debug.Log("BuyFunction in Materia spec view");
            GameMaster.Instance.GetComponent<PlayerData>().BuyMaterial(materialID, View_itemMetalDimensions.options[View_itemMetalDimensions.value].text, (int) slider.value);
             
            AddMessage("Αγορά: "+materialID+" - "+ View_itemMetalDimensions.options[View_itemMetalDimensions.value].text + " "+ (int)slider.value +" Τεμάχια");

            if (materialID == GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.RequestedMaterialID && View_itemMetalDimensions.options[View_itemMetalDimensions.value].text== GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.RequestedMaterialDimensions) {
                Debug.Log("Enterdfor check in MaterialSpec for firsttime tutorial");
                ActivateNextLevelStageTutorial();
            }
           

        }
    }

    public void AddMessage(string message) {
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

    public void ShowPopUP()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Αγορά Υλικού", "Θέλετε να αγοράσετε " + slider.value + " κομμάτια ?", "Ναι", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Buy, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }
}

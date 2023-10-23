using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine.Progress;

public class InventoryItem_Tool : MonoBehaviour
{
    public InventoryToolData tool;

    public Image icon;
    public TextMeshProUGUI _name;
    public TextMeshProUGUI dimensions;
    public TextMeshProUGUI durability;
    public Progressor durabilityBar;
    public Toggle toggle;
    public Button LoadToMachineButton,RepairButton;
    
    


    public void Init(InventoryToolData _item, ToggleGroup toggleGroup,bool ActiveMachine)
    {


        if (_item != null) {

            tool = _item;
            _name.text = _item.Template.ToolName;
            icon.sprite = tool.Template.ItemImage;
            
            dimensions.text = tool.ToolDimensions;
            durability.text = "Ανθεκτηκότητα "+tool.ToolDurability.ToString()+"/"+ tool.Template.Durability.ToString();
            durabilityBar.SetMax(tool.Template.Durability);
            durabilityBar.InstantSetValue(tool.ToolDurability);
            toggle.group = toggleGroup;


            //Show Repair Button
            if (tool.ToolDurability < tool.Template.Durability) {RepairButton.gameObject.SetActive(true);}  else { RepairButton.gameObject.SetActive(false); }

            //Show Loadto machine Button
            LoadToMachineButton.gameObject.SetActive(ActiveMachine);
        }
    }



    public void LoadToMachine() {

        GameMaster.Instance.GetComponent<PlayerData>().TransferTool(tool.UniqueCode, GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine);
    }

    public void Repair()
    {

        GameMaster.Instance.GetComponent<PlayerData>().RepairTool(tool.UniqueCode);
        durabilityBar.InstantSetValue(tool.ToolDurability);
        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("UpdatePlayerInventory");
        

    }

    public void UnloadFromMachine() {
        GameMaster.Instance.GetComponent<PlayerData>().TransferTool(tool.UniqueCode, 0);
    }
    public void Sell()
    {
        GameMaster.Instance.GetComponent<PlayerData>().SellTool(tool.UniqueCode);
    }

   

    public void ShowPopUP()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Πώληση Εργαλείου", "Θέλετε να πουλήσετε το εργαλείο " + _name.text + " ?", "Ναί", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Sell, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }

    public void ShowPopUPRepair()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Επιδιόρθωση Εργαλείου", "Θέλετε να επιδιορθώσετε το εργαλείο " + _name.text + " ?", "Ναί", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Repair, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }
}

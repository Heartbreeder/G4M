using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine.Progress;

public class InventoryItem_Material : MonoBehaviour
{
    public InventoryMaterialData mat;

    public Image icon;
    public TextMeshProUGUI name;
    public TextMeshProUGUI dimensions;
    public TextMeshProUGUI quantaty;
    public Toggle toggle;
    public Slider slider;
    public Button LoadToMachineButton;



    public void Init(InventoryMaterialData _item, ToggleGroup toggleGroup, bool ActiveMachine)
    {
        if (_item != null)
        {

            mat = _item;
            icon.sprite = mat.Template.ItemImage;
            name.text = mat.Template.MetalType;
            dimensions.text = mat.MaterialDimensions;
            quantaty.text = mat.MaterialQuantity.ToString() ;
            slider.maxValue = mat.MaterialQuantity;
            toggle.group = toggleGroup;

            //Show SellButton
            LoadToMachineButton.gameObject.SetActive(ActiveMachine);
        }
    }

    public void LoadToMachine()
    {
        if (slider.value == 0) return;

        int activeMachine = GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine;
        GameMaster.Instance.GetComponent<PlayerData>().TransferMaterial(mat.ID,mat.MaterialDimensions, (int)slider.value ,0,activeMachine);
    }

    public void UnloadFromMachine()
    {
        if (slider.value == 0) return;
        int activeMachine = GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine;
        GameMaster.Instance.GetComponent<PlayerData>().TransferMaterial(mat.ID, mat.MaterialDimensions, (int)slider.value, activeMachine, 0);
    }

    public void Sell() {
        if (slider.value == 0) return;

        Debug.Log("Sell material on InventoryItem - Materials");
        GameMaster.Instance.GetComponent<PlayerData>().SellMaterial(mat.ID,mat.MaterialDimensions,(int) slider.value, 0);
    }



    public void ShowPopUP()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Πώληση Υλικού "+name.text, "Θέλετε να πουλήσετε " + slider.value +" κομμάτια"+ "?", "Ναί", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Sell, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }


}

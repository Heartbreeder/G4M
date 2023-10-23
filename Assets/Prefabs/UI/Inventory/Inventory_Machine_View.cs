using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;

public class Inventory_Machine_View : MonoBehaviour
{
    public TextMeshProUGUI view_name,view_maxtools,view_checktype,view_category;
    public Image machineIcon;


    private ActiveMachineData Machine;
   // private int activeMachine;
    public Transform ToolContainer, MaterialContaner;
    public GameObject InventoryItem_Material_prefab, InventoryItem_Tool_prefab;
    public GameObject GCodeExceputeButton;
    // Start is called before the first frame update
    private void Start()
    {

    }

    public void Show(int activeMachine, ActiveMachineData _Machine)
    {
        Machine = _Machine;
      //  activeMachine = GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine;
      /*  if (activeMachine <= 0)
        {
             GetComponent<UIView>().Hide();
             Debug.LogWarning("Inventory Machine View: Tried to open without ActiveMachine");
            return;
        }*/


        //  if (!GetComponent<UIView>().IsVisible) return;
        ClearAllContent(ToolContainer);
        ClearAllContent(MaterialContaner);




        
       
      //  Machine = GameMaster.Instance.GetComponent<PlayerData>().GetMachineData(activeMachine);
        SetUpMachineViews(Machine);



        //Read From DB and add to UI
        foreach (InventoryToolData item in GameMaster.Instance.GetComponent<PlayerData>().InventoryTools)
        {
            if (!string.IsNullOrEmpty(item.ID))
            {
                if (item.MachineID == activeMachine)
                {

                    GameObject tmp = Instantiate(InventoryItem_Tool_prefab, ToolContainer);
                    tmp.GetComponent<InventoryItem_Tool>().Init(item, ToolContainer.GetComponent<ToggleGroup>(),true);
                }
            }


        }


        //Read From DB and add to UI
        foreach (InventoryMaterialData item in GameMaster.Instance.GetComponent<PlayerData>().InventoryMaterials)
        {
            //Debug.Log("Tool in list "+item.ID);
            if (!string.IsNullOrEmpty(item.ID))
            {
                if (item.MachineID == activeMachine)
                {
                    GameObject tmp = Instantiate(InventoryItem_Material_prefab, MaterialContaner);
                    tmp.GetComponent<InventoryItem_Material>().Init(item, MaterialContaner.GetComponent<ToggleGroup>(),true);
                }
            }


        }


    }


    private void SetUpMachineViews(ActiveMachineData _machine)
    {


        Machine = _machine;

        view_name.text = Machine.Template.MachineName;
        view_maxtools.text = Machine.Template.MaxTools.ToString();
        view_checktype.text = (Machine.Template.IsHaas) ? ("Haas") : ("Siemens");
        view_category.text = (Machine.Template.IsMilling) ? ("Milling") : ("Turning");
        machineIcon.sprite = Machine.Template.MachineSprite;

        SetExceCuteGCodeButton();

    }

    private void SetExceCuteGCodeButton()
    {
        if (GameMaster.Instance.GetComponent<PlayerData>().ActiveMission.IsCodeChecked)
        {
            GCodeExceputeButton.GetComponent<Button>().interactable = true;
            GCodeExceputeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Εκτέλεση Κώδικα";
        }
        else {
            GCodeExceputeButton.GetComponent<Button>().interactable = false;
            GCodeExceputeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Δεν υπάρχει Κώδικας για Εκτέλεση";
        }
    }

    public void ClearAllContent(Transform Container)
    {
        foreach (Transform child in Container.transform)
        {
                GameObject.Destroy(child.gameObject);
        }
    }


    public void ExecuteButtonLogic()
    {
        GameMaster.Instance.GetComponent<MissionManager>().ExecuteCode(GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine);
        Doozy.Engine.UI.Input.BackButton.Instance.Execute();



    }



}
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;


public class Inventory_View : MonoBehaviour
{
    //UI Componets

    public Transform ToolContainer, MaterialContaner;
    public GameObject InventoryItem_Material_prefab, InventoryItem_Tool_prefab;
    public GameObject MachineUI;



    private bool isAnyMachineActive = false;
    private int activeMachine = -1;


    private void Start()
    {
      
        
    }


    public void Show() {


        activeMachine = GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine;
        

        //  if (!GetComponent<UIView>().IsVisible) return;
        if (activeMachine > 0)
        {
            Debug.Log("-------------- active machine number: " + activeMachine.ToString());
           ShowMachine(activeMachine, GameMaster.Instance.GetComponent<PlayerData>().GetMachineData(activeMachine));
       
        }
        else {
            HideMachine();
        }
        


        ClearAllContent(ToolContainer);
        ClearAllContent(MaterialContaner);

       
        //Read From DB and add to UI
        foreach (InventoryToolData item in GameMaster.Instance.GetComponent<PlayerData>().InventoryTools)
            {
            //Debug.Log("Tool in list "+item.ID);
            if (!string.IsNullOrEmpty(item.ID) )
            {
                if (item.MachineID==0)
                {
   
                GameObject tmp = Instantiate(InventoryItem_Tool_prefab, ToolContainer);
                
                tmp.GetComponent<InventoryItem_Tool>().Init(item, ToolContainer.GetComponent<ToggleGroup>(), isAnyMachineActive);
                }
            }


        }


        //Read From DB and add to UI
        foreach (InventoryMaterialData item in GameMaster.Instance.GetComponent<PlayerData>().InventoryMaterials)
        {
            //Debug.Log("Tool in list "+item.ID);
            if (!string.IsNullOrEmpty(item.ID))
            {
                if (item.MachineID == 0)
                {
                    GameObject tmp = Instantiate(InventoryItem_Material_prefab, MaterialContaner);
                    tmp.GetComponent<InventoryItem_Material>().Init(item, MaterialContaner.GetComponent<ToggleGroup>(), isAnyMachineActive);
                }
            }


        }


    }


    public void ClearAllContent(Transform Container)
    {
        foreach (Transform child in Container.transform)
        {

            GameObject.Destroy(child.gameObject);
        }
    }



    private void ShowMachine(int index, ActiveMachineData machine) {
        isAnyMachineActive = true;
        MachineUI.SetActive(true);
        MachineUI.GetComponent<Inventory_Machine_View>().Show(index, machine);
    }

    private void HideMachine()
    {
        GameMaster.Instance.GetComponent<PlayerData>().ActiveMachine = -1;
        isAnyMachineActive = false;
        MachineUI.SetActive(false);
        
    }
    private void OnDisable()
    {
        HideMachine();
    }

   
    public void OnHideUI() {
        HideMachine();
    }
}

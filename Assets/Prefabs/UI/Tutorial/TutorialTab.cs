using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Linq;
using Doozy.Engine.UI;

public class TutorialTab : MonoBehaviour
{


    
    public GameObject TabContainer;
    private ToggleGroup toggleGroupInstance;
    public GameObject TogglePrefab;

    public List<TutorialTemplate> Tutolial_List;
    public List<Toggle> TogglesButton_List;

    //Tutorial RightPanel Container

    public GameObject ExpandableItemPrefab;
    public GameObject RightPanelContainer;


    int stage;
    TutorialTemplate[] tutorials;
    bool[] isRead;
    bool isMillingStage;

    public Toggle currentSelection {
        get { return toggleGroupInstance.ActiveToggles().FirstOrDefault(); }
    
    }
    public void SelectToggle(int id) {

        Toggle toggle = TabContainer.transform.GetChild(id).GetComponent<Toggle>();
        toggle.isOn = true;
    }


    void Start()
    {

       

        Tutolial_List = new List<TutorialTemplate>();
        TogglesButton_List = new List<Toggle>();
       

        //GameMaster.Instance.GetComponent<PlayerData>().MillingStage = 0;
        Show(true);


    }

    public void Show(bool isMilling)
    {

        toggleGroupInstance = TabContainer.GetComponent<ToggleGroup>();

        Tutolial_List.Clear();
        TogglesButton_List.Clear();
        ClearAllButtons();
       
        



        if (isMilling)
        {
            isMillingStage = true;
            stage = GameMaster.Instance.GetComponent<PlayerData>().MillingStage;
            tutorials = MachineShopData.MillingTutorials.Array;
            isRead = GameMaster.Instance.GetComponent<PlayerData>().MillingTutorialsRead;
        }
        else {
            isMillingStage = false;
            stage = GameMaster.Instance.GetComponent<PlayerData>().TurningStage;
            tutorials = MachineShopData.TurningTutorials.Array;
            isRead = GameMaster.Instance.GetComponent<PlayerData>().TurningTutorialsRead;
        }

        for (int i = 0; i < tutorials.Length; i++)
        {
            if (tutorials[i].Stage <= stage) {
            TutorialTemplate tutorial = tutorials[i];
            GameObject toggle = Instantiate(TogglePrefab, TabContainer.transform);
            
                //set sibling
                toggle.transform.SetSiblingIndex(i);
            //create Toggle for the view
            toggle.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = tutorial.Name;
            toggle.name = i.ToString();
            //highlight the tabs that are new


            
            toggle.transform.GetChild(2).GetComponent<Image>().enabled = !isRead[i];

            toggle.GetComponent<Toggle>().group = toggleGroupInstance;
            toggleGroupInstance.RegisterToggle(toggle.GetComponent<Toggle>());
            toggle.GetComponent<Toggle>().onValueChanged.AddListener((value) => ShowTutorial(value));

            //Add To list
            TogglesButton_List.Add(toggle.GetComponent<Toggle>());
            Tutolial_List.Add(tutorial);
            }
        }


            SelectToggle(0);
       
       
        
    }


    public void Check_If_All_are_Read() {
        
        bool isEnabled = false;

        for (int i = 0; i < 5; i++)
        {
            isEnabled = isEnabled || !GameMaster.Instance.GetComponent<PlayerData>().MillingTutorialsRead[i];
        }



    }
  

    public void ShowTutorial(bool isOn) {

        if (isOn) {
            
            
            
            //int.TryParse(currentSelection.transform.name, out _index);

            int _index =  currentSelection.transform.GetSiblingIndex();


            Debug.Log(_index);


            InitTutorialWindows(Tutolial_List[_index]);

            if (isMillingStage)
            {
                GameMaster.Instance.GetComponent<PlayerData>().MillingTutorialsRead[_index] = true;
              
               
            }
            else {
                GameMaster.Instance.GetComponent<PlayerData>().TurningTutorialsRead[_index] = true;
              
            }
            currentSelection.transform.GetChild(2).GetComponent<Image>().enabled = false;

            
        }

    }

    public void InitTutorialWindows(TutorialTemplate _tutorial) {
        ClearAllTutorialContent();

        int counter = 1;
        foreach (TutorialPage tutorialPage in _tutorial.TutorialPageArray) {


            GameObject ExpandableObject = Instantiate(ExpandableItemPrefab, RightPanelContainer.transform);
            ExpandableObject.GetComponent<ExpandableObjectForTutolial>().title.text = tutorialPage.PageTitle + " - "+(counter++).ToString()+"/" + _tutorial.TutorialPageArray.Length.ToString() ;
            ExpandableObject.GetComponent<ExpandableObjectForTutolial>().TutorialImage.sprite = tutorialPage.PageImage;
            ExpandableObject.GetComponent<ExpandableObjectForTutolial>().toggle.group = RightPanelContainer.GetComponent<ToggleGroup>();


        }
    
    
    
    }



    public void ClearAllTutorialContent()
    {
        foreach (Transform child in RightPanelContainer.transform)
        {

            Destroy(child.gameObject);
        }
    }
    // Upd
    public void ClearAllButtons() {
       
        foreach (Transform child in TabContainer.transform)
        {
            Destroy(child.gameObject);
        }

    }
    
    public void WriteOnPlayerLog(string line)
    {
        GameMaster.Instance.GetComponent<PlayerData>().PlayerLog += "[" + GameMaster.Instance.GetComponent<PlayerData>().TimeMemory + "]" + line + "\n";
    }

}


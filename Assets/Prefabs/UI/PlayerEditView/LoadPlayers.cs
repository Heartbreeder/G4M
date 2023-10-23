using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;
using Doozy.Examples;


public class LoadPlayers : MonoBehaviour
{

    public Transform PlayerProfileContainer;
    public GameObject  PlayerProfileItem_Prefab;
    public string CreateProfile_PopupName = "CreateProfilePopup";

    private void Start()
    {
       
       
    }



    public void Show()
    {
         

        ClearAllContent(PlayerProfileContainer);

        GameMaster.Instance.GetComponent<PlayerData>().UpdateProfileNames();
        //Read From DB and add to UI
        if (GameMaster.Instance.GetComponent<PlayerData>().ProfileNames.Count == 0) {

            ShowCreatePlayer_PopUp();


            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("ProfileEmpty");
            return;
        }


        foreach (string item in GameMaster.Instance.GetComponent<PlayerData>().ProfileNames)
        {                              
                    GameObject tmp = Instantiate(PlayerProfileItem_Prefab, PlayerProfileContainer);
                    tmp.GetComponent<PlayerProfileItem>().Init(item, PlayerProfileContainer.GetComponent<ToggleGroup>());
        }





        Invoke("SelectActivePlayer",0);

    }


    public void SelectActivePlayer() {
        string ActivePlayer = GameMaster.Instance.GetComponent<PlayerData>().ProfileName;
        foreach (Toggle item in PlayerProfileContainer.GetComponentsInChildren<Toggle>())
        {
            
            if (string.Equals(ActivePlayer, item.gameObject.name)) {
                Debug.Log(item.gameObject.name);
                item.isOn = true;
                item.group.NotifyToggleOn(item);
            }
        }
    }


    public void DeleteProfile() {

        ToggleGroup toggleGroup = PlayerProfileContainer.gameObject.GetComponent<ToggleGroup>();

        foreach (var toggle in toggleGroup.ActiveToggles())
        {

            if (toggle.isOn)
            {
               

                GameMaster.Instance.GetComponent<PlayerData>().DeleteProfile(toggle.gameObject.name);

                break;
            }
           
        }

        Show();

    }




    public string GetSelectedProfileName() {

        ToggleGroup toggleGroup = PlayerProfileContainer.gameObject.GetComponent<ToggleGroup>();

        foreach (var toggle in toggleGroup.ActiveToggles())
        {

            if (toggle.isOn)
            {


                return toggle.GetComponent<PlayerProfileItem>().name.text;
               
            }

        }

        return "Nothing to show";
    }




   
    public void LoadProfile()
    {

        ToggleGroup toggleGroup = PlayerProfileContainer.gameObject.GetComponent<ToggleGroup>();

        foreach (var toggle in toggleGroup.ActiveToggles())
        {

            if (toggle.isOn)
            {


                GameMaster.Instance.GetComponent<PlayerData>().LoadPlayer(toggle.gameObject.name);

                break;
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



    public void ShowCreatePlayer_PopUp()
    {

        // GameMaster.Instance.GetComponent<PopupConfirm>().PopupName = CreateProfile_PopupName;
        this.gameObject.GetComponent<PopupConfirm>().SetupPopup("Δεν υπάρχουν χρήστες", "Θέλετε να δημιουργήσετε νέο προφίλ?", "Δημιουργία", "Πίσω", "", "");
        this.gameObject.GetComponent<PopupConfirm>().SetActions(null, null);
        this.gameObject.GetComponent<PopupConfirm>().ShowPopup(CreateProfile_PopupName);

    }



    public void ShowCreatePlayer()
    {

        // GameMaster.Instance.GetComponent<PopupConfirm>().PopupName = CreateProfile_PopupName;
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Δημιουργία προφίλ", "Θέλετε να δημιουργήσετε νέο προφίλ?", "Δημιουργία", "Πίσω", "", "");
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(ShowCreatePlayer_PopUp, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup("SimplePopupYesNo");

    }


    public void ShowPopUP_Delete()
    {

        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Διαγραφή Προφίλ", "Θέλετε να διαγράψετε το Προφίλ:  " + GetSelectedProfileName() + " ?", "Ναί", "Όχι", "", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(DeleteProfile, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }



    public void ShowPopUP_Profile()
    {
        LoadProfile();
        UIPopup m_popup = UIPopup.GetPopup("ProfileViewPopUP");
        if(m_popup!=null) m_popup.Show();

    }
}

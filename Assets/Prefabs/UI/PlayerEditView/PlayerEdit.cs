using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Doozy.Engine.UI;
using TMPro;


public class PlayerEdit : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private Transform AvatarContainer;
    [SerializeField]
    private TMP_InputField inputAvatarName;

    [SerializeField]
    private ToggleGroup toggleGroup;
    public Toggle toggle_Milling, toggle_Haas;

    public string OnSuccesSendEvent = "ProfileCreated";
    private bool isMilling, isHass;

    void Start()
    {
        Init();
    }

    public void Init()    {



        int i = 0;
        foreach (Image avatar in AvatarContainer.GetComponentsInChildren<Image>())
        {
            if (avatar.gameObject.transform.parent.name == AvatarContainer.name) { 
                avatar.name = i.ToString();
                avatar.sprite = MachineShopData.Avatars.Array[i++];
            }
        }

    }


    public void LoadUser(string _name) {

        GameMaster.Instance.GetComponent<PlayerData>().LoadPlayer(_name);
        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName(OnSuccesSendEvent);
    }


    public void CreateNewPlayer() {

       

        int AvatarIndex = 0;
        string AvatarName;



        

        

        foreach (var toggle in toggleGroup.ActiveToggles())
        {
            
            if (toggle.isOn)
            {
                AvatarIndex = int.Parse(toggle.gameObject.name);
              
                break;
            }
           
        }



        if (!string.IsNullOrEmpty(inputAvatarName.text))
        {
            AvatarName = inputAvatarName.text;

            if (string.IsNullOrEmpty(GameMaster.Instance.GetComponent<PlayerData>().ProfileName))
            {


                GameMaster.Instance.GetComponent<PlayerData>().SetNewPlayer(AvatarName, AvatarIndex,toggle_Milling.isOn,toggle_Haas.isOn);
                LoadUser(AvatarName);
            }
            else {
                GameMaster.Instance.GetComponent<PlayerData>().SetNewPlayer(AvatarName, AvatarIndex, toggle_Milling.isOn, toggle_Haas.isOn);
                LoadUser(AvatarName); //load after creation
            }

        }
        else {
            Debug.Log("Name is nool");
            CreateNewPlayer();
        }


       
       

    }
}

using DG.Tweening;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.Video;
using Doozy.Examples;
using Doozy.Engine.Soundy;

public class MissionWindow : MonoBehaviour
{

    public GameObject ExpandableWindowPrefab;

    [SerializeField]
    private Transform Container;

    [SerializeField]
    private GameObject _popUpScripts;

    public GameObject videoButton;
    private ActiveMissionData mission;
    public GameObject bluePrintButton;


    [Header("UI Reference")]
    //UI reference
    public Image _pathImage;
    public TextMeshProUGUI _title,_staticDescription, _dinamicDescription, _dinamicRequirements;
  
    

  
    //Mission Data
    private VideoClip _clip;
    private Sprite _mechanicalImage;

    //for popups
    private string title,message;
    void Start()
    {
       

        
    }




    public void OnShow() {
        if(GameMaster.Instance.GetComponent<PlayerData>().ActiveMission!=null)
         Init(GameMaster.Instance.GetComponent<PlayerData>().ActiveMission);
    }

    // Update is called once per frame
    public void Init(ActiveMissionData mission) 
    {


        _title.text = mission.Template.Name;
         title = mission.Template.Name;

        _staticDescription.text = mission.Template.Description;
        _dinamicDescription.text = mission.Template.MissionDetails;
        _dinamicRequirements.text = mission.Template.MissionDetails +"\n"+mission.AdditionalDescription;

        _clip = mission.Template.CarvingVideo;
        

        _pathImage.sprite = mission.Template.CarvingPathBlueprint;


        if (mission.Template.MechanicalBlueprint != null)
        {
            bluePrintButton.SetActive(true);
            _mechanicalImage = mission.Template.MechanicalBlueprint;
        }
        else {
            bluePrintButton.SetActive(false);
        }
        

        if (mission.IsCodeChecked) {
            videoButton.SetActive(true);
        }
        else {
            videoButton.SetActive(false);
        }
    }





    public void ShowBluePrint() {

        _popUpScripts.GetComponent<PopupPhoto>().SetupPopup(title, "message", _mechanicalImage,null);
        _popUpScripts.GetComponent<PopupPhoto>().ShowPopup();
    }
    public void ShowVideo() {
        _popUpScripts.GetComponent<PopupVideo>().SetupPopup(title, "message", _clip, null);
        _popUpScripts.GetComponent<PopupVideo>().ShowPopup();
    }

    public void ShowZoomedPhoto()
    {

        _popUpScripts.GetComponent<PopupPhoto>().SetupPopup(title, "message", _pathImage.sprite, null);
        _popUpScripts.GetComponent<PopupPhoto>().ShowPopup();
    }


    public void ClearAllContent()
    {
        foreach (Transform child in Container.transform)
        {

            GameObject.Destroy(child.gameObject);
        }
    }


    public void ShowVideoButton()
    {

        videoButton.SetActive(true);
        videoButton.transform.DOScale(1, 0.5f).SetEase(Ease.OutElastic).OnStart(PlaySound);
       
    }

    private void PlaySound()
    {
        SoundyManager.Play("SFX", "Notification");

    }



}

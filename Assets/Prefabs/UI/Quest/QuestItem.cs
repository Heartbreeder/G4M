using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine;
using DG.Tweening;
public class QuestItem : MonoBehaviour
{
    [SerializeField] private TMP_Text questTextUI;
    [SerializeField] private TMP_Text questWorningTextUI;

    [SerializeField] public Toggle questToggle;
    public string defaultText;
    public string eventNameListen;
    public string NotificationTag = "";
    public string OnButtonPressedEvent = "default";




    // Start is called before the first frame update
    public void Start()
    {
             
        InitQuest();
       
    }

    public void OnButtonPressedEventSend() {

        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName(OnButtonPressedEvent);
    }

    private void OnValidate()
    {
        questTextUI.text = defaultText;
    }
    public void InitQuest()
    {
        questTextUI.text = defaultText;
        questTextUI.fontStyle = FontStyles.Normal;
        questTextUI.color = Color.white;


        //worning text reset
        questWorningTextUI.text = "";
        questWorningTextUI.DOKill(); //kill the tweeining animation

        questToggle.isOn = false;
        questToggle.interactable = false;

        
    }

    public void QuestComplete(bool onComplete = true ) {
        if (onComplete)
        {
            questTextUI.text = defaultText;
            questTextUI.fontStyle = FontStyles.Strikethrough;
            questTextUI.color = Color.gray;

            //worning text reset
            questWorningTextUI.text = "";
            questWorningTextUI.DOKill(); //kill the tweeining animation

            

            questToggle.isOn = true;
            questToggle.interactable = false;

        }
        else {
            InitQuest();
        }


    }
    public void SetText(string text)
    {
        questWorningTextUI.text = "<color=orange>" + text;
        questWorningTextUI.DOFade(0.5f, 1f).SetLoops(-1).SetEase(Ease.InOutSine);
        
       

    }

    private void SetNotificationOn() {
        QuestManager._instance.ShowNotification();
     
    }

    public void ResetToggle()
    {
        questWorningTextUI.text = "";
        questWorningTextUI.DOKill();
        questToggle.isOn = false;
       

    }

    private void OnEnable() {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable() {
        Message.RemoveListener<GameEventMessage>(OnMessage);

    }


    private void OnMessage(GameEventMessage message)

    { 
      if (message == null) return;
      if (!message.EventName.Equals(eventNameListen)) return;

       
      if (message.Source == null) {
            SetNotificationOn();
            questToggle.isOn=true;
            return;
      } // Toggle to true when the no Sourse  - > QuestCompletee
              
      SetText(message.Source.name);
      Destroy(message.Source);
      SetNotificationOn();

    }



}

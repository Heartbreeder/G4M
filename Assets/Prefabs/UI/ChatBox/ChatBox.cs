using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Doozy.Engine.UI;
using UnityEngine.UI;

public class ChatBox : MonoBehaviour
{
    // Start is called before the first frame update
    private Queue<GameObject> _PopUpsStack;
    public GameObject Dialog_Prefab;
    public RectTransform PopUpContainer;
    public int StackSize = 5;
    public bool toogle_showChatBox = true;
    public RectTransform key, keyHiden;
    private float lastClick;
    public float SpamDelay = 1f;
    //public ChatboxPopUpManager pop;



    void Start()
    {
        lastClick = Time.time + 3;
        _PopUpsStack = new Queue<GameObject>();
        ShowChatBox();


    }


    public void Init()
    {
        LogText log = GameMaster.Instance.GetComponent<ChatLogCentral>().AllMessages.Peek();
            if (log.category == ChatboxCategories.ChatBox) {
                PushMessage(log);
            }
       
    }

    private GameObject CreatePopUpMessage( LogText _message) {

        GameObject messageUI = Instantiate(Dialog_Prefab, PopUpContainer);

        messageUI.GetComponent<LogTextScript>().Init(_message,true);
        

        return messageUI;
        
    }
    private void PushMessage(LogText _message) {

        GameObject message = CreatePopUpMessage(_message);

       
        SetTransparency();
        _PopUpsStack.Enqueue(message);

       if (_PopUpsStack.Count > StackSize) 
        {
           Destroy(_PopUpsStack.Dequeue()); 
        }

       
    }



    private void SetTransparency() {
        foreach (GameObject item  in _PopUpsStack)
        {
            item.GetComponent<CanvasGroup>().alpha /=2f;
        }
        
    }




    public void toggleChat() {
        if (toogle_showChatBox)
        {
            HideChatBox();
        }
        else {
            ShowChatBox();
        }
    }

    public void HideChatBox() {
        ShowKey();
        toogle_showChatBox = false;


        PopUpContainer.DOAnchorPos(new Vector2(-250,0), 0.25f).SetEase(Ease.Linear);
        PopUpContainer.GetComponent<CanvasGroup>().DOFade(0, 1);
     
    }
    public void ShowChatBox()
    {
        HideKey();
        toogle_showChatBox = true;
        PopUpContainer.DOAnchorPos(new Vector2(0, 0), 0.25f).SetEase(Ease.Linear);
        PopUpContainer.GetComponent<CanvasGroup>().DOFade(1, .5f);
    }

    public void ShowKey()
    {
        if (lastClick > (Time.time - SpamDelay)) return;
        
        lastClick = Time.time;


        key.GetComponent<CanvasGroup>().DOFade(1, .5f);
        keyHiden.GetComponent<CanvasGroup>().DOFade(0, 0.25f);
    }
    public void HideKey()
    {
        if (lastClick > (Time.time - SpamDelay)) return;
           lastClick = Time.time;

        keyHiden.GetComponent<CanvasGroup>().DOFade(1, .5f);

        key.GetComponent<CanvasGroup>().DOFade(0, 0.25f);

     
    }

}

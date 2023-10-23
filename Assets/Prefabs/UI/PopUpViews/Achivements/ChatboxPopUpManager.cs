using System;
using System.Collections.Generic;
using Doozy.Engine.UI;
using UnityEngine;


public class ChatboxPopUpManager : MonoBehaviour
{
    [Header("Popup Settings")]
    public string PopupName = "AchievementPopup";
    public string PopupNameSimpleBACK = "PopapBack";
    public string PopupNameSimpleFULL = "PopapFULL";
    private string PopupNameSimple = "PopUpOKSimpleV2";

    [Header("MessageType")]
    public List<MessageType> _MessageType = new List<MessageType>();


    private UIPopup m_popup;


    public void ShowPopUp(string message, ChatboxCategories _category = ChatboxCategories.General, int messageTypeID = 0)
    {
        //make sure the achievement we want to show has been defined in the Achievements list
        //the achievementId is actually the index of the achievement as it has been defined in the list
        if (_MessageType == null || messageTypeID < 0 || messageTypeID > _MessageType.Count - 1) return;

        //get the achievement from the list
        MessageType messageType = _MessageType[messageTypeID];

        //make sure we got an achievement and that the entry was not null
        if (messageType == null) return;

        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
        m_popup = UIPopupManager.GetPopup(PopupName);

        //make sure that a popup clone was actually created
        if (m_popup == null)
            return;

        //set the achievement icon
        m_popup.Data.SetImagesSprites(messageType.Icon);
        //set the achievement title and message
        m_popup.Data.SetLabelsTexts(messageType.Category, message);

        //show the popup
        UIPopupManager.ShowPopup(m_popup, m_popup.AddToPopupQueue, false);
    }

    public void PopupNameSimple_OK(string message)
    {
        //make sure the achievement we want to show has been defined in the Achievements list
        //the achievementId is actually the index of the achievement as it has been defined in the list
       



        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
        m_popup = UIPopupManager.GetPopup(PopupNameSimpleFULL);

        //make sure that a popup clone was actually created
        if (m_popup == null)
            return;

        
        m_popup.Data.SetLabelsTexts(message);

        //show the popup
        UIPopupManager.ShowPopup(m_popup, m_popup.AddToPopupQueue, false);
    }


    public void PopupNameSimple_OK_ver2(string message)
    {
        //make sure the achievement we want to show has been defined in the Achievements list
        //the achievementId is actually the index of the achievement as it has been defined in the list




        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
        m_popup = UIPopupManager.GetPopup(PopupNameSimple);

        //make sure that a popup clone was actually created
        if (m_popup == null)
            return;


        m_popup.Data.SetLabelsTexts(message);

        //show the popup
        UIPopupManager.ShowPopup(m_popup, m_popup.AddToPopupQueue, false);
    }

    public void ClearPopupQueue()
    {
        UIPopupManager.ClearQueue();
    }
}

[Serializable]
public class MessageType
{
    public string Category;
    public Sprite Icon;
}

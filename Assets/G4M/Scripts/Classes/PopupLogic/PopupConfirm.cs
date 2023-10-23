using Doozy.Engine.UI;
using UnityEngine;
using System;


    public class PopupConfirm : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "SimplePopupYesNo";

        public string Title = "Example Title";
        public string Message = "This is an example message for this UIPopup";

        [Space(10)]
        public string LabelButtonOne = "Yes";
        public string LabelButtonTwo = "No";
        public bool HideOnButtonOne = true;
        public bool HideOnButtonTwo = true;

        public string EventOnYes;
        public string EventOnNo;

        private Action callback1, callback2;
        private Action callbackOnHide;


        /// <summary> Reference to the UIPopup clone used by this script</summary>
    private UIPopup m_popup;

     

        public void SetupPopup(string title="Default Title", string message="Default message", string labelYes="Yes", string labelNo="No", string eventOnYes="", string eventOnNo="", Action ActionOnYes = null, Action ActionOnNo=null)
        {
            SetActions(ActionOnYes, ActionOnNo);

            Title = title;
            Message = message;
            LabelButtonOne = labelYes;
            LabelButtonTwo = labelNo;
            EventOnYes= eventOnYes;
            EventOnNo= eventOnNo;
        }




    public void ShowPopup(string _popUpType = "SimplePopupYesNo")
        {

        //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database
        if (_popUpType != null)
        {
            m_popup = UIPopup.GetPopup(_popUpType);
        }
        else 
        { 
            m_popup = UIPopup.GetPopup(PopupName); 
        }
            

            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            //we assume (because we know) this UIPopup has a Title and a Message text objects referenced, thus we set their values
            if (!string.IsNullOrEmpty(Title) && !string.IsNullOrEmpty(Message))
                m_popup.Data.SetLabelsTexts(Title, Message);


            //set the button labels
            if(!string.IsNullOrEmpty(LabelButtonOne) && !string.IsNullOrEmpty(LabelButtonTwo))
                m_popup.Data.SetButtonsLabels(LabelButtonOne, LabelButtonTwo);


            //set the buttons callbacks as methods
            m_popup.Data.SetButtonsCallbacks(ClickButtonOne, ClickButtonTwo);
          
            m_popup.Show(); //show the popup
            
        }


        private void ClickButtonOne()
        {
        if (callback1 != null){
            callback1.Invoke();
            ClearActions(); 
        }

        //DDebug.Log("Clicked button ONE: " + LabelButtonOne);
        if (!string.IsNullOrEmpty(EventOnYes))
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName(EventOnYes);
            if (HideOnButtonOne) ClosePopup();
        }

        private void ClickButtonTwo()
        {
            
            if (callback2 != null) { 
            callback2.Invoke(); 
            ClearActions(); 
        }
           

            //DDebug.Log("Clicked button TWO: " + LabelButtonTwo);
        if (!string.IsNullOrEmpty(EventOnNo))
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName(EventOnNo);
            if (HideOnButtonTwo) ClosePopup();
        }

        private void ClosePopup()
        {
            if (m_popup != null) m_popup.Hide();
        }




    public void SetActions(Action onYesAction, Action onNoAction) {
        callback1 = onYesAction;
        callback2 = onNoAction;
    }

    public void ClearActions()
    {
        callback1 = null;
        callback2 = null;
    }

   
}

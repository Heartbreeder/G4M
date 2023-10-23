using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace Doozy.Examples
{
    public class PopupPhoto : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "PopupPhoto";

        public string Title = "Example Title";
        public string Message = "This is an example message for this UIPopup";
        public Sprite Icon;

        public string EventOnClose;
        private UIPopup m_popup;


        public void SetupPopup(string title, string message, Sprite icon, string eventOnClose)
        {

            Title = title;
            Message = message;
            Icon = icon;
            EventOnClose = eventOnClose;
           
        }

        public void ShowPopup()
        {
            //get a clone of the UIPopup, with the given PopupName, from the UIPopup Database 
            m_popup = UIPopup.GetPopup(PopupName);

            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            //we assume (because we know) this UIPopup has a Title and a Message text objects referenced, thus we set their values
            m_popup.Data.SetLabelsTexts(Title, Message);
            m_popup.Data.SetButtonsCallbacks(ClickButtonExit);

            //Add the photo to pop up
            if (Icon != null) { m_popup.Data.SetImagesSprites(Icon); }


            m_popup.Show(); //show the popup


        }

      

        private void ClickButtonExit()
        {
            DDebug.Log("Clicked button Exit:");
            if (!string.IsNullOrEmpty(EventOnClose))
                GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName(EventOnClose);
            ClosePopup();
        }

        private void ClosePopup()
        {
            if (m_popup != null) m_popup.Hide();
        }
    }
}
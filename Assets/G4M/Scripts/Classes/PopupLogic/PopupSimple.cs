using Doozy.Engine.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Doozy.Examples
{
    public class PopupSimple : MonoBehaviour
    {
        [Header("Popup Settings")]
        public string PopupName = "PopupPhoto";
        public string Title = "Example Title";
        public string Message = "This is an example message for this UIPopup";
        public Sprite Icon;
        private UIPopup m_popup;
        public bool showinparent = false;




        public void SetupPopup(string title, string message, Sprite icon)
        {
            Title = title;
            Message = message;
            Icon = icon;
        }

        public void ShowPopup()
        {
            m_popup = UIPopup.GetPopup(PopupName);

            if (showinparent) { m_popup.transform.parent = this.transform.parent; }
            //make sure that a popup clone was actually created
            if (m_popup == null)
                return;

            //we assume (because we know) this UIPopup has a Title and a Message text objects referenced, thus we set their values
            m_popup.Data.SetLabelsTexts(Title, Message);
           
            if (Icon != null) { m_popup.Data.SetImagesSprites(Icon); }
          

            m_popup.Show(); //show the popup
        }
    }
}
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LogTextScript : MonoBehaviour
{

    public TextMeshProUGUI textUI,timeUI,categoryUI;
    public GameObject titleContent;

    public void Init(LogText log,bool  _showOnlyText = false)
    {
        if (_showOnlyText) { titleContent.SetActive(false); } else { titleContent.SetActive(true); }

        textUI.text = log.text;
        timeUI.text = log.timeStamp;
        categoryUI.text = log.category.ToString();

       
    }

}

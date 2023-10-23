using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ChatLogHandler : MonoBehaviour
{
    private TextMeshProUGUI textComp;
    
    // Start is called before the first frame update
    void Start()
    {
        textComp = GetComponent<TextMeshProUGUI>();
        Clear();
    }

    public void AddText(string sender, string text)
    {
        if (textComp == null) return;
        if (string.IsNullOrEmpty(sender))
        {
            textComp.text += "\n" + text;
        }
        else
        {
            textComp.text += "\n" + "<color=#" + Color.red.r.ToString() + Color.red.g.ToString()+ Color.red.b.ToString() + ">" + sender + "</color>" + ": " + text;
        }
    }

    public void Clear()
    {
        if (textComp == null) return;
        textComp.text = "";
    }
}

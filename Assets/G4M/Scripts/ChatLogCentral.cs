using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEngine;

public class ChatLogCentral : MonoBehaviour
{
    // Start is called before the first frame update
    public Stack<LogText> AllMessages;
    public ChatboxPopUpManager pop;
    public bool ReplaceTextWithColors = true;



    void Awake()
    {
        AllMessages = new Stack<LogText>();
    }

    private void Start()
    {
     //   InvokeRepeating("Test", 2, 10);
    }


    // 0 = Only PopUp, 1 =  Popup and import to ChatBox , 2 Only ChatBox (Comming soon)
    public void Addtext(string _text,ChatboxCategories _category= ChatboxCategories.General)
    {



        AllMessages.Push(new LogText(_text, _category));

        if (ReplaceTextWithColors) {
        _text = ReplaceBasicWordsToColor(_text);
        }


        DisplayMessage(_text, _category);
        Doozy.Engine.GameEventMessage.SendEvent("MessageAdded");


    }

    private void DisplayMessage(string _text, ChatboxCategories _category)
    {
        if (_category == ChatboxCategories.Achievement){ //Add to main list and Achievement PopUp
            pop.ShowPopUp(_text, _category);
        }

        if (_category == ChatboxCategories.FULL) // Acknolegment text pop up
        { //Add to main list and Achievement PopUp
            pop.PopupNameSimple_OK(_text);
        }

        if (_category == ChatboxCategories.Simple) // Acknolegment text pop up
        { //Add to main list and Achievement PopUp
            pop.PopupNameSimple_OK_ver2(_text);
        }

    }


    public string ReplaceBasicWordsToColor(string message)
    {



        message =  Regex.Replace(message, "correct", "<color=green>Correct</color>", RegexOptions.IgnoreCase);
        message = Regex.Replace(message, "error", "<color=red>Error</color>", RegexOptions.IgnoreCase);
        message = Regex.Replace(message, "Worning", "<color=orange>Worning</color>", RegexOptions.IgnoreCase);
       

        return message;
    }


    /// <summary>
    /// This is test popups
    /// </summary>







}

public class LogText
{
    public string timeStamp;
    public ChatboxCategories category = ChatboxCategories.General; 
    public string text;

    public LogText(string _text, ChatboxCategories cat) {
        text = _text;
        category = cat;
        timeStamp = System.DateTime.Now.ToString();
    }
   

    
}
public enum ChatboxCategories
{
    General,
    ChatBox,
    Achievement,
    OK,
    Warning,
    FULL,
    Simple
    
}

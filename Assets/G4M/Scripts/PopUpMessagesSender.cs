using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpMessagesSender : MonoBehaviour
{
    // Start is called before the first frame update
    public void Test_FULL(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.FULL);
    }

    public void Test_Achievement(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.Achievement);
    }

    public void Test_ChatBox(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.ChatBox);
    }

    public void Test_OK(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.OK);
    }

    public void Test_General(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.General);
    }
    public void Test_Warning(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.Warning);
    }

    public void Test_SimplePopUp(string message)
    {
        GameMaster.Instance.GetComponent<ChatLogCentral>().Addtext(message, ChatboxCategories.Simple);
    }

}

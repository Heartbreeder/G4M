using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;
using Doozy.Engine.UI.Base;

public class UIViewListenerScript : MonoBehaviour
{

    public string[] CategoryViews;
    public string[] ExtraViewsNames;
    public string[] On_Show_GameEvents;
    public string[] On_Hide_GameEvents;
    // Start is called before the first frame update
    void Start()
    {
        List<string> categories = new List<string>(CategoryViews);
        List<string> showEvents = new List<string>(On_Show_GameEvents);
        List<string> hideEvents = new List<string>(On_Hide_GameEvents);
        List<string> extraNames = new List<string>(ExtraViewsNames);
        foreach (UIView item in UIView.Database)
        {


            if(categories.Contains(item.ViewCategory) || extraNames.Contains(item.ViewName)) { 
            item.ShowBehavior.OnStart.AddGameEvents(showEvents);
            item.HideBehavior.OnStart.AddGameEvents(hideEvents);
            }


        }

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

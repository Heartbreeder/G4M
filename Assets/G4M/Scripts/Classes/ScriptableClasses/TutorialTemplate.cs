using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class TutorialTemplate
{
    public string Name;
    public int Stage;
    public bool IsExample;

    [SerializeField]
    public TutorialPage[] TutorialPageArray;


    public TutorialTemplate() { }

    public TutorialTemplate(string name, bool isExample, TutorialPage[] tutorialArray)
    {
        Name = name;
        IsExample = isExample;
        TutorialPageArray = tutorialArray;

    }
    public TutorialTemplate(string name, bool isExample, TutorialPage[] tutorialArray, string description, string devNotes)
    {
        Name = name;
        IsExample = isExample;
        TutorialPageArray = tutorialArray;

    }
}

[System.Serializable]
public class TutorialPage
{
    public string PageTitle;
    public Sprite PageImage;
}

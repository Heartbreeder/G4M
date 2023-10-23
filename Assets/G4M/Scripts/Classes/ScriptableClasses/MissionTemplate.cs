using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

[System.Serializable]
public class MissionTemplate
{
    #region Basic data
    public string Name;
    public int ID;
    public int Stage;
    public bool isThisMissionTutorialForTheStage;
    #endregion

    #region Mission Description

    [TextArea(2, 5)]
    public string Description;

    [TextArea(2, 5)]
    public string MissionDetails;

    [SerializeField]
    public RequestedTool[] RequestedToolList;
    
    public GameObject FinishedModel;

    public Sprite CarvingTool;
    public Sprite FinalProduct;
    public Sprite CarvingPathBlueprint;
    public Sprite MechanicalBlueprint;
    #endregion

    #region Mission Code

    [TextArea(2, 200)]
    public string TopCode;

    [TextArea(2, 200)]
    public string MiddleCode;

    [TextArea(2, 200)]
    public string BottomCode;

    [TextArea(2, 200)]
    public string TopCodeDescription;

    [TextArea(2, 200)]
    public string MiddleCodeDescription;

    [TextArea(2, 200)]
    public string BottomCodeDescription;
    #endregion

    #region Mission Solution


    public VideoClip CarvingVideo;
    public Sprite[] CarvingSnapshots;
    #endregion


}

[System.Serializable]
public class RequestedTool
{
    public string RequestedToolID;
    public string RequestedToolDimensions;
}



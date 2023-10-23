using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OptionsData : MonoBehaviour
{
    #region Options Data
    public string LastPlayerProfile;
    public float MusicVolume;
    public float SFXVolume;
    public int ResolutionIndex;
    public int QualityIndex;
    public bool FullscreenToggle;



    #endregion

    #region Unsaved Options
    //new 
     public int FPS;
    #endregion

    #region Constructors
    public OptionsData() { }

    public OptionsData(OptionsDataContainer data)
    {
        Init(data);
    }
    #endregion

    #region Deserialiser
    public void Init(OptionsDataContainer data)
    {
        LastPlayerProfile = data.LastPlayerProfile;
        MusicVolume = data.MusicVolume;
        SFXVolume = data.SFXVolume;
        ResolutionIndex = data.ResolutionIndex;
        QualityIndex = data.QualityIndex;
        FullscreenToggle = data.FullscreenToggle;
    }
    #endregion

    #region Unity Functions
    private void Awake()
    {
        LoadOptions();


    }

    private void OnApplicationQuit()
    {
        SaveOptions();
    }

    #endregion

    #region Save System Functions
    public void SaveOptions()
    {
        Debug.Log("Save System: Saving System Options Data.");
        SaveSystem.SaveOptions(this);
    }

    public void LoadOptions()
    {
        OptionsDataContainer data = SaveSystem.LoadOptions();
        
        if (data != null)
        {
            LastPlayerProfile = data.LastPlayerProfile;
            MusicVolume = data.MusicVolume;
            SFXVolume = data.SFXVolume;
            ResolutionIndex = data.ResolutionIndex;
            QualityIndex = data.QualityIndex;
            FullscreenToggle = data.FullscreenToggle;
            FPS = 30;
        }
        else
        {// default values
            Debug.Log("Options data not found; creating new file.");
            LastPlayerProfile = "";
            MusicVolume = 0;
            SFXVolume = -52;
            ResolutionIndex = 0;
            QualityIndex = 2;
            FullscreenToggle = true;
            FPS = 30;
            SaveSystem.SaveOptions(this);
        }

        //Load Last Profile
        if (!string.IsNullOrEmpty(LastPlayerProfile))
        {
            PlayerDataContainer Pdata = new PlayerDataContainer();
            Pdata = SaveSystem.LoadPlayerData(LastPlayerProfile);
            GameMaster.Instance.gameObject.GetComponent<PlayerData>().LoadPlayer(LastPlayerProfile);
            /*
            if (Pdata != null)
            {

                //set Game Master's Player profile data to the loaded one.
                GameMaster.Instance.gameObject.GetComponent<PlayerData>().Init(Pdata);
            }
            else
            {
                Debug.Log("Save System: Last played profile is Null or Empty; No Player Profile loaded.");
                //DO SOMETHING IF LAST PROFILE IS EMPTY
                /*
                PlayerData newUser = new PlayerData(LastPlayerProfile, 0);
                Pdata = new PlayerDataContainer(newUser);
                SaveSystem.SavePlayerData(newUser, newUser.ProfileName);
                */
            //}

        }
        else
        {
            Debug.Log("Save System: Last played profile is Null or Empty; No Player Profile loaded.");
            //DO NOTHING IF LAST PROFILE IS EMPTY
            /*
            PlayerData newUser = new PlayerData("Μηχανικός", 0);
            Pdata = new PlayerDataContainer(newUser);
            SaveSystem.SavePlayerData(newUser, newUser.ProfileName);
            LastPlayerProfile = "Μηχανικός";
            */
        }
    }

    #endregion

}

[System.Serializable]
public class OptionsDataContainer
{
    #region Serialised Options Data
    public string LastPlayerProfile;
    public float MusicVolume;
    public float SFXVolume;
    public int ResolutionIndex;
    public int QualityIndex;
    public bool FullscreenToggle;

    #endregion

    #region Serialiser
    public OptionsDataContainer(OptionsData data)
    {
        LastPlayerProfile = data.LastPlayerProfile;
        MusicVolume = data.MusicVolume;
        SFXVolume = data.SFXVolume;
        ResolutionIndex = data.ResolutionIndex;
        QualityIndex = data.QualityIndex;
        FullscreenToggle = data.FullscreenToggle;
    }
    #endregion
}

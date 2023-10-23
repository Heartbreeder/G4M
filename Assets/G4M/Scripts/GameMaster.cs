using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour
{
    public static GameMaster Instance { get; private set; }



    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else { Destroy(gameObject); }
    }

    private void Start()
    {
        //PlayerData pd = new PlayerData("empty");
        //SaveSystem.SavePlayerData(pd, pd.ProfileName);
        //SaveSystem.LoadAllPlayerData();
    }

}

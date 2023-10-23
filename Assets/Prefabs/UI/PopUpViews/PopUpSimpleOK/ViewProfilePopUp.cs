using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class ViewProfilePopUp : MonoBehaviour
{
    [SerializeField] private TMP_Text _name, _xp, _money, _milingStage, _timePlayed,_Viewlog;
    
    // Start is called before the first frame update
    void Start()
    {
        LoadPlayerProfileInfo();
    }

    private void LoadPlayerProfileInfo()
    {
        _name.text=GameMaster.Instance.GetComponent<PlayerData>().ProfileName;
        _xp.text = GameMaster.Instance.GetComponent<PlayerData>().Exp.ToString();
        _money.text = GameMaster.Instance.GetComponent<PlayerData>().Money.ToString();


        _milingStage.text = GameMaster.Instance.GetComponent<PlayerData>().MillingStage.ToString();

        int time = Mathf.FloorToInt(GameMaster.Instance.GetComponent<PlayerData>().TimeMemory) ;

        _timePlayed.text = (time/60).ToString()+"min";
        _Viewlog.text = GameMaster.Instance.GetComponent<PlayerData>().PlayerLog;
    }

   
}

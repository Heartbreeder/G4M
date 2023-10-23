using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SystemSettingsPerformance : MonoBehaviour
{
    // Start is called before the first frame update

   
    private void Start()
    {
       // QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = GameMaster.Instance.GetComponent<OptionsData>().FPS;
    }

    public void SetTargetFrameRate(int rate) {
        Application.targetFrameRate = rate;
        GameMaster.Instance.GetComponent<OptionsData>().FPS = rate;
        Debug.Log("Target Frame Rate set to : " + rate);
    }
    public void SetQuality(int qualityIndex)
    {
        QualitySettings.SetQualityLevel(qualityIndex);
        GameMaster.Instance.GetComponent<OptionsData>().QualityIndex = qualityIndex;
    }
}

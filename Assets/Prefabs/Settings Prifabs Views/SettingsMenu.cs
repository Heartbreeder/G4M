using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using TMPro;
using UnityEngine.UI;
public class SettingsMenu : MonoBehaviour
{

    public AudioMixer audioMixer;


    private Resolution[] resolutions;
    public TMP_Dropdown resolutionDropDown;
    public TMP_Dropdown qualityDropDown;
    public Toggle fsp30, fsp60;
    // Start is called before the first frame update

    void Start()
    {
        DropDownResolutions();
        ReadFromOptionData();
       
    }

    //Here we check all the resolutions our system supports and put them in DropDown
    private void  DropDownResolutions() {
        resolutions = Screen.resolutions;
        resolutionDropDown.ClearOptions();
        int currentResolutionIndex = 0;


        List<string> options = new List<string>();
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);
            if (resolutions[i].width == Screen.currentResolution.width && resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }
        resolutionDropDown.AddOptions(options);
        resolutionDropDown.value = currentResolutionIndex;
        resolutionDropDown.RefreshShownValue();

    }



    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("MyMusicVolume", volume);

    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("MySFXVolume", volume);

    }

    //------ Graphics settings
    public void SetQuality(int qualityIndex) {
        GameMaster.Instance.GetComponent<SystemSettingsPerformance>().SetQuality(qualityIndex);
        if (qualityIndex == 0)
        {
            fsp30.interactable = false;
            ToggleFSPto60(true);
            fsp60.interactable = false;
        }
        else {
            fsp30.interactable = true;
            fsp60.interactable = true;
        }
    }

    public void SetResolution(int resolutionIndex)
    {
        if (resolutions != null) { 
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        }
    }
    public void SetFullScreen(bool isFullScreen) {
        Screen.fullScreen = isFullScreen; ;
    }



    public void SetFPS(int fps)
    {
        GameMaster.Instance.GetComponent<SystemSettingsPerformance>().SetTargetFrameRate(fps);
       
    }

    public void ToggleFSPto30(bool isOn)
    {
        if (isOn)
        {
            fsp30.isOn = true;
            SetFPS(30);
        }

    }

    public void ToggleFSPto60(bool isOn)
    {
        if (isOn) {
            fsp60.isOn = true;
            SetFPS(60);
        }
       

    }
    // Update is called once per frame

    public void ReadFromOptionData() {
        int fps = GameMaster.Instance.GetComponent<OptionsData>().FPS;
        int quality = GameMaster.Instance.GetComponent<OptionsData>().QualityIndex;
        qualityDropDown.value = quality;
        SetQuality(quality);

        if (fps == 30) ToggleFSPto30(true);
        else ToggleFSPto60(true);
    }

    private void OnEnable()
    {
        ReadFromOptionData();
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;
using TMPro;
public class VideoPlayerScript : MonoBehaviour
{

    private VideoPlayer videoPlayer;
    public Button playButton,pauseButton;
    public TextMeshProUGUI currentMinutes;
    public TextMeshProUGUI currentSeconds;

    public TextMeshProUGUI totalMinutes;
    public TextMeshProUGUI totalSeconds;

    public Slider VideoSlider;
    // Start is called before the first frame update
    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
    }

    void Start()
    {
      
       
    }


    public void Init(VideoClip clip) {
        videoPlayer.clip = clip;
        videoPlayer.targetTexture.Release();
        
        SetTotalTimeUI();
    }

    private void Update()
    {
        if (videoPlayer.isPlaying) {

            SetCurrentTimeUI();
            VideoSlider.GetComponent<VideoSlider>().MovePlayhead(CalculatePlayedFraction());
        }
       
    }
    void SetCurrentTimeUI()
    {
        string minutes = Mathf.Floor((int)videoPlayer.time / 60).ToString("00");
        string seconds = ((int)videoPlayer.time % 60).ToString("00");

        currentMinutes.text = minutes;
        currentSeconds.text = seconds;
    }


    public void PlayPause()
    {
        if (videoPlayer.isPlaying)
        {
            videoPlayer.Pause();
            playButton.gameObject.SetActive(true);
            pauseButton.gameObject.SetActive(false);
        }
        else
        {
            playButton.gameObject.SetActive(false);
            pauseButton.gameObject.SetActive(true);
            videoPlayer.Play();
            
        }
    }



   

    void SetTotalTimeUI()
    {
        string minutes = Mathf.Floor((int)videoPlayer.clip.length / 60).ToString("00");
        string seconds = ((int)videoPlayer.clip.length % 60).ToString("00");

        totalMinutes.text = minutes;
        totalSeconds.text = seconds;

      
    }


    float CalculatePlayedFraction(){

        float fraction = (float)videoPlayer.frame / (float)videoPlayer.clip.frameCount;
        return fraction;
    }


    public void MoveToTimeStep(float value)
    {
        videoPlayer.time = videoPlayer.clip.length * value;
    }
}

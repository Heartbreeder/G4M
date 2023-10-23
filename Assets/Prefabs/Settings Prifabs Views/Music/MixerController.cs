using UnityEngine;
using UnityEngine.Audio;

public class MixerController : MonoBehaviour
{
    public AudioMixer audioMixer;
    // Start is called before the first frame update
    public void SetMusicVolume(float volume) {
        audioMixer.SetFloat("MyMusicVolume", volume);
    
    }
    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("MySFXVolume", volume);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

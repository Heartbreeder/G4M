using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(AudioSource))]
public class CrossController : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI InteractiveText;
    public bool isInteracting = false;
    public Animator crossAnimator;
    public AudioClip OnIteractSound,OffInteractionSound;
    private AudioSource audio;
    void Start()
    {
        
            InteractiveText.text = "";
            audio = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!string.IsNullOrEmpty(InteractiveText.text))
        {
            isInteracting = true;
        }
        else {
            isInteracting = false;
        }

        crossAnimator.SetBool("Interaction", isInteracting);
    }

    public void PlaySoundInteractionOn() {

        audio.PlayOneShot(OnIteractSound);
    }

    public void PlaySoundInteractionOff()
    {

        audio.PlayOneShot(OffInteractionSound);
    }
}

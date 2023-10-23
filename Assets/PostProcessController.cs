using System.Collections;
using System.Collections.Generic;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.Rendering;
using UnityEngine;

public class PostProcessController : MonoBehaviour
{
    public Volume vol;
    DepthOfField dof;
    public float blurOn = 0.1f;
    public float blurOff = 2;
    public float focusSpeed = 5f;

    private IEnumerator coroutine;
    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    public void SetBlurEffect(bool isOn)
    {
        if (isOn) { 
            
            dof.focusDistance.value = blurOn; 
        }
        else { dof.focusDistance.value = blurOff; }
    }

    public void SetBlurEffectCorutine(bool isOn)
    {
        vol.sharedProfile.TryGet(out dof);

        if (isOn)
        {
            
            coroutine = DoBlurON(blurOn);
            StartCoroutine(coroutine);
        }
        else
        {
            coroutine = DoBlurOFF(blurOff);
            StartCoroutine(coroutine); }
    }


    IEnumerator DoBlurON(float _blur)
    {
        dof.active = true;
        float threshold = 10f;
        while (threshold>0.01f)
        {
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, _blur, Time.deltaTime *focusSpeed);
            threshold = Mathf.Abs(dof.focusDistance.value- _blur);
            // Yield execution of this coroutine and return to the main loop until next frame
            yield return null;
        }


        
    }

    IEnumerator DoBlurOFF(float _blur)
    {
        float threshold = 10f;
        while (threshold > 0.01f)
        {
            dof.focusDistance.value = Mathf.Lerp(dof.focusDistance.value, _blur, Time.deltaTime * focusSpeed);
            threshold = Mathf.Abs(dof.focusDistance.value - _blur);
            // Yield execution of this coroutine and return to the main loop until next frame
            yield return null;
        }
        dof.active = false;


    }


}

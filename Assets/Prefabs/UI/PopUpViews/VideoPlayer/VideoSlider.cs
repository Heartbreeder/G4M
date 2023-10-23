using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Slider))]
public class VideoSlider : MonoBehaviour
{
    // Start is called before the first frame update


    private void Start()
    {
        GetComponent<Slider>().value = 0;
    }

    public void MovePlayhead(float playedFraction)
    {
        GetComponent<Slider>().value = playedFraction;
    }

    
}

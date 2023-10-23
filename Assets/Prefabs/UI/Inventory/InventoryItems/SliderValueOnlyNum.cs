using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class SliderValueOnlyNum : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textValue;
    [SerializeField]
    private Slider slider;
 

    private void Start()
    {
        textValue.text = slider.value.ToString();
    }
    public void SetValue()
    {
        textValue.text = slider.value.ToString();
      
    }


    public void IncreaseValue() {
        slider.value++;
    }

    public void DecreaseValue()
    {
        slider.value--;
    }

    public void SetValueToFull()
    {
        slider.value = slider.maxValue;

    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class SliderValue : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI textValue;
    [SerializeField]
    private Slider slider;
    [SerializeField]
    private TextMeshProUGUI TotalPrice;
    [SerializeField]
    private TextMeshProUGUI ItemPrice;


    private void Start()
    {
        textValue.text = slider.value.ToString();
    }
    public void SetValue() {
        textValue.text = slider.value.ToString();
        TotalPrice.text = (slider.value * float.Parse(ItemPrice.text)).ToString();

    }


    public void IncreaseValue()
    {
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

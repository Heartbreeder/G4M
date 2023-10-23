using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerProfileItem : MonoBehaviour
{
    
    public TextMeshProUGUI name;
    public Toggle toggle;

    // Update is called once per frame
   public void Init(string _name, ToggleGroup group)
    {
        name.text = _name;
        gameObject.name = _name;
        toggle.group = group;
    }


}

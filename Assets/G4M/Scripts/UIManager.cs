using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject[] UIlistToEnable;
    void Awake()
    {
        foreach (GameObject item in UIlistToEnable)
        {
            item.SetActive(true);
        }
    }

   
}

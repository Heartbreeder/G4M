using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FillFormButton : MonoBehaviour
{
    // Start is called before the first frame update

    public void OpenUrlForm(string url)
    {
        //Application.OpenURL(url);
        Application.OpenURL(Application.persistentDataPath + "/Saves/");
       
    }

}

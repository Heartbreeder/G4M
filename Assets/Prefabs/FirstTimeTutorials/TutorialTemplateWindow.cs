using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine.UI;


public class TutorialTemplateWindow : MonoBehaviour
{
    public UIView uiview;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void HideUI() {
        uiview.Hide(true);
    }
   
    public void DestroyAfterHide()
    {
       
        Destroy(this.gameObject);
    }
}

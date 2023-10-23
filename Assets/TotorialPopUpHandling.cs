using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TotorialPopUpHandling : MonoBehaviour
{
    public GameObject TargetObject;
    public float DestroyDelay = 0f;
    public void DestorySelf() {

        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("LockCursor");

        if (TargetObject != null) { 

            Destroy(TargetObject, DestroyDelay);
        }
        else {
            Destroy(this.gameObject, DestroyDelay);
        }
    }

    private void Start()
    {

        GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("UnlockCursor");
    }
   
}

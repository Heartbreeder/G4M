using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class InteractionRaycast : MonoBehaviour
{
    public GameObject mainCamera;
    public TextMeshProUGUI textfield;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        textfield.text = "";
        int layer = 1 << 10;

        if (!GameMaster.Instance.GetComponent<PlayerData>().CursorLocked)
            return;

        RaycastHit hit;
        if (Physics.Raycast(mainCamera.transform.position,mainCamera.transform.forward, out hit, 3.0f, layer))
        {
            InteractionData data = hit.transform.gameObject.GetComponent<InteractionData>();
            if (data != null)
            {
                textfield.text = data.shownText;
                GameMaster.Instance.GetComponent<PlayerData>().playerTarget = hit.transform.gameObject;
            }
        }

    }
}

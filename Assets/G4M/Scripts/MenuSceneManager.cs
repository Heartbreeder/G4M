using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuSceneManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        SetCursorUnlock();
    }

    public void SetCursorUnlock()
    {
        
        
        //we force unlock the cursor if the user disable the cursor locking helper
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        
    }



    public void Exit() {
        Application.Quit();
      
    }


    public void QuitFromApplication_PopPup()
    {

        // GameMaster.Instance.GetComponent<PopupConfirm>().PopupName = CreateProfile_PopupName;
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Τερματισμός παιχνιδιού", "Θέλετε να βγείτε απο το παιχνίδι?", "Ναι", "Οχι", "", "");
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(Exit, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup();

    }
}

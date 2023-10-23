using TMPro;
using UnityEngine;

public class AllMessageUI : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI TextUI;
   


    // Update is called once per frame
    public void Init()
    {
        

        if (GameMaster.Instance.GetComponent<ChatLogCentral>().AllMessages == null) return;





            if (GameMaster.Instance.GetComponent<ChatLogCentral>().AllMessages.Count!= 0) {

            string messages = "";
            int count = 1;
            foreach (LogText item in GameMaster.Instance.GetComponent<ChatLogCentral>().AllMessages)
            {
                messages += count++.ToString()+" - "+item.timeStamp+" - "+"\t"+"Category: "+item.category+"\n"+item.text+"\n\n";
            
            }

            TextUI.text = messages;
        }

    }


    
}

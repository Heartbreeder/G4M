using Doozy.Engine;
using Doozy.Engine.UI;

using UnityEngine;

public class CustomButtonsExecution : MonoBehaviour
{
    // Start is called before the first frame update
    public void BackButton()
    {
        Message.Send(new UIButtonMessage("Back", UIButtonBehaviorType.OnClick));

    }


}

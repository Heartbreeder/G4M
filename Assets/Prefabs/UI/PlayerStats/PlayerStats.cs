using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Doozy.Engine.UI;
public class PlayerStats : MonoBehaviour
{
    // Start is called before the first frame update
    public TextMeshProUGUI _name, _xp ,_money;
    public Image ProfilePhoto;


    private void Start()
    {
        Init();
    }
    public void Init()
    {
        if (!string.IsNullOrEmpty(GameMaster.Instance.GetComponent<PlayerData>().ProfileName))
        {

            ProfilePhoto.sprite = GameMaster.Instance.GetComponent<PlayerData>().AvatarSprite;
            NameUpdate();
            MoneyUpdate();
            XpUpdate();
            this.transform.parent.GetComponent<UIView>().Show(false);
        }
        else { this.transform.parent.GetComponent<UIView>().Hide(true); }
    }



    public void XpUpdate() {
        _xp.text = GameMaster.Instance.GetComponent<PlayerData>().Exp.ToString();
    }
    public void MoneyUpdate() {
        _money.text = GameMaster.Instance.GetComponent<PlayerData>().Money.ToString();
    }
    public void NameUpdate() {
        _name.text = GameMaster.Instance.GetComponent<PlayerData>().ProfileName;
    }


    private void OnEnable()
    {
        XpUpdate();
        MoneyUpdate();
    }

}

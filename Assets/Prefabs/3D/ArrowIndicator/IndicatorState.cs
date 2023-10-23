using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class IndicatorState : MonoBehaviour
{
    [SerializeField] private GameObject indicator;
    public string Tag = "Indicator";
    public AnimationType _animationType = AnimationType.RotateAndMoveY;
    public float speed = 5f;


    public  enum AnimationType
    {
        RotateAndMoveY,
        MoveY,
        Fade

    }

    public void OnEnable() {
        if(_animationType == AnimationType.RotateAndMoveY) {
           indicator.transform.DORotate(new Vector3(0, 360, 0), speed, RotateMode.FastBeyond360).SetLoops(-1).SetEase(Ease.Linear);
           indicator.transform.DOLocalMoveY(0.2f, speed).SetLoops(-1,LoopType.Yoyo).SetEase(Ease.InOutSine);
           }
        else if (_animationType == AnimationType.Fade)
        {
            indicator.transform.GetComponent<MeshRenderer>().sharedMaterial.DOFade(0.2f, speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
        else if (_animationType == AnimationType.MoveY)
        {
            indicator.transform.DOLocalMoveY(0.2f, speed).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutSine);
        }
    }

    private void OnDisable()
    {
       
    }

}

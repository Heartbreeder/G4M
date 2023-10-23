using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Doozy.Engine;
public class Pointer : MonoBehaviour
{
    [SerializeField] private Transform targetPosition;
    [SerializeField] private RectTransform pointerRectTransform;
    [SerializeField] private TMP_Text DistanceUIText;

    public Vector3 offcet;
    public string EventNamePointer = "Pointer";
    private float onLimit = 2;
    private bool isHidden;
    
    
    public string suffix = "m";
    private Transform fromPosition;
   void Awake()
    {
        
        fromPosition = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Transform>();
        isHidden = false;
    
    }

    // Update is called once per frame
    void Update()
    {
        if (isHidden) return;

      

        float borderSize = 20f;
        Vector3 targetPositionScreenPoint = Camera.main.WorldToScreenPoint(this.targetPosition.position);

        float distance = Vector3.Distance(this.fromPosition.position, this.targetPosition.position);
        DistanceUIText.text = Mathf.FloorToInt(distance).ToString() + suffix;

        float dot = Vector3.Dot(fromPosition.forward, (targetPosition.position - fromPosition.position).normalized);

        bool isOffScreen = targetPositionScreenPoint.x <= borderSize ||
                             targetPositionScreenPoint.x >= Screen.width- borderSize ||
                             targetPositionScreenPoint.y <= borderSize ||
                             targetPositionScreenPoint.y >= Screen.height- borderSize;

        if (isOffScreen)
        {

            Vector3 cappedTargetPosition = targetPositionScreenPoint;
                 

                        if (cappedTargetPosition.x <= borderSize) cappedTargetPosition.x = borderSize;
                        if (cappedTargetPosition.x >= Screen.width- borderSize) cappedTargetPosition.x = Screen.width - borderSize;
                        if (cappedTargetPosition.y <= borderSize) cappedTargetPosition.y = borderSize;
                        if (cappedTargetPosition.y >= Screen.height- borderSize) cappedTargetPosition.y = Screen.height- borderSize;


            this.pointerRectTransform.position = cappedTargetPosition;
           
           
        }
        else {

            if (dot < 0)
            {
                this.pointerRectTransform.GetComponent<CanvasGroup>().alpha = 0;
                return;
            }

            this.HidePointerOnDistance(distance, this.onLimit);
            this.pointerRectTransform.position = targetPositionScreenPoint + offcet;
           
        }


       
    }

    public void SetPoint(Transform target) {
        isHidden = false;
        this.pointerRectTransform.gameObject.SetActive(true);
        this.targetPosition = target;

    }

     

    private void HidePointerOnDistance(float _distance, float onLimit) {
        



        if (_distance <= onLimit)
        {

            this.pointerRectTransform.GetComponent<CanvasGroup>().alpha -= Time.deltaTime * 2f;

           /* if (_instance.pointerRectTransform.GetComponent<CanvasGroup>().alpha == 0)
            {
                this.pointerRectTransform.gameObject.SetActive(false);
                isHidden = true;
            }*/

        }
        else 
        {
            this.pointerRectTransform.GetComponent<CanvasGroup>().alpha += Time.deltaTime *2f;
           // this.pointerRectTransform.gameObject.SetActive(true);
        }
    }





    private void OnEnable()
    {
        Message.AddListener<GameEventMessage>(OnMessage);
    }

    private void OnDisable()
    {
        Message.RemoveListener<GameEventMessage>(OnMessage);

    }


    private void OnMessage(GameEventMessage message)

    {
        if (message == null) return;
        if (!message.EventName.Equals(EventNamePointer)) return;


        if (message.Source != null) {
            SetPoint(message.Source.transform);
        }
        
      

     

    }
}

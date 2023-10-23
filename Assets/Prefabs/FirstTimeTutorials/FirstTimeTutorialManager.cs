using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstTimeTutorialManager : MonoBehaviour
{
    public GameObject[] ListOfTutorials;
    private GameObject ActiveTutorial;
    private Animator animator;
    public float delayTime = 1f;

    public static FirstTimeTutorialManager _instance;
    private void Awake()
    {
        _instance = this;
    }
    public int State {
        get { return GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState; }
        set { 

            GameMaster.Instance.GetComponent<PlayerData>().FirstTimeTutorialState = value;
        }
    }


    // Update is called once per frame

    void Start() {
        _instance.ShowPopUP();
    }

   
    public void ShowTutorial(int stateNumber)
    {
        if (stateNumber == -2) {
           
        }
        else { 
            StartCoroutine(_instance.Show(stateNumber, delayTime));
        }
    }

    public IEnumerator Show(int stateNumber, float delayTime)
    {
        
        yield return new WaitForSeconds(delayTime);


        Debug.Log("ShowTutorial = " + stateNumber.ToString());
        if (_instance.ActiveTutorial != null) { Destroy(_instance.ActiveTutorial); }

        _instance.ActiveTutorial = Instantiate(_instance.ListOfTutorials[stateNumber], _instance.transform);
        // Now do your thing here
    }



    public void ShowPopUpTutorial(int stateNumber)
    {

        if (_instance.ActiveTutorial != null) {
            _instance.ActiveTutorial.SendMessageUpwards("HideUI");
           // _instance.ActiveTutorial.GetComponent<TutorialTemplateWindow>().HideUI(); 
        }

        _instance.ActiveTutorial = Instantiate(_instance.ListOfTutorials[stateNumber], _instance.transform);
        // Now do your thing here
    }




    public void StateInit() {
        if (_instance.State == -2)
        {
            _instance.animator.SetInteger("state", _instance.State);
           
            return;
        }

        if (_instance.State >=0 && _instance.State <= 11) {
            _instance.animator.SetInteger("state", _instance.State); 
        }

        if (_instance.State == -1)
        {
            _instance.NextSate();
        }



        



    }

    


    public void SetState(int stateNumber)
    {
        _instance.State = stateNumber;
        _instance.animator.SetInteger("state", _instance.State);
       
    }

    public void NextSate( ) {

        if (_instance.State == -2)
        {
            //NoMoreStates
            return;
        }

        if (_instance.State == 5)
        {
            Debug.Log("Entered NextStep and added state in the first  try");
            //FirstFirstState
            _instance.animator.SetInteger("state", ++_instance.State);

        }

        if (_instance.State >= -1)
        {
            Debug.Log("Entered NextStep and added state in the first  try");
            //FirstFirstState
            _instance.animator.SetInteger("state", ++_instance.State);
           
        }
        
         

       
    }



    public void FirstTimeTutorialFinished()
    {
        _instance.State = -2;

    }

   

    public void WelcomeToTheGameMessage() {


        _instance.animator = _instance.GetComponent<Animator>();
        //if state = -2 then all the first time tutorils are completed
        if (_instance.State == -2 || _instance.State >= 4)
        {
            FirstTimeTutorialFinished();
            Debug.Log("FirstTimeTuotiral CurrentState   ->" + _instance.State);
          
            GameMaster.Instance.GetComponent<PlayerData>().ExecuteEventByName("ActivatePlayerCamera");
           
        }



        _instance.StateInit();

    }
   
    public void ShowPopUP()
    {
        string name = GameMaster.Instance.GetComponent<PlayerData>().ProfileName;
        //0--- Set the type of popup 
       
        //1--- Setup a PopUp (default Events are empty strings)
        GameMaster.Instance.GetComponent<PopupConfirm>().SetupPopup("Καλώς ήρθατε στο παιχνίδι  " + "<color=orange>"+name+ "</color>", "Καλώς ήρθατε στο παιχνίδι  " + "<color=orange>" + name + "</color>", "Ναί", "Όχι", "LockCursor", "");
        //2--- Setup Actions (default actions are Null, if you dont need Actions you can skip this step  
        GameMaster.Instance.GetComponent<PopupConfirm>().SetActions(_instance.WelcomeToTheGameMessage, null);
        GameMaster.Instance.GetComponent<PopupConfirm>().ShowPopup("PopapFULL");

    }




}

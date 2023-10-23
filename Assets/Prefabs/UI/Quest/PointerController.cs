using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Doozy.Engine;
public class PointerController : MonoBehaviour
{
    public static PointerController _instance;
    [SerializeField] private GameObject[] pointers;

    private void Awake()
    {
        _instance = this;
    }

    private void Start()
    {
        HideAll();
    }

    public void HideAll() {
        foreach (var item in pointers)
        {
            item.SetActive(false);
        }   
    }

    public void Show_MachineAndShops() {
        HideAll();

        pointers[0].SetActive(true); //Pointer Machine
        pointers[1].SetActive(true); //Pointer MaterialShop
        pointers[2].SetActive(true); //Pointer ToolShop
    }

    public void Show_officePointers()
    {
        HideAll();

        pointers[3].SetActive(true); //Pointer Gcode
        pointers[4].SetActive(true); //Pointer MaterialShop
        pointers[5].SetActive(true); //Pointer MissionBoard
    }


    public void Show_MissionBoard()
    {
        HideAll();
        pointers[5].SetActive(true); //Pointer MissionBoard
    }

    public void Show_GcodeAndTutorials()
    {
        HideAll();
        pointers[3].SetActive(true); //Pointer Gcode
        pointers[4].SetActive(true); //Pointer Tutorials
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

        if (message.EventName.Equals("NewMissionsGenerated")) {
            Show_MissionBoard();
        }
        else if (message.EventName.Equals("MissionAccepted")){
            Show_GcodeAndTutorials();
        }
        else if (message.EventName.Equals("CodeChecked"))
        {
            Show_MachineAndShops();
        }







    }



}


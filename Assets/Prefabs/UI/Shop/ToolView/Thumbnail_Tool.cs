using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Thumbnail_Tool : MonoBehaviour
{
    [Header("Thumbail Properties")]
    // public string Name = "ThumbnailName";
    public ToolID ID;
    private ToolTemplate Tool;


    [Header("Thumbnails Fields")]
    [SerializeField]
    private TextMeshProUGUI ThumbNail_ToolName;
    [SerializeField]
    private TextMeshProUGUI ThumbNail_ToolPrice;
    [SerializeField]
    private Image ThumbNail_ToolImage;

    [Header("Master Parent")]
    [SerializeField]
    private GameObject ToolSpecsView;
    // Start is called before the first frame update

    void Start()
    {
        
        Init();
    }


    private ToolTemplate RetriveID(string ID)
    {

        for (int i = 0; i < MachineShopData.Tools.Array.Length; i++)
        {
            if (MachineShopData.Tools.Array[i].ID == ID) {
                return MachineShopData.Tools.Array[i];
            }
        }

        return null;
    }

    // Update is called once per frame
    public void Init()
    {



        Tool = RetriveID(ID.ToString()); //Must not return Null. 

        //set the UI content
        if (Tool != null) {
            ThumbNail_ToolName.text =Tool.ToolName;
            ThumbNail_ToolPrice.text = Tool.Price.ToString();

            if (Tool.ItemImage != null)
                ThumbNail_ToolImage.sprite = Tool.ItemImage;
        }
    }
    public void ShowSpecs()
    {
        
            if(Tool!=null)
            ToolSpecsView.GetComponent<Tool_Spec_View>().Init(Tool);
       
    }
    

    public enum ToolID
    {
        Tool1, Tool2, Tool3, Tool4, Tool5, Tool6, Tool7, Tool8, Tool9,Tool10, Tool11, Tool12
    }



}

using System.Collections;
using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class DialogDetection : MonoBehaviour
{
    [SerializeField] GameObject msgPanel;
    public TextMeshProUGUI msgText;
    public Button btnContinue;
    public Button btnClose;
    public Button btnContinue2;
    public Button btnClose2;

    public bool canMove = true;

    public Image avatar;

    GameObject actualNPC;
    int currSent = 0;

    public GameObject zInt;

    public GameObject zIntMinci;

    public GameObject player;
    public GameObject playerAfter;

    public GameObject nino1;
    public GameObject maria;

    public GameObject minci;

    public bool unaVez;

    private SoundManager soundManager;
    void Awake()
    {
        soundManager = GameObject.FindObjectOfType<SoundManager>();
    }

    void Start()
    {
        //GetComponent<Collider>().isTrigger = true;
        msgPanel.SetActive(false);
        btnClose.gameObject.SetActive(false);
        btnContinue.gameObject.SetActive(true);
        if(zInt!=null) zInt.SetActive(false);
        if(btnClose2 != null)  btnClose2.gameObject.SetActive(false);
        if(btnContinue2 != null) btnContinue2.gameObject.SetActive(false);
    }

    

    IEnumerator OnTriggerEnter(Collider other)
    {
        //if ((other.name == "Player") && Input.GetKeyDown(KeyCode.X))
        if (other.tag == "npc" )
        {
            actualNPC = other.gameObject;
            soundManager.PlaySound("hablaMujer");

            yield return new WaitForSeconds(3);
            
            msgText.text = "";
            msgPanel.SetActive(true);
            canMove = false;
            avatar.sprite = Resources.Load<Sprite>("Ilustrations/"+actualNPC.name);
            avatar.gameObject.SetActive(true);
                if(actualNPC.name=="silla"){
                    btnClose2.gameObject.SetActive(false);
                    btnContinue2.gameObject.SetActive(true);
                }
                else
                {
                    btnClose.gameObject.SetActive(false);
                    btnContinue.gameObject.SetActive(true);
                }
            
        }
        
    }
 IEnumerator OnTriggerStay(Collider other)
    {
        if(other.name == "silla" && !unaVez)
        {
            actualNPC = other.gameObject;
            //actualNPC.GetComponent<NPCDialog>().activarAnimacion();

            yield return new WaitForSeconds(3);
            unaVez = true;
            msgText.text = "";
            //avatar.sprite = ;
            msgPanel.SetActive(true);
            canMove = false;
            avatar.gameObject.SetActive(true);
            if(actualNPC.name=="silla"){
                btnClose2.gameObject.SetActive(false);
                btnContinue2.gameObject.SetActive(true);
            }
            else
            {
                btnClose.gameObject.SetActive(false);
                btnContinue.gameObject.SetActive(true);
            }
            
        }
    }
/* 
    IEnumerator OnTriggerStay(Collider other)
    {
        //if ((other.name == "Player") && Input.GetKeyDown(KeyCode.X))
        if (other.tag == "silla")
        {
            yield return new WaitForSeconds(2);
            actualNPC = other.gameObject;
            msgText.text = actualNPC.GetComponent<NPCDialog>().GetMsg();
            msgPanel.SetActive(true);
            canMove = false;
            avatar.sprite = Resources.Load<Sprite>("Ilustrations/minci");
            avatar.gameObject.SetActive(true);
            btnClose.gameObject.SetActive(false);
            btnContinue.gameObject.SetActive(true);
        }
    }
*/
    void OnTriggerExit(Collider other)
    {
        if (other.tag == "npc")
        {
            msgPanel.SetActive(false);
        }
    }

    public void BtnContinue()
    {
        canMove = false;

    }

    public void BtnClose()
    {
    }
}

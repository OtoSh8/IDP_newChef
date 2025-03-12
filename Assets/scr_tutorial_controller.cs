using System.Collections;
using UnityEngine;
using TMPro;
using TMPEffects.Components;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.SceneManagement;

public class scr_tutorial_controller : MonoBehaviour
{
    
    [Header("GameObject Links")]
    public scr_instructor_tutorial objinstr;

    public GameObject ui_button;

    public GameObject par_customer;
    public GameObject par_dialog;
    public GameObject par_dishlist;

    public TMP_Text obj_textfield_dialog;
    public TMP_Text obj_textfield_dishlist;

    public TMP_Text ingramt;


    public Animator txttitle;

    private string fullText; // Original text with rich tags
    private string displayedText; // Text currently being displayed
    public int crnt_dialog_index = 0;

    public bool isTalking = false;
    public int crnt_customer;

    private bool Started = false;

    [Header("Personality Preferences")]
    private string[] tutorial = { "Welcome to the tutorial! Here we will teach you the basics of becoming an <b>All Rounded Chef!</b>", "You will be taught how to use your handy cooking knife to prepare food, as well as how to plate and serve your food.", "For today's training, just make me a simple tomato soup. Good Luck!" };
    private string[] tutorialend = { "<funky>Good job</funky>,<!delay=0.02> you've finished the tutorial!", "Oh yeah, I almost forgot! ", "Don't forget to order the ingredients needed everyday, and don't order too much or you'll end up creating <+shake><color=red>food waste<color=black></+shake>!" };




    [Header("Other Preferences")]
    public float revealSpeed = 0.5f;
    public bool tutorialFinished = false;

    [Header("Dish Preferences")]
    //FOOD TYPES
    //1 Fried Rice
    //2 Soup
    private int typesofdishes = 3;
    private string[] dishnames = { "Fried Rice", "Soup", "Steak" };

    public List<string> dishlist = new List<string>();
    public void PlayBell()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(11);
    }
    public void ReInit()
    {
        crnt_customer = 0;
        dishlist.Clear();
        crnt_dialog_index = 0;
        Started = false;
        obj_textfield_dishlist.text = "";
    }

    private void Start()
    {
        dishlist.Add("2_1");
        par_dialog.GetComponent<Animator>().Play("ani_dialogue_enter");
        Customer_Speak(0);
        isTalking = true;
    }

    string GetDishList(bool settext)
    {
        string dishstring = "";

        foreach(string s in dishlist)
        {
            string dishname = "";
            string[] info = s.Split('_');
            dishname = dishnames[int.Parse(info[0])-1];

            dishstring += ", "+ info[1]+ " " + dishname;
            /*Debug.Log(dishname + info[1]);*/
            if (settext)
            {
                obj_textfield_dishlist.text += dishname +" x" + info[1] + "\n";
            }
            
        }

        

        return dishstring;
    }

    public void HandleWriterFinished(TMPWriter writer)
    {
        if(isTalking == true)
        {
            isTalking = false;
            par_dialog.transform.GetChild(1).gameObject.SetActive(true);
        }

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && isTalking == false && Started == false){
            if (crnt_dialog_index < tutorial.Length)
            {
                Customer_Speak(crnt_dialog_index);
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                isTalking=true;
            }
            else if (crnt_dialog_index >= tutorial.Length)
            {
                FinishedDialog();
                StartCoroutine(StartCook());
            }
        }
    }
    public void OnButtonHit()
    {
        if (isTalking == false && Started == false)
        {
            if (crnt_dialog_index < tutorial.Length)
            {
                Customer_Speak(crnt_dialog_index);
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                isTalking = true;
            }
            else if (crnt_dialog_index >= tutorial.Length)
            {
                FinishedDialog();
                StartCoroutine(StartCook());
            }
        }
    }
    public void FinishedDialog()
    {
        if (GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>() != null)
        {
            GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>().phase = 3;

        }
        if (tutorialFinished)
        {
            if (GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>() != null)
            {
                GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>().phase = 2;

            }
            SceneManager.LoadScene("scn_preplay");
        }

        //Finished Dialog
        par_dialog.GetComponent<Animator>().Play("ani_dialogue_hidden");
        obj_textfield_dialog.text = "";
        par_dialog.transform.GetChild(1).gameObject.SetActive(false);
        /*par_dishlist.SetActive(true);*/
        Started = true;
        ui_button.SetActive(false);
        
    }

    public IEnumerator StartCook()
    {
        txttitle.Play("ani_title_appear");
        PlayBell();
        yield return new WaitForSeconds(1f);
        objinstr.StartCook();
    }

    public void FinalDialog()
    {
        ReInit();
        par_dialog.GetComponent<Animator>().Play("ani_dialogue_enter");
        crnt_customer = 1;
        Customer_Speak(0);
        isTalking = true;

    }

    private void Customer_Speak(int x)
    {
        if (GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>() != null)
        {
            GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>().phase = 3;
        }

        string getnextdialog = "";
        switch (crnt_customer)
        {
            case 0:
                getnextdialog = tutorial[x];
                break;
            case 1:
                getnextdialog = tutorialend[x];
                break;
        }


        string constrstring = "";

        foreach (char y in getnextdialog)
        {
            if(y.ToString() == "$")
            {
                constrstring += GetDishList(false);
            }
            else
            {
                constrstring += y;
            }
        }


        obj_textfield_dialog.text = constrstring;
        crnt_dialog_index++;
    }

}

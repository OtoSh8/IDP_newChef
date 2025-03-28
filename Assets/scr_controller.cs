using System.Collections;
using UnityEngine;
using TMPro;
using TMPEffects.Components;
using UnityEngine.Events;
using System.Collections.Generic;
using System.Linq;

public class scr_controller : MonoBehaviour
{
    
    [Header("GameObject Links")]
    public scr_instructor objinstr;

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

    private bool isTalking = false;
    public int crnt_customer;

    private bool Started = false;

    [Header("Personality Preferences")]
    public int personalities = 2;
    public string[] Customer_Prideful = { "Hello there sir,<!wait=0.5> its such a <b><+shake><funky> Greeaaaaat </b></funky></+shake> day today huh?<!wait=1>", "Anyways,<!wait=0.2> I'm feeling <!delay=0.2><+shake> very hungry <!delay=0.02> </+shake> so I'll have $<!wait=1>", "I'm looking forward to enjoying a <wave><!delay=0.1>hugeeeeee<!delay=0.02></wave> feastttt.<!wait=0.2> Hehe." };
    public string[] Customer_Saving = { "Hello there, I'd like to have <!delay=0.2> maybe.....<!delay=0.02> <shake>Ahhh</shake>,<!wait=0.2> I don't know<!delay=0.1>....<!delay=0.02>", "<+shake><sketchy>Uhhhhhh</sketchy></+shake>,<!wait=0.2> how about<!delay=0.1>...<!wait=0.2><!delay=0.02> maybe not<!delay=0.1>....<!delay=0.02>", "<+grow>Oh</+grow>,<!wait=0.2> I'm sorry for holding up the line, I think I'll just have $" };

    private int Customer_Prideful_int = 3;


    [Header("Other Preferences")]
    public float revealSpeed = 0.5f;


    [Header("Dish Preferences")]
    //FOOD TYPES
    //1 Fried Rice
    //2 Soup
    private int typesofdishes = 3;
    private string[] dishnames = { "Fried Rice", "Soup", "Steak" };

    public List<string> dishlist = new List<string>();

    public void PlayCustomerIn()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(7);
    }
    public void PlayBell()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(11);
    }
    public void ReInit()
    {
        crnt_customer = 0;
        isTalking = false;
        dishlist.Clear();
        crnt_dialog_index = 0;
        Started = false;
        isTalking=false;
        obj_textfield_dishlist.text = "";
    }

    private void Start()
    {
        //TESTING PURPOSES

        GetCust();
        /*obj_textfield_dialog.gameObject.GetComponent<TMPWriter>().OnFinishWriter.AddListener(HandleWriterFinished);*/
        UpdateIngrList();
        
    }

    public void UpdateIngrList()
    {
        ingramt.text = "";
        foreach(int i in GameObject.Find("obj_var").GetComponent<scr_var>().amountingr)
        {
            ingramt.text += i<=0 ? "<color=red>" : "<color=green>" + "x" + i + "\n";
        }
        
    }

    public void GetCust()
    {
        ReInit();
        StartCoroutine(GetCustomer());
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

    private void GenerateDishes()
    {
        int[] x = {1,1};
        
        switch (crnt_customer)
        {
            case 0:
                //Pride Dish Count Range
                x[0] = 3;
                x[1] = 4;
                break;
            case 1:
                //Saving Dish Count Range
                x[0] = 1;
                x[1] = 2;
                break;
        }

        int[] crnt_ingr = (int[])GameObject.Find("obj_var").GetComponent<scr_var>().amountingr.Clone();


        int howmanydishes = Random.Range(x[0], x[1]+1);
        int[] generateddishes = new int[howmanydishes];


        for (int i = 0; i < howmanydishes; i++)
        {
            
            int gen = Random.Range(1, typesofdishes + 1);
            //0 carot
            //1 tomato
            //2 onion
            //3 eggplat
            //4 potato
            //5 meat
            //6 butter

            //1 fr - carrot meat
            //2 soup - onion, carrot, tomato, potato
            //3 steak - meat butter

            int[] required = new int[0];
            bool possible = true;

            switch (gen)
            {
                case 1:
                    required = new int[] { 0, 5 };
                    break;
                case 2:
                    required = new int[] { 2, 0, 1, 4 };
                    break;
                case 3:
                    required = new int[] { 5, 6 };
                    break;
                default:
                    required = new int[] { 0};
                    break;
            }

            foreach(int k in required)
            {
                if(crnt_ingr[k] <= 0)
                {
                    possible = false;
                }
            }

            if (possible)
            {
                foreach (int k in required)
                {
                    crnt_ingr[k] -= 1;
                }

                generateddishes[i] = gen;
            }

        }

        //Amount of Dish Types Here TOO
        for (int i = 1;i <= typesofdishes; i++)
        {
            int count = generateddishes.Count(n => n == i);
            if (count >= 1) dishlist.Add(i+"_" + count);
        }

        GameObject.Find("obj_var").GetComponent<scr_var>().amountingr = (int[])crnt_ingr.Clone();

    }

    public void HandleWriterFinished(TMPWriter writer)
    {
        if(isTalking == true)
        {
            isTalking = false;
            par_dialog.transform.GetChild(1).gameObject.SetActive(true);
            UpdateIngrList();
        }

    }

    private void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.Space) && isTalking == false && Started == false){
            if (crnt_dialog_index < Customer_Prideful.Length)
            {
                Customer_Speak(crnt_dialog_index);
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                isTalking=true;
            }
            else if (crnt_dialog_index >= Customer_Prideful.Length)
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
            if (crnt_dialog_index < Customer_Prideful.Length)
            {
                Customer_Speak(crnt_dialog_index);
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                isTalking = true;
            }
            else if (crnt_dialog_index >= Customer_Prideful.Length)
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
            GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>().phase = 0;
        }

        //Finished Dialog
        par_dialog.GetComponent<Animator>().Play("ani_dialogue_hidden");
        obj_textfield_dialog.text = "";
        isTalking = false;
        par_dialog.transform.GetChild(1).gameObject.SetActive(false);
        /*par_dishlist.SetActive(true);*/
        Started = true;
        ui_button.SetActive(false);
        
    }

    public IEnumerator StartCook()
    {
        par_dishlist.GetComponent<Animator>().Play("ani_dishlist_appear");
        yield return new WaitForSeconds(2.5f);
        txttitle.Play("ani_title_appear");
        PlayBell();
        yield return new WaitForSeconds(1f);
        objinstr.StartCook();
    }

    public IEnumerator GetCustomer()
    {

        int generate_p = Random.Range(0, personalities);

        switch (generate_p)
        {
            case 0:
                //Pride
                 crnt_customer = 0;
                if (GameObject.Find("obj_var").GetComponent<scr_var>().special)
                {
                    crnt_customer = 1;
                }
                break;
            case 1:
                //Saving
                crnt_customer = 1;
                break;
            
        }
        GenerateDishes();
        GetDishList(true);
        PlayCustomerIn();
        par_customer.GetComponent<Animator>().Play("ani_customer_enter",0);
        yield return new WaitForSeconds(1);
        par_dialog.GetComponent<Animator>().Play("ani_dialogue_enter");
        yield return new WaitForSeconds(1);
        ui_button.SetActive(true);
        Customer_Speak(0);
        isTalking = true;



        yield return null;
    }


    private void Customer_Speak(int x)
    {
        if(GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>() != null)
        {
            GameObject.Find("obj_arduino_handler").GetComponent<scr_serialhandler_button>().phase = 3;
        }
        string getnextdialog = "";
        switch (crnt_customer)
        {
            case 0:
                getnextdialog = Customer_Prideful[x];
                break;
            case 1:
                getnextdialog = Customer_Saving[x];
                break;
        }

        string constrstring = "";
        foreach (char y in getnextdialog)
        {
            if(y.ToString() == "$")
            {
                constrstring += GetDishList(false);
                UpdateIngrList();
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

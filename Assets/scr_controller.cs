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

    public Animator txttitle;

    private string fullText; // Original text with rich tags
    private string displayedText; // Text currently being displayed
    private int crnt_dialog_index = 0;

    private bool isTalking = false;
    private int crnt_customer;

    private bool Started = false;

    [Header("Personality Preferences")]
    public int personalities = 1;
    private string[] Customer_Prideful = { "Hello there sir,<!wait=0.5> its such a <b><+shake><funky> Greeaaaaat </b></funky></+shake> day today huh?<!wait=1>", "Anyways,<!wait=0.2> I'm feeling <!delay=0.2><+shake> very hungry <!delay=0.02> </+shake> so I'll have $<!wait=1>", "I'm looking forward to enjoying a <wave><!delay=0.1>hugeeeeee<!delay=0.02></wave> feastttt.<!wait=0.2> Hehe." };
    private int Customer_Prideful_int = 3;


    [Header("Other Preferences")]
    public float revealSpeed = 0.5f;


    [Header("Dish Preferences")]
    //FOOD TYPES
    //1 Fried Rice
    //2 Soup
    private int typesofdishes = 2;
    private string[] dishnames = { "Fried Rice", "Soup" };

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
    }

    private void Start()
    {
        //TESTING PURPOSES

        StartCoroutine(GetCustomer());
        /*obj_textfield_dialog.gameObject.GetComponent<TMPWriter>().OnFinishWriter.AddListener(HandleWriterFinished);*/
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
        }

        int howmanydishes = Random.Range(x[0], x[1]+1);
        int[] generateddishes = new int[howmanydishes];


        for (int i = 0; i < howmanydishes; i++)
        {
            //Amount of Dish Types Here => (count +1)
            /*            generateddishes[i] = Random.Range(1, typesofdishes+1);
            */
            generateddishes[i] = Random.Range(1, typesofdishes+1);
        }

        //Amount of Dish Types Here TOO
        for (int i = 1;i <= typesofdishes; i++)
        {
            int count = generateddishes.Count(n => n == i);
            if (count >= 1) dishlist.Add(i+"_" + count);
        }



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
            if (crnt_dialog_index < Customer_Prideful.Length)
            {
                Customer_Speak(crnt_dialog_index);
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                isTalking=true;
            }
            else if (crnt_dialog_index >= Customer_Prideful.Length)
            {
                //Finished Dialog
                par_dialog.GetComponent<Animator>().Play("ani_dialogue_hidden");
                obj_textfield_dialog.text = "";
                isTalking = false;
                par_dialog.transform.GetChild(1).gameObject.SetActive(false);
                /*par_dishlist.SetActive(true);*/
                Started = true;
                ui_button.SetActive(false);
                StartCoroutine(StartCook());

            }
        }
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
        
        int generate_p = Random.Range(0, personalities-1);

        switch (generate_p)
        {
            case 0:
                //Pride
                 crnt_customer = 0;
                break;
            case 1:
                break;
            case 2:
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
        string getnextdialog = "";
        switch (crnt_customer)
        {
            case 0:
                getnextdialog = Customer_Prideful[x];
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

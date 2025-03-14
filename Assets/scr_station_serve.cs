using System.Collections.Generic;
using UnityEngine;

public class scr_station_serve : MonoBehaviour
{
    [SerializeField] GameObject par_toserve;
    private int totalmoney = 0;
    private int crntamt = 0;
    public bool readytoserve = false;

    [SerializeField] GameObject ui_button;
    [SerializeField] Material fr;
    [SerializeField] Material soup;
    [SerializeField] Material steak;

    [SerializeField] scr_station_closing closing;

    [SerializeField] scr_instructor_tutorial instrtut;


    List<int> dishes = new List<int>();

    public void ReInit()
    {
        totalmoney = 0;
        readytoserve = false;
        crntamt = 0;
        foreach(Transform child in par_toserve.transform){
            child.gameObject.SetActive(false);
        }

        dishes.Clear();
    }

    public void OnButtonHit()
    {
        if (readytoserve)
        {
                if(instrtut != null)
                {
                instrtut.tut_serve.SetActive(false);
                }
                this.GetComponent<Animator>().Play("ani_serve_serve");
                readytoserve = false;
                ui_button.SetActive(false);
        }
    }

    private void Update()
    {
        if (readytoserve)
        {
            if (!ui_button.activeSelf)
            {
                ui_button.SetActive(true);
            }

            if (Input.GetKey(KeyCode.Space))
            {
                if (instrtut != null)
                {
                    instrtut.tut_serve.SetActive(false);
                }
                this.GetComponent<Animator>().Play("ani_serve_serve");
                readytoserve = false;
                ui_button.SetActive(false);
            }
        }
    }
    public void AddDish(int no)
    {
        
        

        par_toserve.transform.GetChild(crntamt).gameObject.SetActive(true);
        switch (no)
        {
            case 1:
                par_toserve.transform.GetChild(crntamt).transform.GetChild(0).GetComponent<MeshRenderer>().material = fr;
                totalmoney += 20;
                break;
            case 2:
                par_toserve.transform.GetChild(crntamt).transform.GetChild(0).GetComponent<MeshRenderer>().material = soup;
                totalmoney += 15;
                break;
            case 3:
                par_toserve.transform.GetChild(crntamt).transform.GetChild(0).GetComponent<MeshRenderer>().material = steak;
                totalmoney += 35;
                break;
        }

        int percwaste = 0;


        if (GameObject.Find("obj_controller").GetComponent<scr_controller>() != null)
        {
            switch (GameObject.Find("obj_controller").GetComponent<scr_controller>().crnt_customer)
            {
                case 0:
                    percwaste = 65;
                    break;
                case 1:
                    percwaste = 5;
                    break;
            }
        }
        
        if (Random.Range(0, 100) < percwaste)
        {
            dishes.Add(no);
        }
        
        crntamt++;

    }

    private void OnServe()
    {
        if(GameObject.Find("obj_controller").GetComponent<scr_controller>() != null)
        {

            PlayCash();
            GameObject.Find("obj_var").GetComponent<scr_var>().AddMoney(totalmoney);
            GameObject.Find("station_closing").GetComponent<scr_station_closing>().totalearnt += totalmoney;
            dishes.Insert(0, GameObject.Find("obj_controller").GetComponent<scr_controller>().crnt_customer);
            closing.History.Add(dishes.ToArray());
            ReInit();

            GameObject.Find("npc").GetComponent<Animator>().Play("ani_customer_exit");
            GameObject.Find("par_dishlist").GetComponent<Animator>().Play("ani_dishlist_hidden");
        }
        else
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().FinishStep(null);
        }
        
    }

    private void PlayCash()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(14);
    }
}

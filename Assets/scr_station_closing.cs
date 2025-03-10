using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class scr_station_closing : MonoBehaviour
{
    [SerializeField] GameObject Pref_food;
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject cam_pos;
    [SerializeField] GameObject par_txt_value;

    public int totalearnt = 0;

    public List<Material> materials = new List<Material>();

    public List<int[]> History = new List<int[]>();

    public List<int> crnt = new List<int>();
    int crnt_person;

    public bool isScraping = false;

    public void StartScrape()
    {
        isScraping = true;
        GameObject.Find("obj_controller").GetComponent<scr_controller>().enabled = false;
        SetupScrape();
    }


    private void Update()
    {
        if (isScraping)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("SDSD");
                OnScrape();
            }
        }
    }
    public void ReInit()
    {
        History.Clear();
        crnt.Clear();
        totalearnt = 0;
    }

    public void SetupScrape()
    {
        if(crnt.Count > 0)
        {
            GameObject spawned = Instantiate(Pref_food,spawn.transform.position, spawn.transform.rotation, spawn.transform);
            spawned.GetComponent<Renderer>().material = materials[crnt[0]];
            crnt.RemoveAt(0);
            this.GetComponent<Animator>().Play("closing_setup");
        }
        else
        {
            if(History.Count > 0)
            {
                crnt = ((int[])History[0].Clone()).ToList();
                crnt_person = crnt[0];
                crnt.RemoveAt(0);
                History.RemoveAt(0);
                SetupScrape();
            }
            else
            {
                //FINISHED CLOSING
                int totalingrlost  = 0;
                foreach(int i in GameObject.Find("obj_var").GetComponent<scr_var>().amountingr)
                {
                    totalingrlost += i;
                }
                par_txt_value.transform.GetChild(0).GetComponent<Text>().text = GameObject.Find("obj_var").GetComponent<scr_var>().totalspent.ToString() + "$"; // cost
                par_txt_value.transform.GetChild(1).GetComponent<Text>().text = totalingrlost.ToString() + "$"; // ingr lost
                par_txt_value.transform.GetChild(2).GetComponent<Text>().text = ""; // food wasted
                par_txt_value.transform.GetChild(3).GetComponent<Text>().text = (totalearnt - GameObject.Find("obj_var").GetComponent<scr_var>().totalspent > 0 ? "<color=green>" : "<color=red>-") + (totalearnt - GameObject.Find("obj_var").GetComponent<scr_var>().totalspent) + "$";// profit
                par_txt_value.transform.parent.gameObject.SetActive(true);
            }
            
        }
    }

    public void OnScrape()
    {
        this.GetComponent<Animator>().Play("closing_scrape");
        
    }

    public void FinishScrape()
    {
        SetupScrape();
    }
}

using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class scr_station_closing : MonoBehaviour
{
    [SerializeField] GameObject Pref_food;
    [SerializeField] GameObject spawn;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject cam_pos;
    [SerializeField] GameObject par_txt_value;

    [SerializeField] scr_instructor instr;
    [SerializeField] scr_instructor_tutorial instrtut;
    [SerializeField] scr_scrape_dialog dia;

    public bool finalized = false;
    public int totalearnt = 0;

    public List<Material> materials = new List<Material>();

    public List<int[]> History = new List<int[]>();

    public List<int> crnt = new List<int>();
    public int crnt_person;

    public bool isScraping = false;

    public void StartScrape()
    {
        isScraping = true;
        if(instr != null)
        {
            GameObject.Find("obj_controller").GetComponent<scr_controller>().enabled = false;

        }
        SetupScrape();
    }


    private void Update()
    {

        if (isScraping && !finalized)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if(instrtut != null)
                {
                    instrtut.tut_closing.SetActive(false);
                }
                

                OnScrape();
            }
        }
        else if (finalized)
        {
            //restart scene boss
            GameObject.Find("obj_var").GetComponent<scr_var>().NewDay();
            SceneManager.LoadScene("scn_preplay");
        }
    }

    public void OnButtonHit()
    {
        if (isScraping)
        {
            if (instrtut != null)
            {
                instrtut.tut_closing.SetActive(false);
            }
            OnScrape();
        }
    }
    public void ReInit()
    {
        History.Clear();
        crnt.Clear();
        totalearnt = 0;
    }
    private void PlayCash()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(14);
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
                dia.gameObject.SetActive(true);
                crnt.RemoveAt(0);
                History.RemoveAt(0);
                SetupScrape();
            }
            else
            {
                dia.gameObject.SetActive(false);
                isScraping = false;

                //FINISHED CLOSING
                if (instr != null)
                {
                    int totalingrlost = 0;
                    foreach (int i in GameObject.Find("obj_var").GetComponent<scr_var>().amountingr)
                    {
                        totalingrlost += i;
                    }
                    par_txt_value.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = GameObject.Find("obj_var").GetComponent<scr_var>().totalspent.ToString() + "$"; // cost
                    par_txt_value.transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = "<color=red>" + totalingrlost.ToString() + "$"; // ingr lost
                    par_txt_value.transform.GetChild(2).GetComponent<TextMeshProUGUI>().text = ""; // food wasted
                    par_txt_value.transform.GetChild(3).GetComponent<TextMeshProUGUI>().text = (totalearnt - GameObject.Find("obj_var").GetComponent<scr_var>().totalspent > 0 ? "<color=green>" : "<color=red>") + (totalearnt - GameObject.Find("obj_var").GetComponent<scr_var>().totalspent) + "$";// profit
                    par_txt_value.transform.parent.gameObject.SetActive(true);
                    PlayCash();

                    finalized = true;
                }
                else
                {
                    instrtut.FinishStep(null);
                }
                
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

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
public class scr_sel : MonoBehaviour
{
    public int sel = 0;
    [SerializeField] Transform par_cons;
    [SerializeField] Transform par_icons;

    [SerializeField] TextMeshProUGUI txt_title;
    [SerializeField] TextMeshProUGUI desc;
    [SerializeField] Slider slider;
    [SerializeField] TextMeshProUGUI sliderval;
    [SerializeField] Slider orgsliderval;
    [SerializeField] Slider grnsliderval;

    [SerializeField] TextMeshProUGUI txt_totalspent;
    
    //carrot, tomato, onion, eggplant, potato, meat
    string[] names = { "Carrot", "Tomato", "Onion", "Eggplant", "Potato", "Meat", "Butter"};
    int[] prices = { 2, 3, 2, 4, 2, 10, 5};
    int[] amountingr = { 10, 7, 7, 0, 5, 5, 5 };

    int[] amountmax =    { 30, 30, 30, 30, 30, 20, 20 };
    int[] amountorange = { 25, 20, 20, 0, 20, 12, 12};
    int[] amountavg =    { 15, 12, 12, 0, 10, 8, 8};
    int[] amountmin =    { 7, 5, 5, 0, 3, 3, 3};

    private void Start()
    {

        SelectContainer();
    }
    private void Update()
    {
        int ho = 0;
        for(int i = 0; i < prices.Length; i++)
        {
            ho += prices[i]*amountingr[i];
        }
        txt_totalspent.text = "Total Spent: " + ho.ToString() + " $";

        if (Input.GetKeyDown(KeyCode.Space))
        {
            sel++;
            if(sel > 6)
            {
                sel = 0;
            }
            SelectContainer();
            if (GameObject.Find("tut") != null && GameObject.Find("tut").activeSelf)
            {
                GameObject.Find("tut").SetActive(false);
            }
        }

        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            AddIngr();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartGame();
        }
    }

    public void OnButtonHit()
    {
        sel++;
        if (sel > 6)
        {
            sel = 0;
        }
        SelectContainer();
        if (GameObject.Find("tut") != null && GameObject.Find("tut").activeSelf)
        {
            GameObject.Find("tut").SetActive(false);
        }
    }
    public void AddIngr()
    {
        int hoe = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            hoe += prices[i] * amountingr[i];
        }

        if (GameObject.Find("obj_var").GetComponent<scr_var>().money - hoe >= prices[sel])
        {
            amountingr[sel]++;
            if (amountingr[sel] > amountmax[sel])
            {
                amountingr[sel] = amountmin[sel];
            }
            SelectContainer();
        }
        else
        {
            amountingr[sel] = amountmin[sel];
        }
    }

    public void SelectContainer()
    {
        for(int x = 0; x < par_cons.childCount; x++)
        {
            par_cons.GetChild(x).GetChild(2).GetComponent<TextMeshProUGUI>().text = names[x] + " " + prices[x] + "$";
            par_cons.GetChild(x).GetChild(3).GetComponent<TextMeshProUGUI>().text = "x" + amountingr[x];

            if (x == sel)
            {
                par_cons.GetChild(x).GetChild(0).gameObject.SetActive(true);

                if(par_icons.childCount > x)
                {
                    par_icons.GetChild(x).gameObject.SetActive(true);
                }
                

                if (par_cons.GetChild(x).childCount >= 2)
                {
                    par_cons.GetChild(x).GetChild(1).GetComponent<Animator>().Play("ani_rotate");
                }
            }
            else
            {
                par_cons.GetChild(x).GetChild(0).gameObject.SetActive(false);
                if (par_icons.childCount > x)
                {
                    par_icons.GetChild(x).gameObject.SetActive(false);
                }

                if (par_cons.GetChild(x).childCount >= 2)
                {
                    par_cons.GetChild(x).GetChild(1).GetComponent<Animator>().Play("ani_idle");
                }
            }
        }




        switch (sel)
        {
            case 0: //carrot
                txt_title.text = "Carrot <color=green> $10";
                desc.text = "Tasty orange treats that pack a crunch!";
                
                break;
            case 1: //tomato
                txt_title.text = "Tomato <color=green> $10";
                desc.text = "Juicy balls of sweet and savoury goodness!";
                break;
            case 2:
                txt_title.text = "Onion <color=green> $10";
                desc.text = "These mean things will make you cry!";
                break;
            case 3:
                txt_title.text = "Eggplant <color=green> $10";
                desc.text = "Purple long bois!";
                break;
            case 4:
                txt_title.text = "Potato <color=green> $10";
                desc.text = "Potayto potahtoes!";
                break;
            case 5:
                txt_title.text = "Meat <color=green> $20";
                desc.text = "Who know's where this piece of meat came from.";
                break;
            case 6:
                txt_title.text = "butter <color=green> $20";
                desc.text = "Slippery bar of goodness.";
                break;
            default:
                txt_title.text = "";
                desc.text = "";
                break;

        }
        
        sliderval.text = amountingr[sel].ToString();

        

        slider.maxValue = amountmax[sel];
        slider.minValue = amountmin[sel];
        grnsliderval.maxValue = amountmax[sel];
        grnsliderval.minValue = amountmin[sel];
        orgsliderval.maxValue = amountmax[sel];
        orgsliderval.minValue = amountmin[sel];

        grnsliderval.value = amountavg[sel];
        orgsliderval.value = amountorange[sel];
        slider.value = amountingr[sel];
    }

    public void StartGame()
    {

        GameObject.Find("obj_var").GetComponent<scr_var>().time = 25200;
        GameObject.Find("obj_var").GetComponent<scr_var>().isTime = true;
        GameObject.Find("obj_var").GetComponent<scr_var>().amountingr = (int[])amountingr.Clone();

        int ho = 0;
        for (int i = 0; i < prices.Length; i++)
        {
            ho += prices[i] * amountingr[i];
        }
        GameObject.Find("obj_var").GetComponent<scr_var>().totalspent = ho;


        GameObject.Find("obj_var").GetComponent<scr_var>().MinusMoney(ho);

        SceneManager.LoadScene("scn_play");

    }
}

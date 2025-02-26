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

    int[] prices = { 10, 10, 10, 0, 0, 20, 0, 0 };
    int[] amountingr = { 2, 1, 1, 0, 1, 1, 0, 0 };

    int[] amountmax =    { 20, 10, 10, 1, 10, 10, 0, 0 };
    int[] amountorange = { 12, 8, 7, 0, 7, 6, 0, 0 };
    int[] amountavg =    { 6, 4, 4, 0, 4, 4, 0, 0 };
    int[] amountmin =    { 2, 1, 1, 0, 1, 1, 0, 0 };

    private void Start()
    {
        GameObject.Find("obj_var").transform.GetChild(0).gameObject.SetActive(true);
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
            if(sel > 7)
            {
                sel = 0;
            }
            SelectContainer();
        }

        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            StartGame();
        }
    }

    public void SelectContainer()
    {
        for(int x = 0; x < par_cons.childCount; x++)
        {
            if(x == sel)
            {
                par_cons.GetChild(x).GetChild(0).gameObject.SetActive(true);

                if(par_icons.childCount > x)
                {
                    par_icons.GetChild(x).gameObject.SetActive(true);
                }
                

                if (par_cons.GetChild(x).childCount == 2)
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

                if (par_cons.GetChild(x).childCount == 2)
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
        SceneManager.LoadScene("scn_play");

    }
}

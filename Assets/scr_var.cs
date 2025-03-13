using System.Collections;
using TMPro;
using UnityEngine;

public class scr_var : MonoBehaviour
{
    public int money = 0;
    public int totalspent = 0;

    public float time = 21600;
    public float timemultiplier;
    public bool special = false;

    public bool isTime = false;
    [SerializeField] TextMeshProUGUI moneytxt;
    [SerializeField] TextMeshProUGUI timetxt;

    public int[] amountingr = {0,0,0,0,0,0,0,0};

    void Start()
    {
        DontDestroyOnLoad(this.gameObject);
        AddMoney(1000);
        UpdateMoneyText();
    }

    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.V))
        {
            time = 9999999;
        }
        if (isTime)
        {
            SetTime();
        }
    }

    public void NewDay()
    {
        special = false;
        
        if(Random.RandomRange(0,100) < 30)
        {
            special = true;
        }
        
        SetTime();
        ResetTime();

    }
    private void SetTime()
    {
        time += Time.deltaTime * timemultiplier;
        string phase = "";
        int minutes = Mathf.FloorToInt(time / 60);
        int seconds = Mathf.FloorToInt(time % 60);
        int hours = Mathf.FloorToInt(minutes / 60);
        minutes -= (hours * 60);

        if(hours <= 6)
        {
            phase = "Opening Time";
        }
        else if (hours <= 20)
        {
            phase = "Service Ongoing";
        }
        else if(isTime)
        {
            
            GameObject.Find("obj_instructor").GetComponent<scr_instructor>().GetComponent<scr_instructor>().StationClosing();
            isTime = false;
            phase = "Closing Time";
        }

        timetxt.text = "<b>" + phase + " </b>" + string.Format("{0:00}:{1:00}", hours, minutes) + " <size=24>p.m. <br> " + (special ? "<color=red>Chinese New Year<color=white>" : "Normal Weekday");
    }

    public void ResetTime()
    {
        time = 36000;
    }

    public void AddMoney(int mon)
    {
        
        moneytxt.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "+ " + mon.ToString() + " $";
        moneytxt.gameObject.GetComponent<Animator>().Play("money_add");
        money += mon;
    }

    public void MinusMoney(int mon)
    {

        moneytxt.gameObject.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "- " + mon.ToString() + " $";
        moneytxt.gameObject.GetComponent<Animator>().Play("money_minus");
        money -= mon;
    }

    public void UpdateMoneyText()
    {
        moneytxt.text = money.ToString();
    }
}

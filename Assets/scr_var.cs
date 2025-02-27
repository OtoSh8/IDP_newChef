using System.Collections;
using TMPro;
using UnityEngine;

public class scr_var : MonoBehaviour
{
    public int money = 0;
    public float time = 21600;
    public float timemultiplier;

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
            AddMoney(100);
        }
        if (isTime)
        {
            SetTime();
        }
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
        else
        {
            phase = "Closing Time";
        }

        timetxt.text = "<b>" + phase + " </b>" + string.Format("{0:00}:{1:00}", hours, minutes) + " <size=24>p.m. <br> Normal Weekday";
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

    public void UpdateMoneyText()
    {
        moneytxt.text = money.ToString();
    }
}

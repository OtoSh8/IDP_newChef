using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class scr_station_plate : MonoBehaviour
{
    public float movespeed = 0;
    private Vector3 posog;
    private Vector3 original;
    [SerializeField] Transform pot;

    public float speed = 1;
    public float crntno = 0;
    private bool crntdir = true; // true right, false left
    private int targno = 10;
    int dishno = 0;
    private bool platdown = false;

    private bool hit = false;

    [SerializeField] Slider timebar;
    [SerializeField] Slider timebargoal;

    [SerializeField] scr_instructor instr;
    [SerializeField] scr_instructor_tutorial instrtut;

    [Header("Food Material References")]
    [SerializeField] scr_cook scr;
    [SerializeField] GameObject plate;
    [SerializeField] GameObject plateoutline;
    private Material mat_friedrice;
    private Material mat_soup;
    private Material mat_steak;

    private bool ready = false;

    private void Start()
    {
        original = pot.position;
        mat_friedrice = scr.mat_friedrice;
        mat_soup = scr.mat_soup;
        mat_steak = scr.mat_steak;
    }

    public void ReInit(int dish)
    {
        hit = false;
        dishno = dish;
        targno = Random.Range(10, 101);
        posog = original + new Vector3(-((float)targno) / 100f * movespeed, 0, 0);
        timebargoal.value = ((float)targno)/100f;
        pot.gameObject.SetActive(false);
        this.GetComponent<Animator>().Play("ani_plate_og");
        plateoutline.SetActive(true);
        ready = false;

    }

    public void OnButtonHit()
    {
        if (dishno != 0)
        {
            if (!platdown)
            {
                if (instrtut != null)
                {
                    instrtut.tut_plate.SetActive(false);
                }

                this.GetComponent<Animator>().Play("ani_plate_show");
                platdown = true;
                StartCoroutine(ShowTimebar());
            }

            else if (platdown && !hit && ready)
            {
                if (instrtut != null)
                {
                    instrtut.tut_plate2.SetActive(false);
                }

                if (crntno <= targno && crntno > targno - 10)
                {
                    //HIT IT BOI
                    Debug.Log("HIT");
                    hit = true;
                    timebar.gameObject.transform.parent.gameObject.SetActive(false);
                    pot.gameObject.SetActive(false);
                    OnPour();
                }
                else
                {
                    crntno = 0;
                }

            }
        }
    }

    public void Update()
    {
        if(dishno != 0)
        {
            if (Input.GetKeyDown(KeyCode.Space) && platdown == false)
            {
                if (instrtut != null)
                {
                    instrtut.tut_plate.SetActive(false);
                }
                this.GetComponent<Animator>().Play("ani_plate_show");
                platdown = true;
                StartCoroutine(ShowTimebar());
            }

            if(platdown == true && hit == false)
            {
                
                switch (crntdir)
                {
                    case true:
                        crntno += speed;
                        if(crntno >= 100)
                        {
                            crntdir = !crntdir;
                        }
                        break;
                    case false:
                        crntno -= speed;
                        if (crntno <= 0)
                        {
                            crntdir = !crntdir;
                        }
                        break;
                }
                timebar.value = ((float)crntno)/100f;
                pot.transform.position = posog + new Vector3(timebar.value*movespeed, 0,0);

                if (Input.GetKeyDown(KeyCode.Space) && ready)
                {
                    if (instrtut != null)
                    {
                        instrtut.tut_plate2.SetActive(false);
                    }
                    if (crntno <= targno && crntno > targno - 10)
                    {
                        //HIT IT BOI
                        Debug.Log("HIT");
                        hit = true;
                        timebar.gameObject.transform.parent.gameObject.SetActive(false);
                        pot.gameObject.SetActive(false);
                        OnPour();
                    }
                    else
                    {
                        crntno = 0;
                    }
                }
            }

        }
    }

    public void PlayPlate()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(3);
        plateoutline.SetActive(false);
        if (instrtut != null)
        {
            instrtut.tut_plate2.SetActive(true);
        }
    }

    public void PlayDrag()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(13);
    }
    IEnumerator ShowTimebar()
    {
        yield return new WaitForSeconds(2f);
        switch (dishno)
        {
            case 1:
                //FR
                pot.transform.GetChild(1).gameObject.SetActive(true);
                pot.transform.GetChild(0).gameObject.SetActive(false);
                pot.transform.GetChild(1).transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_friedrice;
                break;
            case 2:
                //S
                pot.transform.GetChild(1).gameObject.SetActive(false);
                pot.transform.GetChild(0).gameObject.SetActive(true);
                pot.transform.GetChild(0).transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_soup;
                break;
            case 3:
                //STK
                pot.transform.GetChild(1).gameObject.SetActive(true);
                pot.transform.GetChild(0).gameObject.SetActive(false);
                pot.transform.GetChild(1).transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_steak;
                break;
        }
        pot.gameObject.SetActive(true);
        timebar.gameObject.transform.parent.gameObject.SetActive(true);
        pot.gameObject.SetActive(true);
        ready = true;
        yield return null;
    }

    private void OnPour()
    {
        Material mat = null;

        switch (dishno)
        {
            case 1:
                //FR
                mat = mat_friedrice;
                break;
            case 2:
                //S
                mat = mat_soup;
                break;
            case 3:
                //S
                mat = mat_steak;
                break;
        }

        plate.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat;
        plate.transform.GetChild(0).gameObject.SetActive (true);

        this.GetComponent<Animator>().Play("ani_plate_servwe");
    }


    public void FinishPlating()
    {
        dishno = 0;
        this.GetComponent<Animator>().Play("ani_plate_og");
        platdown = false;
        plate.transform.GetChild(0).gameObject.SetActive(false);
        hit = false;

        if(instr != null)
        {
            instr.FinishStep(null);

        }
        else
        {
            instrtut.FinishStep(null);

        }

    }
}

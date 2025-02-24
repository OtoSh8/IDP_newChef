using NUnit.Framework;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class scr_cook : MonoBehaviour
{
    [Header("Action Values")]
    public int targetvalue;
    public int targettime;
    public float crnt_time = 0;
    private bool cooking = false;

    [Header("Finished Materials/Textures")]
    [SerializeField] public Material mat_water;
    [SerializeField] public Material mat_friedrice;
    [SerializeField] public Material mat_soup;


    [Header("Stovetop left/right References")]
    public int leftValue = 0;
    public int rightValue = 0; // The adjustable integer value
    public GameObject knob_left;
    public GameObject knob_right;
    public GameObject fire_left; // The GameObject to enable/disable based on currentValue
    public GameObject fire_right;
    public Transform Spawn_left;
    public scr_instructor instr;

    public GameObject finished_left;
    public GameObject finished_left_pot;
    public Animator objcooker;

    public GameObject spat;

    public GameObject objpan;
    public GameObject objpot;

    private bool isplayingaud = false;

    [SerializeField] public GameObject ui_knob;
    [SerializeField] public Slider ui_slider;

    public bool sel_side = false;
    private bool final = false;

    List<GameObject> ingr = new List<GameObject>();

    public bool pots = false;

    public void pot(bool ifpot)
    {
        pots = ifpot;

        if (pots)
        {
            objpot.SetActive(true);
            objpan.SetActive(false);
            spat.gameObject.SetActive(false);
            objpot.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_water;
        }
        else
        {
            objpot.SetActive(false);
            objpan.SetActive(true);
            spat.gameObject.SetActive(true);
        }

    }

    public void PlayClank()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(4);

    }
    private IEnumerator FinishCooking()
    {
        cooking = false;
        final = true;
        
        objcooker.Play("ani_cook_idle");
        ui_knob.SetActive(false);

        foreach (GameObject go in ingr)
        {
           Destroy(go);
        }

        ingr.Clear();

        if (!sel_side)
        {
            finished_left.SetActive(true);
            switch (GameObject.Find("obj_instructor").GetComponent<scr_instructor>().crntdish)
            {
                case 1:
                        finished_left.SetActive(true);
                        finished_left.GetComponent<MeshRenderer>().material = mat_friedrice;
                    break;
                case 2:
                    
                    objpot.transform.GetChild(0).GetComponent<MeshRenderer>().material = mat_soup;
                    break;

            }
        }
        isplayingaud = false;
        GameObject.Find("obj_audio").GetComponent<scr_audio>().StopAllLoop();
        yield return new WaitForSeconds(2f);

        crnt_time = 0;
        leftValue = 0;
        rightValue = 0;
        cooking = false;
        ui_knob.SetActive(false);
        ui_slider.gameObject.SetActive(false);
        instr.FinishStep(null);
        
        this.GetComponent<scr_cook>().enabled = false;

        
    }

    public void ReInit(List<GameObject> cutted)
    {
        ingr = cutted;
        crnt_time = 0;
        leftValue = 0;
        rightValue = 0;
        final = false;
        foreach (GameObject obj in cutted)
        {
            foreach(Transform t in obj.transform)
            {
                t.localPosition = new Vector3(0, 0, 0);
            }

            obj.transform.parent = Spawn_left.transform;
            obj.transform.localScale = new Vector3(obj.transform.localScale.x/2, obj.transform.localScale.y / 2, obj.transform.localScale.z / 2);
            obj.transform.localPosition = new Vector3(0, 0, 0);
        }

        finished_left.SetActive(false);
        cooking = false;
        ui_slider.gameObject.SetActive(false);
        ui_knob.SetActive(true);

    }

    void Update()
    {
        

        
        if (cooking && !final)
        {

            crnt_time += Time.deltaTime;
            if (pots)
            {
                if (!isplayingaud)
                {
                    GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundIDLoop(5);
                       isplayingaud = true;
                }
            }
            else
            {
                if (!isplayingaud)
                {
                    GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundIDLoop(10);
                    isplayingaud = true;
                }
                objcooker.Play("ani_cooking");
            }
            
            ui_slider.value = (float)crnt_time/targettime;
            if (crnt_time > targettime)
            {
                Debug.Log("FINISHED COOKING CUH");
                if (!final)
                {
                    StartCoroutine(FinishCooking());
                }
                
                
            }
        }
        else
        {
            objcooker.Play("ani_cook_idle");
        }

        
        /*if (leftValue > targetvalue) return;*/




        GameObject targetObject = null;
        GameObject objectToToggle = null;


        switch (sel_side)
        {
            case false:
                objectToToggle = fire_left;
                targetObject = knob_left;
                // Adjust the value with arrow keys
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    leftValue = Mathf.Min(leftValue + 1, 100); // Increase value, max 100
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    leftValue = Mathf.Max(leftValue - 1, 0); // Decrease value, min 0
                }

                // Rotate the GameObject based on the value
                float rotationY = (leftValue / 100f) * 360f; // Map value to 0-360 degrees
                targetObject.transform.localRotation = Quaternion.Euler(0, rotationY, 0);

                // Enable or disable the object based on currentValue
                if (leftValue > targetvalue)
                {
                    objectToToggle.SetActive(true); // Enable the GameObject
                    ui_knob.SetActive(false);
                    ui_slider.gameObject.SetActive(true);
                    cooking = true;

                }
                else
                {
                    objectToToggle.SetActive(false); // Disable the GameObject
                    cooking = false;
                    GameObject.Find("obj_audio").GetComponent<scr_audio>().StopAllLoop();
                    isplayingaud = false;
                }

                break;
            case true:
                objectToToggle = fire_right;
                targetObject = knob_right;

                // Adjust the value with arrow keys
                if (Input.GetKey(KeyCode.RightArrow))
                {
                    rightValue = Mathf.Min(rightValue + 1, 100); // Increase value, max 100
                }
                else if (Input.GetKey(KeyCode.LeftArrow))
                {
                    rightValue = Mathf.Max(rightValue - 1, 0); // Decrease value, min 0
                }

                // Rotate the GameObject based on the value
                float rotationY_ = (rightValue / 100f) * 360f; // Map value to 0-360 degrees
                targetObject.transform.localRotation = Quaternion.Euler(0, rotationY_, 0);

                // Enable or disable the object based on currentValue
                if (rightValue > 10)
                {
                    objectToToggle.SetActive(true); // Enable the GameObject
                }
                else
                {
                    objectToToggle.SetActive(false); // Disable the GameObject
                }
                break;
                
        }

        
    }
}
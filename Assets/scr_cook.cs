using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class scr_cook : MonoBehaviour
{
    [Header("Action Values")]
    public int targetvalue;
    public int targettime;
    public float crnt_time = 0;
    private bool cooking = false;

    [Header("Finished Materials/Textures")]
    [SerializeField] Material mat_friedrice;

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
    public Animator objcooker;

    public bool sel_side = false;

    List<GameObject> ingr = new List<GameObject>();

    private void FinishCooking()
    {
        cooking = false;
        objcooker.Play("ani_cook_idle");

        foreach (GameObject go in ingr)
        {
            go.SetActive(false);
        }

        if (!sel_side)
        {
            finished_left.SetActive(true);
            switch (GameObject.Find("obj_instructor").GetComponent<scr_instructor>().crntdish)
            {
                case 1:
                    finished_left.GetComponent<MeshRenderer>().material = mat_friedrice;
                    break;
            }
        }

        crnt_time = 0;
        leftValue = 0;
        rightValue = 0;
        cooking = false;
        this.GetComponent<scr_cook>().enabled = false;
        instr.FinishStep(null);
    }

    public void ReInit(List<GameObject> cutted)
    {
        ingr = cutted;
        crnt_time = 0;
        leftValue = 0;
        rightValue = 0;

        foreach (GameObject obj in cutted)
        {
            foreach(Transform t in obj.transform)
            {
                t.localPosition = new Vector3(0, 0, 0);
            }
            obj.transform.parent = Spawn_left.transform;
            obj.transform.localPosition = new Vector3(0, 0, 0);
        }

        finished_left.SetActive(false);
        cooking = false;
    }

    void Update()
    {
        if(cooking)
        {
            crnt_time += Time.deltaTime;
            objcooker.Play("ani_cooking");
            if(crnt_time > targettime)
            {
                Debug.Log("FINISHED COOKING CUH");
                
                FinishCooking();
            }
        }
        else
        {
            objcooker.Play("ani_cook_idle");
        }

        if (leftValue > targetvalue) return;

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
                if (leftValue > 10)
                {
                    objectToToggle.SetActive(true); // Enable the GameObject
                    cooking = true;

                }
                else
                {
                    objectToToggle.SetActive(false); // Disable the GameObject
                    cooking = false;
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
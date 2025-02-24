using UnityEngine;
using EzySlice;
using System.Collections.Generic;

public class scr_station_cut : MonoBehaviour
{
    [SerializeField] private scr_instructor instr;

    [SerializeField] private Transform cutspawn;

    [SerializeField] private GameObject obj_knife;
    public List<GameObject> ToCut;
    public List<bool> ToSalt;


    public void PlayCut()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(0);
    }

    public void Cut()
    {
        if (obj_knife.GetComponent<scr_knife>().cutting == false && obj_knife.GetComponent<scr_knife>().slicedcount < 5)
        {
            this.GetComponent<Animator>().Play("ani_cutting_cut");
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Cut();
        }
    }

    public void TriggerKnifeCut()
    {
        obj_knife.GetComponent<scr_knife>().CutIt();
    }

    public void Repos()
    {
        obj_knife.GetComponent<scr_knife>().Repos();
    }

    void SetupSlicedObject(GameObject obj)
    {
        obj.transform.position = cutspawn.transform.position;
        obj_knife.GetComponent<scr_knife>().PosStart(obj.GetComponent<MeshRenderer>().bounds.size.x);
        /*obj.AddComponent<MeshCollider>().convex = true;*/
        obj.AddComponent<Rigidbody>();  // Enable physics
        obj.tag = "Sliceable";
        obj.layer = LayerMask.NameToLayer("Objects");  // Keep layer for further slicing
    }

    public void StartCut()
    {
        if(ToCut.Count > 0)
        {
            obj_knife.GetComponent<scr_knife>().enabled = true;
            obj_knife.GetComponent<scr_knife>().Reinit();
            GameObject ix = Instantiate(ToCut[ToCut.Count-1]);
            obj_knife.GetComponent<scr_knife>().salt = ToSalt[ToSalt.Count - 1];
            SetupSlicedObject(ix);
            ToCut.RemoveAt(ToCut.Count - 1);
            ToSalt.RemoveAt(ToSalt.Count - 1);


        }
        else
        {
            //INGREDIENTS ALL CUT
            Debug.Log("DONE CUTTING");
            instr.FinishStep(obj_knife.GetComponent<scr_knife>().cutpars);
            obj_knife.GetComponent<scr_knife>().cutpars.Clear();
            this.GetComponent<scr_station_cut>().enabled = false;
            obj_knife.GetComponent<scr_knife>().enabled = false;
            obj_knife.GetComponent<scr_knife>().ui_icon_cut.SetActive(false);
            obj_knife.GetComponent<scr_knife>().ui_icon_salt.SetActive(false);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scr_mixer : MonoBehaviour
{
    [SerializeField] GameObject ui_mix;

    [SerializeField] GameObject objmixer;
    [SerializeField] Transform spawn;

    private bool isMoving = false;
    private float timecounter;

    public float Width;
    public float Length;
    public float Speed;

    private Vector3 startingpos;

    public int amt = 0;
    public int goal = 0;

    [SerializeField] scr_instructor instr;

    List<GameObject> ingr = new List<GameObject> ();
    public void Reinit(int x, List<GameObject> cutted)
    {
        amt = 0;
        goal = x;
        ingr.Clear ();

        if(cutted != null)
        {
            foreach(GameObject obj in cutted)
            {
                obj.transform.parent = this.transform.parent;
                obj.transform.localPosition = spawn.localPosition;
                ingr.Add(obj);
            }
            StartCoroutine(SpawnAll());
        }
        ui_mix.SetActive(true);
    }

    private IEnumerator SpawnAll()
    {
        foreach (GameObject obj in ingr)
        {
            obj.SetActive (true);
            yield return new WaitForSeconds(0.5f);
        }
    }

    private void Start()
    {
        startingpos = objmixer.transform.position;
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isMoving == false)
        {
            isMoving = true;
            amt++;
            StartCoroutine(GetSignal());
        }

        if (isMoving)
        {
            timecounter += Time.deltaTime * Speed;
        }
         

        objmixer.transform.position = new Vector3(Mathf.Sin(timecounter)* Width, 0f , Mathf.Cos(timecounter)*Length) + startingpos;
    }

    IEnumerator GetSignal()
    {
        PlayMix();
        yield return new WaitForSeconds(0.5f);
        isMoving = false;
        if(amt >= goal)
        {
            Finished();
        }
        yield return null;
    }

    public void PlayMix()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(1);
    }

    public void Finished()
    {
        ui_mix.SetActive(false);
        instr.FinishStep(ingr);
        this.GetComponent<scr_mixer>().enabled = false;
    }
}

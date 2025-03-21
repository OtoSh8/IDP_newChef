using UnityEngine;

public class scr_salt : MonoBehaviour
{
    [Header("Object References")]
    private Animator animator;
    private Transform spawnpoint;
    [SerializeField] ParticleSystem saltshaker;
    [Header("Preferences")]
    public bool salt_active = false;
    private bool up = false;

    public int amt = 0;

    public void PlayShake()
    {
        GameObject.Find("obj_audio").GetComponent<scr_audio>().PlaySoundID(2);
    }
    private void Start()
    {
        animator = GetComponent<Animator>();
        spawnpoint = transform.GetChild(0).GetChild(0).transform;
    }
    void Update()
    {

        if(salt_active && Input.GetKeyDown(KeyCode.UpArrow) && up == false)
        {
            if (GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>() != null)
            {
                GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().tut_salt.SetActive(false);
            }
            /*animator.Play("ani_salt_up",-1,0f);*/
            up = true;
            animator.SetBool("up", up);
            
        }
        else if (salt_active && Input.GetKeyDown(KeyCode.DownArrow) && up == true)
        {
            /*animator.Play("ani_salt_down", -1, 0f);*/
            up = false;
            animator.SetBool("up", up);
            CreateSalt();
        }
    }

    public void DoShaker()
    {
        if (GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>() != null)
        {
            GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().tut_salt.SetActive(false);
        }


        if (salt_active && up == false)
        {
            /*animator.Play("ani_salt_up",-1,0f);*/
            up = true;
            animator.SetBool("up", up);

        }
        else if (salt_active && up == true)
        {
            /*animator.Play("ani_salt_down", -1, 0f);*/
            up = false;
            animator.SetBool("up", up);
            CreateSalt();
        }
    }

    public void UpdateShakerUp()
    {
        /*up = true;*/
    }
    public void UpdateShakerDown()
    {
        /* up = false;*/
        saltshaker.Play();
    }

    private void CreateSalt()
    {
        amt++;
    }
}

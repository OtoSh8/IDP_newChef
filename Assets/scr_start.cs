using UnityEngine;
using UnityEngine.SceneManagement;

public class scr_start : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            StartGame();
        }
    }

    public void StartGame()
    {
        GameObject.Find("obj_var").transform.GetChild(0).gameObject.SetActive(true);
        SceneManager.LoadSceneAsync("scn_tutorial");
    }
}

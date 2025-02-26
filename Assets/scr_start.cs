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
        Debug.Log("SD");
        SceneManager.LoadScene("scn_preplay");
    }
}

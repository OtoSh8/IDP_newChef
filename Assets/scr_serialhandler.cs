using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;

public class scr_serialhandler : MonoBehaviour
{

    private int current;

    public static string knifeport = "COM16"; // Change COM port if necessary
    private SerialPort sp;
    private Thread IOThread;
    private bool threadRunning = false;
    private string incoming = "";

    private void Start()
    {
        DontDestroyOnLoad(this);

        // Initialize serial port
        sp = new SerialPort(knifeport, 9600);
        sp.ReadTimeout = 50; // Prevent freezing on read
        sp.WriteTimeout = 50;

        try
        {
            sp.Open(); // Open the serial port
            threadRunning = true;
            IOThread = new Thread(DataThread);
            IOThread.Start();
        }
        catch (System.Exception e)
        {
            Debug.LogError("Failed to open serial port: " + e.Message);
        }
    }

    private void DataThread()
    {
        while (threadRunning && sp.IsOpen)
        {
            try
            {
                string data = sp.ReadLine(); // Read line-by-line
                lock (this)
                {
                    incoming = data; // Store data safely
                }
            }
            catch (System.Exception) { } // Ignore errors (e.g., ReadTimeout)
        }
    }

    private void Update()
    {
        lock (this)
        {
            if (!string.IsNullOrEmpty(incoming))
            {
                Debug.Log("Received: " + incoming);
                if(System.Convert.ToInt16(incoming) > current)
                {
                    current = System.Convert.ToInt16(incoming);
                    
                    if(SceneManager.GetActiveScene().name == "scn_start")
                    {
                        GameObject.Find("obj_start").GetComponent<scr_start>().StartGame();
                    }
                    else if (SceneManager.GetActiveScene().name == "scn_preplay")
                    {
                        GameObject.Find("obj_selector").GetComponent<scr_sel>().StartGame();
                    }
                    else if (SceneManager.GetActiveScene().name == "scn_play")
                    {
                        GameObject.Find("station_cut").GetComponent<scr_station_cut>().Cut();
                    }
                                

                }

                incoming = ""; // Clear after reading
            }
        }
    }

    private void OnDestroy()
    {
        threadRunning = false;
        if (IOThread != null && IOThread.IsAlive)
            IOThread.Join(); // Ensure thread stops before quitting

        if (sp != null && sp.IsOpen)
        {
            sp.Close();
        }
    }
}

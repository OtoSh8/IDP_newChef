using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;

public class scr_serialhandler_button : MonoBehaviour
{

    private int current;

    public static string btnport = "COM16"; // Change COM port if necessary
    private SerialPort sp;
    private Thread IOThread;
    private bool threadRunning = false;
    private string incoming = "";

    public int phase = 0;
    private void Start()
    {
        DontDestroyOnLoad(this);

        // Initialize serial port
        sp = new SerialPort(btnport, 9600);
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

                //Button Pushed
                switch (phase)
                {
                    case 0:
                        //Rest Phase / Do Nothing
                        break;
                    case 1:
                        //Start Button
                        if(SceneManager.GetActiveScene().name == "scn_start")
                        {
                            GameObject.Find("obj_start").GetComponent<scr_start>().StartGame();
                        }
                        break;
                    case 2:
                        //Select Button
                        if (SceneManager.GetActiveScene().name == "scn_preplay")
                        {
                            GameObject.Find("obj_selector").GetComponent<scr_sel>();
                        }
                        break;
                    case 3:
                        //Chat Button
                        break;
                    case 4:
                        //Plate
                        break;
                    case 5:
                        //Serve
                        break;
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

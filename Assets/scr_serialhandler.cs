using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;

public class scr_serialhandler : MonoBehaviour
{

    private int current;

    public static string knifeport = "COM4"; // Change COM port if necessary
    private SerialPort sp;
    private Thread IOThread;
    private bool threadRunning = false;
    private string incoming = "";

    public int phase = 0;

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

    public void ChangePhase(int x)
    {
        phase = x;
        sp.Write(x.ToString());
        Debug.Log("Change Phase: " + phase);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Equals))
        {
            ChangePhase(phase + 1);

        }

        lock (this)
        {
            if (!string.IsNullOrEmpty(incoming))
        {
            Debug.Log("Received: " + incoming);

                switch (phase)
                {
                    case 0:
                        if (SceneManager.GetActiveScene().name == "scn_preplay")
                        {
                            GameObject.Find("obj_selector").GetComponent<scr_sel>().AddIngr();
                        }
                        break;
                    case 1:
                        GameObject.Find("station_cut").GetComponent<scr_station_cut>().Cut();
                        break;
                    case 2:
                        GameObject.Find("par_salt").GetComponent<scr_salt>().DoShaker();
                        break;
                    case 3:
                        GameObject.Find("par_mixer").GetComponent<scr_mixer>().DoMix();
                        break;
                    case 4:
                        break;
                }
                

                incoming = "";
            }
        }

            /*lock (this)
            {
                if (!string.IsNullOrEmpty(incoming))
                {
                    Debug.Log("Received: " + incoming);

    *//*                if(System.Convert.ToInt16(incoming) > current)
                    {
                        current = System.Convert.ToInt16(incoming);
                        *//*
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
                        else if (SceneManager.GetActiveScene().name == "scn_tutorial")
                        {
                            switch (phase)
                            {
                                //cut
                                //salt
                                //mix
                                case 0:
                                    if (Input.GetKeyDown(KeyCode.Q))
                                    {
                                        sp.Write("0");
                                    }
                                    break;
                                case 1:
                                    GameObject.Find("station_cut").GetComponent<scr_station_cut>().Cut();
                                    break;
                                case 2:
                                    GameObject.Find("par_salt").GetComponent<scr_salt>().DoShaker();
                                    break;
                                case 3:
                                    GameObject.Find("par_mixer").GetComponent<scr_mixer>().DoMix();
                                    break;
                            }
                            GameObject.Find("station_cut").GetComponent<scr_station_cut>().Cut();
                        }


                    *//*}*//*

                    incoming = ""; // Clear after reading
                }
            }*/
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

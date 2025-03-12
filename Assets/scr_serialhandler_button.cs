using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;
using System;

public class scr_serialhandler_button : MonoBehaviour
{

    private int current;

    public static string btnport = "COM7"; // Change COM port if necessary
    private SerialPort sp;
    private Thread IOThread;
    private bool threadRunning = false;
    private string incoming = "";

    public int phase = 1;
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
                if(incoming[0] == '_')
                {
                    if (SceneManager.GetActiveScene().name == "scn_tutorial")
                    {
                        GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().tut_cook.SetActive(false);
                        GameObject.Find("obj_instructor").GetComponent<scr_instructor_tutorial>().tut_cook2.SetActive(false);
                    }
                    if (SceneManager.GetActiveScene().name == "scn_tutorial" || SceneManager.GetActiveScene().name == "scn_play")
                    {
                        GameObject.Find("station_cook").GetComponent<scr_cook>().UpdateVal(100 - Convert.ToInt32(incoming.TrimStart('_')));
                    }
                    }
                else
                {
                    //Button Pushed
                    switch (phase)
                    {
                        case 0:
                            //Rest Phase / Do Nothing
                            break;
                        case 1:
                            //Start Button
                            if (SceneManager.GetActiveScene().name == "scn_start")
                            {
                                GameObject.Find("obj_start").GetComponent<scr_start>().StartGame();
                            }
                            break;
                        case 2:
                            //Select Button
                            if (SceneManager.GetActiveScene().name == "scn_preplay")
                            {
                                if(GameObject.Find("tut") != null && GameObject.Find("tut").activeSelf)
                                {
                                    GameObject.Find("tut").SetActive(false);
                                }
                                GameObject.Find("obj_selector").GetComponent<scr_sel>().OnButtonHit();
                            }
                            break;
                        case 3:
                            //Chat Button
                            if (SceneManager.GetActiveScene().name == "scn_play")
                            {
                                GameObject.Find("obj_controller").GetComponent<scr_controller>().OnButtonHit();
                            }
                            else if (SceneManager.GetActiveScene().name == "scn_tutorial")
                            {
                                GameObject.Find("obj_controller").GetComponent<scr_tutorial_controller>().OnButtonHit();
                            }
                            break;
                        case 4:
                            //Plate
                            if (SceneManager.GetActiveScene().name == "scn_play" || SceneManager.GetActiveScene().name == "scn_tutorial")
                            {
                                GameObject.Find("station_plate").GetComponent<scr_station_plate>().OnButtonHit();
                            }
                            break;
                        case 5:
                            //Serve
                            break;
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

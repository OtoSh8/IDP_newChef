using UnityEngine;
using System.IO.Ports;
using System.Threading;
using UnityEngine.SceneManagement;

public class scr_cook_serial : MonoBehaviour
{

    private int current;

    public static string knifeport = "COM3"; // Change COM port if necessary
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
                
                    current = System.Convert.ToInt16(incoming);
                    this.GetComponent<scr_cook>().UpdateVal(100-current);

                

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

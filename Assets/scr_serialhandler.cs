using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class scr_serialhandler : MonoBehaviour
{
    [SerializeField] scr_station_cut station_cut;
    private int current;

    public static string knifeport = "COM16"; // Change COM port if necessary
    private SerialPort sp;
    private Thread IOThread;
    private bool threadRunning = false;
    private string incoming = "";

    private void Start()
    {
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
                    station_cut.Cut();

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

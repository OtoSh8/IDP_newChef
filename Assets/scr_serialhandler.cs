using UnityEngine;
using System.IO.Ports;
using System.Threading;

public class scr_serialhandler : MonoBehaviour
{
    public static string knifeport = "COM";

    Thread IOThread = new Thread(DataThread);
    private static SerialPort sp;
    private static string incoming = "";
    private static string outgoing = "";

    private static void DataThread()
    {
        sp = new SerialPort(knifeport, 9600);
        sp.Open();

        if(outgoing != "")
        {
            sp.Write(outgoing);
            outgoing = "";
        }

        incoming = sp.ReadExisting();
        Thread.Sleep(200);
    }

    private void OnDestroy()
    {
        IOThread.Abort();
        sp.Close();
    }

    private void Start()
    {
        IOThread.Start();
    }

    private void Update()
    {
        if(incoming != "")
        {
            Debug.Log(incoming);
        }
    }
}

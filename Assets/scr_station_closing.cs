using System.Collections.Generic;
using UnityEngine;

public class scr_station_closing : MonoBehaviour
{
    public List<int[]> History = new List<int[]>();

    public void ReInit()
    {
        History.Clear();
    }
}

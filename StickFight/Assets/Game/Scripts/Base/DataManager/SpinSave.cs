using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SpinSave
{
    private const int TIME_COUNT_DOWN = 10800;
    public string str_timeStart;

    public SpinSave() {
        str_timeStart = DateTime.Today.AddDays(-1).ToString();
    }

    public DateTime DateStart {
        get {
            return DateTime.Parse(str_timeStart);
        }
    }

    public bool CanGetFree() {
        return DateTime.Compare(DateTime.Now, TimeGetReward()) >=0;
    }

    public DateTime TimeGetReward() {
        DateTime result = DateStart + new TimeSpan(0, 0, TIME_COUNT_DOWN);
        return result;
    }

    public void SetTimeStart() {
        str_timeStart = DateTime.Now.ToString();
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

[System.Serializable]
public class DailySave {
    public List<int> lstIndexReceived;
    public string str_TimeStart;
    public DateTime dateStart => DateTime.Parse(str_TimeStart);

    public DailySave() {
        lstIndexReceived = new List<int>();
        DateTime start = DateTime.Today.AddDays(-1);
        str_TimeStart = start.ToString();
    }

    public int GetCurIndex() {
        TimeSpan time = DateTime.Today - dateStart;
        int daymore = (int)time.TotalDays;
        if(daymore >= 1) {
            int days =lstIndexReceived.Count();
            if(days >= 7) {
                return 0;
            }
            return lstIndexReceived.Count();
        } else {
            return lstIndexReceived.Count() - 1;
        }
    }

    public bool CheckReceived(int index) {
        return lstIndexReceived.Contains(index);
    }

    public void AddIndexDay(int index) {
        lstIndexReceived.Add(index);
        str_TimeStart = DateTime.Today.ToString();
    }

    public void GetUpDate() {
        int index = GetCurIndex();
        if(index >= 7) {
            TimeSpan time = DateTime.Today - dateStart;
            int daymore = (int)time.TotalDays;
            if(daymore>=1) {
                lstIndexReceived.Clear();
            }
        }
    }
}

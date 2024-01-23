using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DisplayObjects : MonoBehaviour {
    [SerializeField] private List<GameObject> lstObj;
    //<summary>Index: -1(active all) ,-2 (uactive all)</summary>
    public void Active(params int[] indexs) {
        if(indexs.Length == 1 && (indexs[0] == -1 || indexs[0] == -2)) {
            foreach(GameObject obj in lstObj) {
                obj.SetActive(indexs[0] == -1 ? true : false);
            }
        } else {
            for(int i = 0; i < lstObj.Count; i++) {
                lstObj[i].SetActive(indexs.Contains(i) ? true : false);
            }
        }
    }

    public void ActiveAll(bool active) {
        if(active) {
            Active(-1);
        } else {
            Active(-2);
        }
    }

    public bool CheckActiveByIndex(int index) {
        return lstObj[index].activeInHierarchy;
    }
}

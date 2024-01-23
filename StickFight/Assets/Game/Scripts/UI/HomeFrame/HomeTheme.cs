using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeTheme : MonoBehaviour {
    [SerializeField] private RectTransform parent;
    [SerializeField] private List<HomeThemeData> lstHomeData;
    private void Start() {
        int levelMap = DataManager.Instance.PlayerData.LevelMap;

        GameObject backgroundPref = null;
        int amount = lstHomeData.Count;
        for(int i = 0; i < amount; i++) {
            if(levelMap <= lstHomeData[i].level) {
                backgroundPref = lstHomeData[i].objbg;
                break;
            }
        }

        if(backgroundPref != null) {
            var newBG = backgroundPref.Spawn();
            var rect = newBG.GetComponent<RectTransform>();
            rect.SetParent(parent);
            newBG.transform.SetAsLastSibling();
            newBG.transform.localScale = Vector3.one;
            rect.SetLeft(0f);
            rect.SetRight(0f);
            rect.SetTop(0f);
            rect.SetBottom(0f);

        }
    }
}

[System.Serializable]
public class HomeThemeData {
    public int level;
    public GameObject objbg;
}

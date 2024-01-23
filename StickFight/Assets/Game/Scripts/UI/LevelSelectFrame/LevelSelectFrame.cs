using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class LevelSelectFrame : FrameBase {
    [SerializeField] private List<LevelSelectView> levelView;
    [SerializeField] private Button btn_Right,btn_Left;
    [SerializeField] private TextMeshProUGUI txt_Detail;
    private int amountLevelView = 10;
    private int levelStart;
    private void Awake() {
        amountLevelView = levelView.Count;
        btn_Right.onClick.AddListener(SelectLevelRight);
        btn_Left.onClick.AddListener(SelectlevelLeft);
    }

    public override void OnShow(Action onCompleted = null, bool instant = false) {
        base.OnShow(onCompleted, instant);
        txt_Detail.text = $"LEVEL {DataManager.Instance.PlayerData.LevelMap+1}/{DataManager.Instance.LevelMapMax+1}";
        int curLevel = DataManager.Instance.PlayerData.LevelMap;
        levelStart = Mathf.FloorToInt(curLevel / amountLevelView) * amountLevelView;
        GenderLevel();
    }

    private void GenderLevel() {
        for(int i = 0; i < amountLevelView; i++) {
            levelView[i].Show(levelStart+i);
        }
    }

    private void SelectlevelLeft() {
        int levelCheck = levelStart-amountLevelView;
        if(levelCheck >= 0 ) {
            levelStart -=amountLevelView;
        }
        GenderLevel();
    }

    private void SelectLevelRight() {
        int levelCheck = levelStart+amountLevelView;
        if(levelCheck<= DataManager.Instance.LevelMapMax) {
            levelStart += amountLevelView;
        }
        GenderLevel();
    }
}
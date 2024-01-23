using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LevelSelectView : MonoBehaviour {
    [SerializeField] private TextMeshProUGUI txt_Level;
    [SerializeField] private GameObject iconBoss;
    [SerializeField] private DisplayObjects objStatus; // 0. passlevel,1. curlevel;
    [SerializeField] private Button btn_Select;
    [Header("Customer")]
    [SerializeField] private int curLevel;
    private LevelMap levelMapPref;
    private void Awake() {
        btn_Select.onClick.AddListener(OnSelect);
    }

    public void Show(int level) {
        this.curLevel = level;
        levelMapPref = DataManager.Instance.GetlevelMapByLevel(level);
        if(levelMapPref == null) {
            gameObject.SetActive(false);
            return;
        }
        gameObject.SetActive(true);
        txt_Level.text = (curLevel+1).ToString();
        iconBoss.SetActive(levelMapPref.NumberEnemy == 1);
        int levelPlayer = DataManager.Instance.PlayerData.LevelMap;
        if(curLevel == DataManager.Instance.PlayerData.LevelMap) {
            objStatus.Active(1);
        }else if(curLevel < levelPlayer) {
            objStatus.Active(0);
        } else {
            objStatus.ActiveAll(false);
        }
    }

    private void OnSelect() {
        int levelMap = DataManager.Instance.PlayerData.LevelMap;
        if(curLevel <= levelMap) {
            //if(curLevel == levelMap) {
            //    if(Application.internetReachability == NetworkReachability.NotReachable) {
            //        FrameManager.Instance.Push<ConectFrame>();
            //        return;
            //    }
            //}
            GameManager.Instance.CurLevel = curLevel;
            SceneManagerLoad.Instance.LoadSceneAsyn(SceneManagerLoad.SCENE_GAME);
        } else {
            TextNotify.Instance.Show("Please pass the previous levels !!");
        }
    }
}

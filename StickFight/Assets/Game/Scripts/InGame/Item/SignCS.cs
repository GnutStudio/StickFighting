using UnityEngine;
using TMPro;
using STU;

public class SignCS : MonoBehaviour
{
    [SerializeField] private TextMeshPro txt_SumEnemy;
    [SerializeField] private TextMeshPro txt_CurEnemy;

    private LevelMap lvlMap;
    private int sumEnemy;
    private void Start()
    {
        sumEnemy = InGameManager.Instance.LevelMap.NumberEnemy;
        // sumEnemy = transform.parent.GetComponent<LevelMap>().NumberEnemy;
        txt_SumEnemy.text = sumEnemy.ToString();
        txt_CurEnemy.text = "0";
    }

    private void OnEnable() {
        EventDispatcher.AddListener<EventKey.EnemyDie>(HalderEvent);
    }

    private void OnDisable() {
        EventDispatcher.RemoveListener<EventKey.EnemyDie>(HalderEvent);
    }

    private void HalderEvent(EventKey.EnemyDie evt) {
        txt_CurEnemy.text = InGameManager.Instance.EnemyKilled.ToString();
    }
}

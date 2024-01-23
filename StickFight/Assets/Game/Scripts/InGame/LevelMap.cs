using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMap : MonoBehaviour
{
    [SerializeField] private int level;
    [SerializeField] private int numberEnemy;
    public int Level => level;
    public int NumberEnemy => numberEnemy;

    [ContextMenu("SetUpInfoMap")]
    public void SetUpInFoMap() {
        numberEnemy = transform.GetComponentsInChildren<EnemyBase>().Length;
    }

    [ContextMenu("SetupZeroz")]
    public void SetupZeroz() {
        foreach(Transform chil in transform) {
            chil.position = new Vector3(chil.position.x, chil.position.y, 0);
            SetUpAll(chil);
        }
    }

    private void SetUpAll(Transform transform) {
        foreach(Transform chil in transform) {
            chil.position = new Vector3(chil.position.x, chil.position.y, 0);
            SetUpAll(chil);
        }
    }

}

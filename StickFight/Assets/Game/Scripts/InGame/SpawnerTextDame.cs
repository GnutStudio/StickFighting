using STU;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerTextDame : Singleton<SpawnerTextDame> {
    [SerializeField] private TextDameInGame textPref;
    public void Spawner(Vector3 position, string content, TextDameInGame.TypeTextShow type = TextDameInGame.TypeTextShow.UP) {
        var textInGame = textPref.Spawn();
        textInGame.transform.SetParent(transform);
        textInGame.Show(content, position, type);
    }

    private void Update() {
        if(Input.GetKeyDown(KeyCode.K)) {
            Spawner(InGameManager.Instance.Player.transform.position, "+3", TextDameInGame.TypeTextShow.UP);
        }

        if(Input.GetKeyDown(KeyCode.L)) {
            Spawner(InGameManager.Instance.Player.transform.position, "+3", TextDameInGame.TypeTextShow.UP_DOWN);
        }
    }
}

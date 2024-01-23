using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShackParticle : MonoBehaviour
{
    private void OnEnable() {
        SoundManager.Instance.Vibrate();
    }
}

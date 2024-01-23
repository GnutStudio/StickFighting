using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class SoundButton : MonoBehaviour
{
    [SerializeField] private Button btn_Btn;

    private void Awake() {
        btn_Btn.onClick.AddListener(PlaySound);
    }

    private void PlaySound() {
        SoundManager.Instance.PlaySoundButton();
    }

    private void OnValidate() {
        btn_Btn = GetComponent<Button>();
        if(btn_Btn == null) {
            btn_Btn = gameObject.AddComponent<Button>();
        }
    }
}

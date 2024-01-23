using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Button))]
public class ButtonClose : MonoBehaviour {
    [SerializeField] private Button btn_Close;
    [SerializeField] private FrameBase frame;

    private void Awake()
    {
        btn_Close = GetComponent<Button>();
        btn_Close.onClick.AddListener(CloseFrame);
    }

    private void CloseFrame() 
    {
        if(frame!=null) {
            frame.Hide();
        }
    }

    private void OnValidate() 
    {
        if(btn_Close == null) {
            btn_Close = gameObject.AddComponent<Button>();
        }
    }
}

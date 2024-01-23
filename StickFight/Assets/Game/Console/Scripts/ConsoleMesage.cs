using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole.Log {
    internal class ConsoleMesage : MonoBehaviour {
        [SerializeField] private Image icon;
        [SerializeField] private Text message;

        System.Action<ConsoleMesage> onSelectedCallback;

        public string ShortMessage { get; private set; }
        public string FullMessage { get; private set; }

        public void Init(Sprite sp, ConsoleView.Log log, System.Action<ConsoleMesage> selectedCallback) {
            this.icon.sprite = sp;
            ShortMessage = string.Format("{0}: {1}", log.time, log.message);
            FullMessage = string.Format("{0}: {1}\n{2}", log.time, log.message, log.stackTrace);
            this.message.text = ShortMessage;
            //this.message.color = color;
            this.onSelectedCallback = selectedCallback;
        }

        public void OnSelect() {
            onSelectedCallback?.Invoke(this);
        }

        public void Show() {
            gameObject.SetActive(true);
        }

        public void Hide() {
            gameObject.SetActive(false);
        }

        public void RemoveSelf() {
            Destroy(gameObject);
        }
    }
}

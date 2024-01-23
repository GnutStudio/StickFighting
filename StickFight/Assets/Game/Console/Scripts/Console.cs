using STU;
using UnityEngine;

namespace IngameConsole {
    public class Console : Singleton<Console> {
        [SerializeField] private IngameConsole.Log.ConsoleView console;

        public void Show(bool maximize = false) {
            console.Show(maximize);
        }

        public void Hide() {
            console.Hide();
        }

        #region Check show
        private int clickCount = 0;
        private float finishTime = float.MaxValue;

        private void Update() {
            if (console.gameObject.activeInHierarchy)
                return;
            if (Input.GetMouseButtonUp(0)) {
                if (Time.realtimeSinceStartup < finishTime) {
                    clickCount++;

                    if (clickCount >= 5) {
                        Show();
                        clickCount = 0;
                    }
                }
                else {
                    clickCount = 1;
                }
                finishTime = Time.realtimeSinceStartup + 0.2f;
            }
        }
        #endregion



        [ContextMenu("TestShowNormalLog")]
        private void TestShowNormalLog() {
            Debug.Log("This is a log just for test normal");
        }
        [ContextMenu("TestShowWarningLog")]
        private void TestShowWarningLog() {
            Debug.LogWarning("This is a log just for test warning. This is a log just for test warning");
        }
        [ContextMenu("TestShowErrorLog")]
        private void TestShowErrorLog() {
            Debug.LogError("This is a log just for test error. This is a log just for test error. This is a log just for test error");
        }
    }
}
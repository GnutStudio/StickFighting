using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace IngameConsole.Log {
    internal partial class ConsoleView : MonoBehaviour {
        private bool showNormal = true, showError = true, showWarning = false; // This will be reverse on Start.
        private int totalLog = 0;

        void OnEnable() {
            ResetVariables();
            Application.logMessageReceived += RecordLog;
        }

        void OnDisable() {
            Application.logMessageReceived -= RecordLog;
        }

        public void Show(bool maximize = false) {
            gameObject.SetActive(true);
            if (maximize) ShowMaximize();
            else ShowMinimize();
        }

        public void Hide() {
            ClearAllLogs();
            gameObject.SetActive(false);
        }

        private void RecordLog(string message, string stackTrace, LogType type) {
            Log lg = new Log(message, stackTrace, type);
            OnRecordLog(lg);
        }

        /// <summary> TODO: update view, spawn a new message item. </summary>
        partial void OnRecordLog(Log log);

        #region define
        public struct Log {
            public string time;
            public string message;
            public string stackTrace;
            public LogType type;

            public Log(string message, string stackTrace, LogType type) {
                this.time = System.DateTime.Now.ToLongTimeString();
                this.message = message;
                this.stackTrace = stackTrace;
                this.type = type;
            }

            public bool IsNormal => type == LogType.Log;
            public bool IsWarning => type == LogType.Warning;
        }

        //readonly Dictionary<LogType, Color> logTypeColors = new Dictionary<LogType, Color>() {
        //{ LogType.Assert, Color.red },
        //{ LogType.Error, Color.red },
        //{ LogType.Exception, Color.red },
        //{ LogType.Log, Color.white },
        //{ LogType.Warning, Color.yellow },
        //};
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace IngameConsole.Log {
    internal partial class ConsoleView : MonoBehaviour {
        [SerializeField] private GameObject maximizePanel, minimumPanel;
        [Header("Resources")]
        [SerializeField] private GameObject msgPrefab;
        [SerializeField] private Sprite normalSprite, warningSprite, errorSprite;
        [Header("References Input")]
        [SerializeField] private ScrollRect scroll;
        [SerializeField] private RectTransform scrollContainer;
        [SerializeField] private Transform selectedBorder;
        [SerializeField] private Text detailView;
        [SerializeField] private InputField filter;
        [Header("Text info")]
        [SerializeField] private Text normalCountText;
        [SerializeField] private Text warningCountText, errorCountText;
        [SerializeField] private Text normalCountText2, warningCountText2, errorCountText2;

        private List<ConsoleMesage> normalMessages = new List<ConsoleMesage>();
        private List<ConsoleMesage> warningMessages = new List<ConsoleMesage>();
        private List<ConsoleMesage> errorMessages = new List<ConsoleMesage>();
        
        private void ResetVariables() {
            UpdateEnableState(normalCountText, showNormal);
            UpdateEnableState(warningCountText, showWarning);
            UpdateEnableState(errorCountText, showError);

            UpdateTextInfo(normalCountText, normalCountText2, 0);
            UpdateTextInfo(warningCountText, warningCountText2, 0);
            UpdateTextInfo(errorCountText, errorCountText2, 0);
        }

        private void OnSelectDetailMessage(ConsoleMesage item) {
            detailView.text = item.FullMessage;
            selectedBorder.gameObject.SetActive(true);
            selectedBorder.SetParent(item.transform);
            selectedBorder.SetAsFirstSibling();
            selectedBorder.localPosition = Vector3.zero;
        }

        partial void OnRecordLog(Log log) {
            totalLog++;
            ConsoleMesage msg = GameObject.Instantiate(msgPrefab, scrollContainer).GetComponent<ConsoleMesage>();
            msg.Init(log.IsNormal ? normalSprite : log.IsWarning ? warningSprite : errorSprite, log, OnSelectDetailMessage);

            if (log.IsNormal) {
                msg.gameObject.SetActive(showNormal);
                normalMessages.Add(msg);
                UpdateTextInfo(normalCountText, normalCountText2, normalMessages.Count);
            }
            else if (log.IsWarning) {
                msg.gameObject.SetActive(showWarning);
                warningMessages.Add(msg);
                UpdateTextInfo(warningCountText, warningCountText2, warningMessages.Count);
            }
            else {
                msg.gameObject.SetActive(showError);
                errorMessages.Add(msg);
                UpdateTextInfo(errorCountText, errorCountText2, errorMessages.Count);
            }

            //scroll.verticalNormalizedPosition = 0;
            scroll.verticalScrollbar.size = 0;
            scroll.verticalScrollbar.value = 0;

            if (totalLog % 2 == 0) {
                msg.GetComponent<Image>().enabled = false;
            }
        }
        
        private void UpdateTextInfo(Text countText1, Text countText2, int countValue) {
            string v = countValue < 1000 ? countValue.ToString() : "999+";
            countText1.text = v;
            countText2.text = v;
        }

        private void UpdateEnableState(Text textOfButton, bool show) {
            textOfButton.transform.parent.GetComponent<Image>().color = show ? Color.gray : Color.black;
        }

        private void UpdateItem(bool show, List<ConsoleMesage> list) {
            if (show) {
                foreach (var it in list) {
                    if (string.IsNullOrEmpty(filter.text) || it.ShortMessage.ToLower().Contains(filter.text.ToLower())) {
                        it.Show();
                    }
                    else it.Hide();
                }
            }
            else {
                foreach (var it in list) {
                    it.Hide();
                }
            }
        }

        public void OnFillterChangeCallback() {
            if (showNormal) UpdateItem(showNormal, normalMessages);
            if (showWarning) UpdateItem(showWarning, warningMessages);
            if (showError) UpdateItem(showError, errorMessages);
        }
        
        public void ShowMaximize() {
            minimumPanel.SetActive(false);
            maximizePanel.SetActive(true);
        }

        public void ShowMinimize() {
            var rect = minimumPanel.transform as RectTransform;
#if UNITY_EDITOR
            float screenW = UnityEditor.Handles.GetMainGameViewSize().x;
#else
            float screenW = Screen.width;
#endif
            rect.anchoredPosition = new Vector2(screenW/2 - rect.sizeDelta.x/2, 0);
            minimumPanel.SetActive(true);
            maximizePanel.SetActive(false);
        }

        public void OnDragMinize() {
            Vector2 mousePos = Input.mousePosition;
#if UNITY_EDITOR
            Vector2 screenHalfSize = UnityEditor.Handles.GetMainGameViewSize()/2;
#else
            Vector2 screenHalfSize = new Vector2(Screen.width/2, Screen.height/2);
#endif

            var radius = (minimumPanel.transform as RectTransform).sizeDelta/2;
            mousePos.x = Mathf.Clamp(mousePos.x - screenHalfSize.x, -screenHalfSize.x + radius.x, screenHalfSize.x - radius.x);
            mousePos.y = Mathf.Clamp(mousePos.y - screenHalfSize.y, -screenHalfSize.y + radius.y, screenHalfSize.y - radius.y);

            (minimumPanel.transform as RectTransform).anchoredPosition = mousePos;
        }

        public void ClearAllLogs() {
            totalLog = 0;
            selectedBorder.transform.SetParent(scrollContainer);
            selectedBorder.gameObject.SetActive(false);

            foreach (var lg in normalMessages) {
                lg.RemoveSelf();
            }
            foreach (var lg in warningMessages) {
                lg.RemoveSelf();
            }
            foreach (var lg in errorMessages) {
                lg.RemoveSelf();
            }

            normalMessages.Clear();
            warningMessages.Clear();
            errorMessages.Clear();

            UpdateTextInfo(normalCountText, normalCountText2, 0);
            UpdateTextInfo(warningCountText, warningCountText2, 0);
            UpdateTextInfo(errorCountText, errorCountText2, 0);
        }

        public void OnClickShowNormal() {
            showNormal = !showNormal;
            UpdateEnableState(normalCountText, showNormal);
            UpdateItem(showNormal, normalMessages);
        }

        public void OnClickShowWarning() {
            showWarning = !showWarning;
            UpdateEnableState(warningCountText, showWarning);
            UpdateItem(showWarning, warningMessages);
        }

        public void OnClickShowError() {
            showError = !showError;
            UpdateEnableState(errorCountText, showError);
            UpdateItem(showError, errorMessages);
        }
    }
}

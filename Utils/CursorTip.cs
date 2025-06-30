using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
namespace BubbleZun.Utils{
    public class CursorTip : MonoBehaviour
    {
        string currentId;
        public static CursorTip instance;
        private void Awake()
        {
            instance = this;
        }
        public TMP_Text tipText;
        public UnityEvent onTipShow;
        public UnityEvent onTipHide;
        public Vector2 offset;
        public void Update()
        {
            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = mousePos + offset;
            transform.position = Vector2.Lerp(transform.position, targetPos, 20f * Time.deltaTime);
        }
        static public void ShowTip(string text, string id)
        {
            if (instance == null) {
                Debug.Log("CursorTip instance is null");
                return;
            }
            instance.showTip(text, id);
        }
        static public void HideTip(string id)
        {
            if (instance == null) {
                Debug.Log("CursorTip instance is null");
                return;
            }
            instance.hideTip(id);
        }
        bool isShowing = false;
        void showTip(string text, string id)
        {
            if (text == tipText.text && isShowing) return;
            isShowing = true;
            tipText.text = text;
            currentId = id;
            onTipShow.Invoke();
        }
        void hideTip(string id)
        {
            if (id != currentId) return;
            isShowing = false;
            onTipHide.Invoke();
        }
    }
}
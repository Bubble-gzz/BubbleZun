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
            if (instance != null) {
                Destroy(gameObject);
                return;
            }
            instance = this;
        }
        public TMP_Text tipText;
        public int priority = 0;
        public UnityEvent onTipShow;
        public UnityEvent onTipHide;
        public Vector2 defaultOffset;
        [HideInInspector] public Vector2 offset;
        public bool updatePosAutomatically = true;
        public void Update()
        {
            UpdatePos();
        }
        void UpdatePos()
        {
            if (!updatePosAutomatically) return;
            Vector2 mousePos = Input.mousePosition;
            Vector2 targetPos = mousePos + offset;
            transform.position = Vector2.Lerp(transform.position, targetPos, 20f * Time.deltaTime);
        }
        static public void ShowTip(string text, string id, int priority = 0)
        {
            if (instance == null) {
                Debug.Log("CursorTip instance is null");
                return;
            }
            ShowTip(text, id, instance.defaultOffset, priority);
        }
        static public void ShowTip(string text, string id, Vector2 offset, int priority = 0)
        {
            if (instance == null) {
                Debug.Log("CursorTip instance is null");
                return;
            }
            instance.showTip(text, id, offset, priority);
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
        void showTip(string text, string id, Vector2 offset, int priority = 0)
        {
            this.offset = offset;
            if (isShowing)
            {
                if (text == tipText.text) return;
                if (priority < this.priority) return;
            }
            isShowing = true;
            tipText.text = text;
            currentId = id;
            this.priority = priority;
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

namespace BubbleZun.Utils
{
    public class CursorManager : MonoBehaviour
    {
        private static CursorManager _instance;
        public static CursorManager Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<CursorManager>();
                    if (_instance == null)
                    {
                        GameObject go = new GameObject("CursorManager");
                        _instance = go.AddComponent<CursorManager>();
                    }
                }
                return _instance;
            }
        }

        [System.Serializable]
        public class CursorInfo{
            public string name;
            public Sprite sprite;
            public Vector2 offset;
            public Vector2 size;
        }
        public List<CursorInfo> cursorInfos = new List<CursorInfo>();
        private GameObject canvasObj, cursorObj;
        private Image cursorImage;
        private Canvas cursorCanvas;
        CursorInfo currentCursorInfo;
        private void Awake()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this.gameObject);
                return;
            }
            else
            {
                _instance = this;
            }
            DontDestroyOnLoad(this.gameObject);
            Initialize();
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        private void OnDestroy()
        {
            if (_instance == this)
            {
                _instance = null;
            }
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }

        void Initialize()
        {
            InitializeCursorSprite();
            SetCursor("default");
        }

        private void InitializeCursorSprite()
        {
            // 创建 Canvas
            canvasObj = new GameObject("CursorCanvas");
            cursorCanvas = canvasObj.AddComponent<Canvas>();
            cursorCanvas.renderMode = RenderMode.ScreenSpaceOverlay;
            cursorCanvas.sortingOrder = 2;
            
            // 添加 CanvasScaler
            CanvasScaler scaler = canvasObj.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1920, 1080);
            
            // 创建光标
            cursorObj = new GameObject("CursorSprite");
            cursorObj.transform.SetParent(canvasObj.transform, false);
            cursorImage = cursorObj.AddComponent<Image>();
            cursorImage.raycastTarget = false;
            DontDestroyOnLoad(canvasObj);
        }

        private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
        {
            // 确保在场景加载后光标仍然存在
            if (cursorObj == null || cursorImage == null)
            {
                InitializeCursorSprite();
                SetCursor("default");
            }
        }
        private void OnApplicationQuit()
        {
            // Unity退出Play模式时会调用这个方法
            if (canvasObj != null) Destroy(canvasObj);
            if (cursorObj != null) Destroy(cursorObj);
            Destroy(gameObject);
        }
        private void SetCursor(string name)
        {
            CursorInfo cursorInfo = cursorInfos.Find(info => info.name == name);
            if (cursorInfo == null) {
                Debug.LogError("CursorInfo not found: " + name);
                return;
            }
            if (cursorInfo.sprite == null) {
                cursorImage.enabled = false;
            } else {
                cursorImage.sprite = cursorInfo.sprite;
                cursorImage.enabled = true;
            }
            currentCursorInfo = cursorInfo;
            UpdateCursorSize();
        }

        private void UpdateCursorSize()
        {
            if (cursorObj == null) return;
            RectTransform rectTransform = cursorObj.GetComponent<RectTransform>();
            if (rectTransform == null) return;
            rectTransform.sizeDelta = currentCursorInfo.size;
        }

        private void Update()
        {
            UpdateCursorPos();
        }

        void UpdateCursorPos()
        {
            cursorObj.transform.position = Input.mousePosition + (Vector3)currentCursorInfo.offset;
        }
    }
}
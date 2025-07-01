using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
namespace BubbleZun.Utils{
    public class NotificationSystem : MonoBehaviour
    {
        private static NotificationSystem instance;
        public static NotificationSystem Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = FindObjectOfType<NotificationSystem>();
                    if (instance == null)
                    {
                        GameObject go = new GameObject("BubbleNotificationSystem");
                        instance = go.AddComponent<NotificationSystem>();
                    }
                }
                return instance;
            }
        }

        private Canvas notificationCanvas;

        void Awake()
        {
            if (instance != null && instance != this)
            {
                Destroy(gameObject);
                return;
            }
            instance = this;
            transform.position = Vector3.zero;

            // Create notification canvas
            notificationCanvas = CanvasManager.CreateCanvas("NotificationCanvas", 10, transform);
        }

        public GameObject toastMessagePrefab;
        public UnityEvent onNewMessageAppear;

        static public void ShowToastMessage(string message, ToastMessage.Type type = ToastMessage.Type.OK, float duration = 1.5f)
        {
            Instance.onNewMessageAppear.Invoke();
            ToastMessage toastMessage = Instantiate(Instance.toastMessagePrefab, Instance.notificationCanvas.transform).GetComponent<ToastMessage>();
            toastMessage.SetMessage(message);
            toastMessage.SetType(type);
            toastMessage.duration = duration;
            Instance.onNewMessageAppear.AddListener(() => toastMessage.FadeOut(true));

        }
    }

}

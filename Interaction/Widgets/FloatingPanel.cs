using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class FloatingPanel : MonoBehaviour
{
    // Start is called before the first frame update
    public UnityEvent onShow;
    public UnityEvent onHide;
    public bool isShown = false;

    public void Show()
    {
        if (isShown) return;
        isShown = true;
        onShow?.Invoke();
        StartCoroutine(UntilHide());
    }

    IEnumerator UntilHide()
    {
        while (true)
        {
            yield return null;
            if (Input.GetMouseButtonDown(0)) break;
        }
        isShown = false;
        onHide?.Invoke();
    }
}

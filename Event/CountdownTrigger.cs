using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
namespace BubbleZun.Event
{
    public class CountdownTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        public float countdownTime = 0.2f;
        public UnityEvent onCountdownEnd;
        public void StartCountdown()
        {
            StartCoroutine(CountdownCoroutine());
        }

        private IEnumerator CountdownCoroutine()
        {
            yield return new WaitForSeconds(countdownTime);
            onCountdownEnd.Invoke();
        }
    }
}
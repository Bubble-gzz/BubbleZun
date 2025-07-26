using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
namespace BubbleZun.Interaction.Widgets {
    public class ProgressBar : MonoBehaviour
    {
        public float progress;
        public Image progressBar;
        [SerializeField] float lerpSpeed = 10;
        public void SetProgress(float progress) {
            this.progress = progress;
        }
        void Update() {
            if (progressBar != null) {
                progressBar.fillAmount = Mathf.Lerp(progressBar.fillAmount, progress, Time.deltaTime * lerpSpeed);
            }
        }
    }
}
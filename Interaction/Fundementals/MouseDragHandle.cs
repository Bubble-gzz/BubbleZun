using System.Collections;
using System.Collections.Generic;
using BubbleZun.Interaction;
using UnityEngine;

namespace BubbleZun.Interaction{
    public class MouseDragHandle : MouseInteractable
    {
        // Update is called once per frame
        bool isDragging = false;
        MouseDetector mouseDetector;
        public bool useLMB = true;
        public bool useRMB = false;
        public override void GenerateComponents(){
            if (generateComponentsDone) return;
            base.GenerateComponents();
            InstallComponent(ref mouseDetector);
        }
        void Update()
        {
            if (!IsInteractable()) return;
            InteractionCheck();
            MoveObject();
        }
        void InteractionCheck()
        {
            if (!isDragging)
            {
                if (mouseDetector.isMouseOver)
                {
                    if (useLMB && Input.GetMouseButtonDown(0)) isDragging = true;
                    if (useRMB && Input.GetMouseButtonDown(1)) isDragging = true;
                }
            }
            else
            {
                if (!(useLMB && Input.GetMouseButton(0)) && !(useRMB && Input.GetMouseButton(1))) isDragging = false;
            }
        
        }
        Vector3 lastMousePosition;
        void MoveObject()
        {
            if (!isDragging) return;
            Vector3 mousePosition = Input.mousePosition;
            if (!useScreenSpace) mousePosition = Camera.main.ScreenToWorldPoint(mousePosition);
            Vector3 delta = mousePosition - lastMousePosition;
            transform.position += delta;
            lastMousePosition = mousePosition;
        }
    }
}
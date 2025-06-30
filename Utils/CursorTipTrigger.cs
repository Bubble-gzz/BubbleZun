using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BubbleZun.Utils;
namespace BubbleZun.Utils{
    public class CursorTipTrigger : MonoBehaviour
    {
        // Start is called before the first frame update
        public string id;
        [System.Serializable]
        public class TipData{
            public string text;
            public bool useDefaultOffset = true;
            public Vector2 offset;
        }
        public List<TipData> tips;
        public void TriggerTip(int index)
        {
            if (index < 0 || index >= tips.Count) return;
        
            if (tips[index].useDefaultOffset)
            {
                CursorTip.ShowTip(tips[index].text, id);
            }
            else
            {
                CursorTip.ShowTip(tips[index].text, id, tips[index].offset);
            }
        }
        public void HideTip()
        {
            CursorTip.HideTip(id);
        }
    }
}
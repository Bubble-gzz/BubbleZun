using UnityEngine;
namespace BubbleZun.Utils{
    public static class BDebug{
        static int debugCount = 0;
        public static void Log(string message, bool showTime = true, bool showDebugCount = false){
            string timeString = showTime ? "[" + Time.time + "] " : "";
            string debugCountString = showDebugCount ? "[" + debugCount + "] " : "";
            Debug.Log(timeString + debugCountString + message);
            debugCount++;
        }
    }
}
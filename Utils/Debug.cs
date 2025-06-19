using UnityEngine;
namespace BubbleZun.Utils{
    public static class BDebug{
        public static void Log(string message){
            Debug.Log("[" + Time.time + "] " + message);
        }
    }
}
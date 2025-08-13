using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleZun.Utils{
    public static class GameObjectExtensions
    {
        public static GameObject FindInChildren(this GameObject go, string name, bool caseSensitive = false, bool ignoreSpace = true)
        {
            // 查找所有子物体并遍历
            foreach (Transform child in go.transform)
            {
                string childName = ignoreSpace ? child.gameObject.name.Replace(" ", "") : child.gameObject.name;
                if (childName == name || (!caseSensitive && childName.ToLower() == name.ToLower()))
                {
                    return child.gameObject;
                }

                // 递归查找子物体的子物体
                GameObject found = child.gameObject.FindInChildren(name, caseSensitive, ignoreSpace);
                if (found != null)
                {
                    return found;
                }
            }
            return null;
        }
    }
}
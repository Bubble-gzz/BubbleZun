using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace BubbleZun.Utils
{
    public interface IObjectPoolable{
        ObjectPool pool{get; set;}
    }
    public class ObjectPool
    {
        GameObject prefab;
        List<GameObject> pool = new List<GameObject>();
        public ObjectPool(GameObject prefab)
        {
            this.prefab = prefab;
        }
        public GameObject GetObject(Transform parent)
        {
            GameObject obj = null;
            while (obj == null && pool.Count > 0)
            {
                obj = pool[pool.Count - 1];
                pool.RemoveAt(pool.Count - 1);
            }
            if (obj == null)
            {
                obj = GameObject.Instantiate(prefab);
                obj.GetComponent<IObjectPoolable>().pool = this;
            }
            obj.transform.SetParent(parent);
            obj.SetActive(true);
            return obj;
        }
        public void Recycle(GameObject obj)
        {
            obj.SetActive(false);
            pool.Add(obj);
        }
    }
}
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
        Transform root;
        public ObjectPool(GameObject prefab, int initialSize = 0, Transform root = null)
        {
            this.prefab = prefab;
            this.root = root;
            for (int i = 0; i < initialSize; i++)
            {
                GameObject obj = GameObject.Instantiate(prefab, root);
                obj.SetActive(false);
                pool.Add(obj);
            }
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
                obj = GameObject.Instantiate(prefab, root);
                IObjectPoolable poolable = obj.GetComponent<IObjectPoolable>();
                if (poolable != null) poolable.pool = this;
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
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T> : MonoBehaviour
    where T : Component
{
    [SerializeField]
    protected T prefab;
    [SerializeField]
    protected List<T> pool;
    [SerializeField]
    protected int initialSize;
    [SerializeField]
    protected Transform parentTransform;

    public void Create()
    {
        pool = new List<T>();

        for (int i = 0; i < initialSize; i++)
        {
            T newObj = Instantiate(prefab, parentTransform);
            Add(newObj);
        }
    }

    public virtual void Add(T obj)
    {
        obj.transform.SetParent(parentTransform);
        pool.Add(obj);
        obj.gameObject.SetActive(false);
    }

    public virtual T Pick()
    {
        if (pool.Count > 0)
        {
            T itm = pool[pool.Count - 1];
            pool.RemoveAt(pool.Count - 1);
            itm.gameObject.SetActive(true);
            return itm;
        }
        else 
        {
            // If no object is found, create a new one
            return Instantiate(prefab);
        }
    }

    public virtual void Clear() 
    {
        foreach (var item in pool) { Destroy(item.gameObject); }
        pool.Clear();
    }
}

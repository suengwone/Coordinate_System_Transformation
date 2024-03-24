using System.Collections.Generic;
using UnityEngine;

public class ObjectPool<T>
    where T : Component
{
    private Stack<T> pool;
    private GameObject objectPool;

    public ObjectPool(int count = 0)
    {
        objectPool = new GameObject("ObjectPool");
        pool = new Stack<T>();

        SetObject(count);
    }

    public List<T> GetObject(int count)
    {
        if(pool.Count < count)
        {
            SetObject(count - pool.Count);
        }

        List<T> result = new();

        for(int i=0; i<count; i++)
        {
            var temp = pool.Pop();
            result.Add(temp);
        }

        return result;
    }

    private void SetObject(int count)
    {
        while(count != 0)
        {
            T visualObject = GameObject.CreatePrimitive(PrimitiveType.Sphere).GetComponent<T>();
            visualObject.transform.localScale = Vector3.one * 0.05f;
            visualObject.transform.SetParent(objectPool.transform);

            visualObject.gameObject.SetActive(false);
            pool.Push(visualObject);
            count--;
        };
    }

    public void ReturnObject(ref IEnumerable<T> objects)
    {
        foreach(var obj in objects)
        {
            obj.transform.SetParent(objectPool.transform);
            obj.gameObject.SetActive(false);
            pool.Push(obj);
        }
    }
}

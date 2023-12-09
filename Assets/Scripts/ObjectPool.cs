using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPool : MonoBehaviour
{
    private List<GameObject> pool = new List<GameObject>();
    [SerializeField] GameObject[] objectTypes;
    [SerializeField] float[] objectQuantities;


    public static ObjectPool objectPool;

    private void Awake()
    {
        if (objectPool == null) objectPool = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < Mathf.Min(objectTypes.Length, objectQuantities.Length); i++)
        {
            for (int j = 0; j < objectQuantities[i]; j++)
            {
                GameObject obj = Instantiate(objectTypes[i]);
                obj.name = objectTypes[i].name;
                obj.SetActive(false);
                pool.Add(obj);
                obj = null;
            }
        }
    }

    //Gets the first object available in the pool
    public GameObject GetObject(GameObject typeObject)
    {
        for (int i = 0; i < pool.Count; i++)
        {
            if (pool[i].name == typeObject.name && !pool[i].activeInHierarchy) return pool[i];
        }
        GameObject obj = Instantiate(typeObject);
        obj.name = typeObject.name;
        obj.SetActive(false);
        pool.Add(obj);
        return obj;
    }
}

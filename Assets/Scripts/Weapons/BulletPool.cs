using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class BulletPool : MonoBehaviour
{
    [SerializeField] GameObject m_ObjectToPool;
    [SerializeField] bool m_CollectionCheck = true;
    [SerializeField] int m_MaxPoolSize = 50;
    [SerializeField] PoolType m_PoolType;
    IObjectPool<GameObject> pool;

    public IObjectPool<GameObject> Pool
    {
        get
        {
            if (pool == null)
            {
                if (m_PoolType == PoolType.Stack)
                {
                    pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_CollectionCheck, 10, m_MaxPoolSize);
                }
                if (m_PoolType == PoolType.LinkedList)
                {
                    pool = new ObjectPool<GameObject>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, m_CollectionCheck, m_MaxPoolSize);
                }
                
            }
            return pool;
        }
    }

    private void Awake()
    {
        WeaponsSingleton.Instance.RegisterBulletPool(Pool);
    }

    private void OnDestroyPoolObject(GameObject @object)
    {
        Destroy(@object);
    }

    private void OnReturnedToPool(GameObject @object)
    {
        @object.SetActive(false);
    }

    private void OnTakeFromPool(GameObject @object)
    {
        @object.SetActive(true);
    }

    private GameObject CreatePooledItem()
    {
        var pooledBullet = Instantiate<GameObject>(m_ObjectToPool, transform);
        var returnToPool = pooledBullet.AddComponent<ReturnToPool>();
        returnToPool.Pool = pool;
        pooledBullet.SetActive(false);
        return pooledBullet;
    }
}

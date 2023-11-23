using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class ReturnToPool : MonoBehaviour
{
    public IObjectPool<GameObject> Pool;
    
    public void SendToPool()
    {
        Pool.Release(gameObject);
    }
}

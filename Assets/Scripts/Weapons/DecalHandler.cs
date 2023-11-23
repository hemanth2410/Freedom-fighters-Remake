using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
public class DecalHandler : MonoBehaviour
{

    float timer = 0.0f;
    float timeToLive = 0.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    public void Setup(float timeToLive)
    {
        timer = 0.0f;
        this.timeToLive = timeToLive;
    }
    // Update is called once per frame
    void Update()
    {
        if(timer >= timeToLive)
        {
            GetComponent<ReturnToPool>().SendToPool();
        }
        timer += Time.deltaTime;
    }
}

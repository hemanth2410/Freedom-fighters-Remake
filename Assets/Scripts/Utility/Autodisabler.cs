using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Autodisabler : MonoBehaviour
{
    [SerializeField] bool m_OneFrameOperation;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(destroyAfter(m_OneFrameOperation));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator destroyAfter(bool oneFrame)
    {
        if(oneFrame)
        {
            yield return new WaitForEndOfFrame();
            Destroy(this.gameObject);
        }
        else
        {
            yield return new WaitForSecondsRealtime(time);
            Destroy(this.gameObject);
        }
    }
}

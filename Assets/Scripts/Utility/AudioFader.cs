using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioFader : MonoBehaviour
{
    [SerializeField] float m_effectDuration;
    [SerializeField] float m_volume;
    float effectTimer;
    AudioSource source;
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void OnEnable()
    {
        source = GetComponent<AudioSource>();
        source.Play();
        effectTimer = m_effectDuration;
        effectTimer += 0.85f;
    }
    // Update is called once per frame
    void Update()
    {
        m_volume = Mathf.Lerp(0.0f, 1.0f, (effectTimer/m_effectDuration));
        source.volume = m_volume;
        effectTimer -= Time.deltaTime;
        if(effectTimer <= 0.0f)
        {
            source.Stop();
            effectTimer = 0.0f;
        }
    }
}

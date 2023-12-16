using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ImageFader : MonoBehaviour
{
    [SerializeField] float m_effectDuration;
    float effectTimer;
    Image image;
    Color fullcolor = Color.white;
    Color fadeColor;
    // Start is called before the first frame update
    void Start()
    {

    }
    private void OnEnable()
    {
        image = GetComponent<Image>();
        effectTimer = m_effectDuration;
        fadeColor = new Color(1, 1, 1, 0);
    }
    // Update is called once per frame
    void Update()
    {
       image.color = Color.Lerp(fadeColor, fullcolor, (effectTimer/m_effectDuration));
        effectTimer -= Time.deltaTime;
        if (effectTimer <= 0.0f)
        {
            effectTimer = 0.0f;
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Recoil : MonoBehaviour
{
    Texture2D recoilTexture;
    // Start is called before the first frame update
    void Start()
    {
        WeaponsSingleton.Instance.RegisterRecoilProcessor(this);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SetupTextureRecoil(Texture2D recoilPattern)
    {
        this.recoilTexture = recoilPattern;
    }
    public Vector3 GetRecoil(float normalizedTime)
    {
        normalizedTime = Mathf.Clamp01(normalizedTime);
        Vector2 halfSize = new Vector2(recoilTexture.width / 2.0f, recoilTexture.height / 2.0f);
        int halfSquareExtents = Mathf.CeilToInt(
            Mathf.Lerp(0.01f, halfSize.x, normalizedTime));
        int minX = Mathf.FloorToInt(halfSize.x) - halfSquareExtents;
        int minY = Mathf.FloorToInt(halfSize.y) - halfSquareExtents;
        Color[] sampleColors = recoilTexture.GetPixels(minX, minY, halfSquareExtents * 2, halfSquareExtents * 2);
        float[] colorsAsGrey = System.Array.ConvertAll(sampleColors, (color) => color.grayscale);
        float totalGreyValue = colorsAsGrey.Sum();
        float grey = Random.Range(0, totalGreyValue);
        int i = 0;
        for (; i < colorsAsGrey.Length;i++)
        {
            grey -= colorsAsGrey[i];
            if(grey<=0)
            {
                break;
            }
        }
        int x = minX + i % (halfSquareExtents * 2);
        int y = minY + i / (halfSquareExtents * 2);
        Vector2 targetPosition = new Vector2(x, y);
        Vector2 direction = (targetPosition - halfSize) / halfSize.x;
        return direction;
    }
}

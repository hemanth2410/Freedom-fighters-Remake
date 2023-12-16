using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.HighDefinition;
using UnityEngine.UI;

public class FlashbangImageHelper : MonoBehaviour
{
    [SerializeField] float m_fadeTimer;
    float resetTimer;
    Image image;
    Color fullcolor = Color.white;
    Color fadeColor;
    Texture2D screenShot;
    Camera mainCamera;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    private void Start()
    {
        
    }
    private void OnEnable()
    {
        fadeColor = new Color(1, 1, 1, 0);
        resetTimer = m_fadeTimer;
        image = GetComponent<Image>();
        // wait for one frame and do
        StartCoroutine(captureScreen());
    }
    
    IEnumerator captureScreen()
    {
        yield return new WaitForEndOfFrame();
        screenShot = RTImage(Screen.currentResolution.width, Screen.currentResolution.height);
        image.sprite = Sprite.Create(screenShot,new Rect(0,0,screenShot.width,screenShot.height), new Vector2(0.5f,0.5f));
    }

    private Texture2D RTImage(int mWidth, int mHeight)
    {
        //mainCamera = Camera.main;
        //Rect rect = new Rect(0, 0, mWidth, mHeight);
        //RenderTexture renderTexture = new RenderTexture(mWidth, mHeight, 24);
        //Texture2D screenShot = new Texture2D(mWidth, mHeight, TextureFormat.RGBA32, false);
        //mainCamera.targetTexture = renderTexture;
        //mainCamera.Render();

        //RenderTexture.active = renderTexture;
        //screenShot.ReadPixels(rect, 0, 0);

        //mainCamera.targetTexture = null;
        //RenderTexture.active = null;

        //Destroy(renderTexture);
        //renderTexture = null;
        //return screenShot;
        // Create a new Texture2D with the specified width and height
        Texture2D screenshotTexture = new Texture2D(mWidth, mHeight, TextureFormat.RGB24, false);
        screenshotTexture = ScreenCapture.CaptureScreenshotAsTexture();
        return screenshotTexture;
    }
    // Update is called once per frame
    void Update()
    {
        if(image == null)
        {
            image = GetComponent<Image>();
        }
        image.color = Color.Lerp(fadeColor, fullcolor, (resetTimer/m_fadeTimer));
        resetTimer -= Time.deltaTime;
        if(resetTimer <= 0.0f)
            resetTimer = 0.0f;
    }
}

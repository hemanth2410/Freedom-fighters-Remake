using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using System.IO;
using System;
using MacFsWatcher;

public class TextureConverterEditor : EditorWindow
{
    Texture2D m_RedChannel;
    Texture2D m_GreenChannel;
    Texture2D m_BlueChannel;
    Texture2D m_AlphaChannel;
    Texture2D m_MaskMap;
    string m_MapName;
    string m_OutputDirectory;
    bool invertAlphaChannel = true;
    string error;
    Vector2 m_TextureSize;
    string saveDirectory;
    string fileName;

    [MenuItem("Tools/TextureConverter")]

    public static void ShowWindow()
    {
        //Show existing window instance. If one doesn't exist, make one.
        EditorWindow.GetWindow(typeof(TextureConverterEditor),false, "Aquena Texture Processor");
    }
    void OnGUI()
    {

        GUILayout.Label("Base Settings, Please provide textures of same size", EditorStyles.boldLabel);
        saveDirectory = EditorGUILayout.TextField("Directory", saveDirectory);
        fileName = EditorGUILayout.TextField("FileName", fileName);

        GUILayout.BeginHorizontal();
        m_RedChannel = TextureField("Red Channel", m_RedChannel);
        if(m_RedChannel != null)
        {
            m_TextureSize = new Vector2(m_RedChannel.width, m_RedChannel.height);
        }
        m_GreenChannel = TextureField("Green Channel", m_GreenChannel);
        if(m_BlueChannel == null)
        {
            m_BlueChannel = Texture2D.blackTexture;
        }
        m_BlueChannel = TextureField("Blue Channel", m_BlueChannel);
        m_AlphaChannel = TextureField("Alpha Channel", m_AlphaChannel);
        GUILayout.EndHorizontal();
        GUILayout.Label("Output Texture size : " + m_TextureSize);
        invertAlphaChannel = EditorGUILayout.Toggle("Invert alpha channel", invertAlphaChannel);
        if(GUILayout.Button("Generate Mask map"))
        {
            try
            {
                m_MaskMap = RenderOutputTexture(m_RedChannel, m_GreenChannel, m_BlueChannel, m_AlphaChannel);

                //if (m_AlphaChannel == null || m_RedChannel == null || m_GreenChannel == null)
                //{
                //    error = "One of the required textures are missing";
                //    return;
                //}
                //m_MaskMap = ConvertToMask(m_RedChannel, m_GreenChannel, m_BlueChannel, m_AlphaChannel, m_TextureSize);
            }
            catch (Exception e)
            {
                error = e.Message;
            }
            
        }
        GUILayout.Label("Preview", EditorStyles.boldLabel);
        if(m_MaskMap != null)
        {
            EditorGUILayout.BeginVertical();
            m_MaskMap = TextureFieldPreview("Output", m_MaskMap);
            EditorGUILayout.EndVertical();
        }
        if (GUILayout.Button("Save Texture to disk"))
        {
            if(m_MaskMap == null || string.IsNullOrEmpty(saveDirectory) || string.IsNullOrEmpty(fileName))
            {
                error = "one or more errors occured, make sure you have valid filepath and texture available";
                return;
            }
            string filepath = saveDirectory + "\\" + fileName + ".png";
            byte[] textureData = m_MaskMap.EncodeToPNG();
            File.WriteAllBytes(filepath, textureData);

            Debug.Log("Texture saved as PNG: " + filepath);
        }
        GUILayout.Label(error, EditorStyles.boldLabel);
    }

    private Texture2D TextureField(string name, Texture2D texture)
    {
        
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleLeft;
        style.fixedWidth = 100;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));
        GUILayout.EndVertical();
        return result;
    }
    private Texture2D TextureFieldPreview(string name, Texture2D texture)
    {
        GUILayout.BeginVertical();
        var style = new GUIStyle(GUI.skin.label);
        style.alignment = TextAnchor.MiddleLeft;
        style.fixedWidth = 512;
        GUILayout.Label(name, style);
        var result = (Texture2D)EditorGUILayout.ObjectField(texture, typeof(Texture2D), false, GUILayout.Width(512), GUILayout.Height(512));
        GUILayout.EndVertical();
        return result;
    }
    private Texture2D ConvertToMask(Texture2D redChannel, Texture2D greenChannel, Texture2D blueChannel, Texture2D AlphaChannel, Vector2 size)
    {
        try
        {
            Texture2D result = new Texture2D((int)size.x, (int)size.y);
            for (int i = 0; i < size.x; i++)
            {
                for (int j = 0; j < size.y; j++)
                {
                    float r = redChannel.GetPixel(i, j).r;
                    float g = greenChannel.GetPixel(i, j).r;
                    float b = blueChannel.GetPixel(i, j).r;
                    float a = AlphaChannel.GetPixel(i, j).r;
                    UnityEngine.Color _color = new UnityEngine.Color(r,g,b,a);
                    result.SetPixel(i, j, _color);
                }
            }
            return result;
        }
        catch(Exception ex)
        {
            Debug.LogError(ex.Message);
            return null;
        }
        
    }

    Texture2D RenderOutputTexture(Texture2D redTexture, Texture2D greenTexture, Texture blueTexture, Texture2D alphaTexture)
    {

        // Create a new material using the desired shader
        Shader shader = Shader.Find("Custom/TextureCombine");
        Material previewMaterial = new Material(shader);

        // Set the textures as properties in the material
        previewMaterial.SetTexture("_RedTexture", redTexture);
        previewMaterial.SetTexture("_GreenTexture", greenTexture);
        previewMaterial.SetTexture("_BlueTexture", blueTexture);
        previewMaterial.SetTexture("_AlphaTexture", alphaTexture);
        previewMaterial.SetFloat("_Invert", invertAlphaChannel ? 1.0f : 0.0f);
        RenderTexture outputRenderTexture = new RenderTexture((int)m_TextureSize.x, (int)m_TextureSize.y, 0);
        RenderTexture.active = outputRenderTexture;

        // Render the shader output to the RenderTexture
        Graphics.Blit(null, outputRenderTexture, previewMaterial);

        // Read the pixel data from the RenderTexture and create a Texture2D
        Texture2D outputTexture = new Texture2D((int)m_TextureSize.x, (int)m_TextureSize.y);
        outputTexture.ReadPixels(new Rect(0, 0, (int)m_TextureSize.x, (int)m_TextureSize.y), 0, 0);
        outputTexture.Apply();

        // Clean up
        RenderTexture.active = null;
        outputRenderTexture.Release();
        DestroyImmediate(outputRenderTexture);
        return outputTexture;
    }
}

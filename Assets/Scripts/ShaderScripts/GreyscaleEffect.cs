using UnityEngine;

public class GreyscaleEffect : MonoBehaviour
{
    #region Variablen
    private Shader curShader;
    [Range(0f, 1f)]
    public float greyscaleAmount = 0.0f;
    private Material curMaterial;
    #endregion

    #region Properties
    Material material
    {
        get
        {
            if (curMaterial == null)
            {
                curMaterial = new Material(curShader);
                curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return curMaterial;
        }
    }
    #endregion

    void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        curShader = GameObjectBank.instance.greyscaleShader;

        if (!curShader || !curShader.isSupported)
            enabled = false;
    }

    void OnRenderImage(RenderTexture sourceTex, RenderTexture destTex)
    {
        if (curShader)
        {
            material.SetFloat("_LuminosityAmount", greyscaleAmount);
            Graphics.Blit(sourceTex, destTex, material);
        }
        else
            Graphics.Blit(sourceTex, destTex);
    }

    void OnDisable()
    {
        if (curMaterial)
            DestroyImmediate(curMaterial);
    }

}
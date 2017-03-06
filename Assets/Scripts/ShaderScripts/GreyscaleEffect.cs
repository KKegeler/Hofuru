using UnityEngine;

public class GreyscaleEffect : MonoBehaviour
{
    #region Variables
    private Shader _curShader;
    [SerializeField] [Range(0f, 1f)]
    private float _greyscale;
    private Material _curMaterial;
    #endregion

    #region Properties
    private Material CurMaterial
    {
        get
        {
            if (_curMaterial == null)
            {
                _curMaterial = new Material(_curShader);
                _curMaterial.hideFlags = HideFlags.HideAndDontSave;
            }
            return _curMaterial;
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

        _curShader = GameObjectBank.Instance.greyscaleShader;

        if (!_curShader || !_curShader.isSupported)
            enabled = false;
    }

    void OnRenderImage(RenderTexture sourceTex, RenderTexture destTex)
    {
        if (_curShader)
        {
            CurMaterial.SetFloat("_LuminosityAmount", _greyscale);
            Graphics.Blit(sourceTex, destTex, CurMaterial);
        }
        else
            Graphics.Blit(sourceTex, destTex);
    }

    void OnDisable()
    {
        if (_curMaterial)
            DestroyImmediate(_curMaterial);
    }

}
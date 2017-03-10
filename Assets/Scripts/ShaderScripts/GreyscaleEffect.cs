using UnityEngine;
using System.Collections;

/// <summary>
/// Graustufeneffekt bei Tod des Spielers
/// </summary>
public class GreyscaleEffect : MonoBehaviour
{
    #region Variables
    private static GreyscaleEffect _instance;

    private Shader _shader;
    [Range(0f, 1f)]
    private float _greyscale = 0;
    [SerializeField] [Range(0.5f, 3f)]
    private float _blendTime = 0.5f;
    private Material _mat;
    #endregion

    #region Properties
    public static GreyscaleEffect Instance
    {
        get { return _instance; }
    }

    private Material Mat
    {
        get
        {
            if (_mat == null)
            {
                _mat = new Material(_shader);
                _mat.hideFlags = HideFlags.HideAndDontSave;
            }
            return _mat;
        }
    }
    #endregion

    private void Awake()
    {
        if (!_instance)
            _instance = this;
        else if (_instance != this)
            Destroy(gameObject);
    }

    private void Start()
    {
        if (!SystemInfo.supportsImageEffects)
        {
            enabled = false;
            return;
        }

        _shader = GameObjectBank.Instance.greyscaleShader;

        if (!_shader || !_shader.isSupported)
            enabled = false;
    }

    private void OnRenderImage(RenderTexture sourceTex, RenderTexture destTex)
    {
        if (_shader)
        {
            Mat.SetFloat("_LuminosityAmount", _greyscale);
            Graphics.Blit(sourceTex, destTex, Mat);
        }
        else
            Graphics.Blit(sourceTex, destTex);
    }

    public void BlendToGrey()
    {
        StartCoroutine(Blend());
    }

    private IEnumerator Blend()
    {
        while (_greyscale < 1f)
        {
            _greyscale += Time.deltaTime / _blendTime;
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        if (_mat)
            DestroyImmediate(_mat);
    }

}
using UnityEngine;
using System.Collections;

public class GreyscaleEffect : MonoBehaviour
{
    #region Variables
    private static GreyscaleEffect _instance;

    private Shader _curShader;
    [Range(0f, 1f)]
    private float _greyscale = 0;
    [SerializeField] [Range(0.5f, 3f)]
    private float _blendTime = 1;
    private Material _curMaterial;
    #endregion

    #region Properties
    public static GreyscaleEffect Instance
    {
        get { return _instance; }
    }

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

        _curShader = GameObjectBank.Instance.greyscaleShader;

        if (!_curShader || !_curShader.isSupported)
            enabled = false;
    }

    private void OnRenderImage(RenderTexture sourceTex, RenderTexture destTex)
    {
        if (_curShader)
        {
            CurMaterial.SetFloat("_LuminosityAmount", _greyscale);
            Graphics.Blit(sourceTex, destTex, CurMaterial);
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
            _greyscale = Mathf.Clamp01(_greyscale);
            yield return new WaitForEndOfFrame();
        }
    }

    private void OnDisable()
    {
        if (_curMaterial)
            DestroyImmediate(_curMaterial);
    }

}
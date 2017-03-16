using UnityEngine;

/// <summary>
/// Dynamic background
/// </summary>
public class Parallax : MonoBehaviour
{
    #region Variables
#pragma warning disable 0649
    [SerializeField]
    private Transform _bgCam;
    [SerializeField]
    private Transform _levelStart;
    [SerializeField]
    private Transform _levelEnd;
    [SerializeField]
    private Transform _bgStart;
    [SerializeField]
    private Transform _bgEnd;
    [SerializeField]
    private Transform _layer1;
    [SerializeField]
    private Transform _layer2;
    [SerializeField]
    private Transform _layer3;
    [SerializeField]
    private Transform _layer4;
#pragma warning restore

    private Transform _mainCam;
    private float _levelDiff;
    private float _bgDiff;
    private float _camStart;
    private Vector3 _newPos;
    private Vector3 _layerPos;
    #endregion

    private void Start()
    {
        _mainCam = GameObjectBank.Instance.mainCamera.transform;
        _bgDiff = _bgEnd.position.x - _bgStart.position.x;
        _levelDiff = (_levelEnd.position.x - _levelStart.position.x) / _bgDiff;
        _camStart = _bgCam.position.x - _mainCam.position.x / _levelDiff;
        _newPos = new Vector3(0, -50, -5);
        _layerPos = new Vector3();
    }

    private void LateUpdate()
    {
        Vector3 oldPos = _bgCam.position;
        _newPos.x = _mainCam.position.x / _levelDiff + _camStart;
        _bgCam.position = _newPos;

        if (oldPos.x != _newPos.x)
        {
            _layerPos.x = (oldPos.x - _newPos.x) * 0.2f;
            _layer1.position += _layerPos;

            _layerPos.x *= 0.75f;
            _layer2.position += _layerPos;

            _layerPos.x *= 0.75f;
            _layer3.position += _layerPos;

            _layerPos.x *= 0.75f;
            _layer4.position += _layerPos;
        }
    }

}
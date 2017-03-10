using UnityEngine;

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
#pragma warning restore
    private Transform _mainCam;
    private float _levelDiff;
    private float _bgDiff;
    private float _camStart;
    private Vector3 _newPos;
    #endregion

    private void Start()
    {
        _mainCam = GameObjectBank.Instance.mainCamera.transform;  
        _bgDiff = _bgEnd.position.x - _bgStart.position.x;
        _levelDiff = (_levelEnd.position.x - _levelStart.position.x) / _bgDiff;
        _camStart = _bgCam.position.x - _mainCam.position.x / _levelDiff;
        _newPos = new Vector3(0, -25, -5);
    }

    private void LateUpdate()
    {
        _newPos.x = _mainCam.position.x / _levelDiff + _camStart;
        _bgCam.position = _newPos;
    }

}
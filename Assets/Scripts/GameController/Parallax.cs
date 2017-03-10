using UnityEngine;

public class Parallax : MonoBehaviour
{
    #region Variables
    private Transform _mainCam;
    private Transform _bgCam;
#pragma warning disable 0649
    [SerializeField]
    private Transform _levelStart;
    [SerializeField]
    private Transform _levelEnd;
    [SerializeField]
    private Transform _bgStart;
    [SerializeField]
    private Transform _bgEnd;
#pragma warning restore
    private float _levelDiff;
    private float _bgDiff;
    private float _camStart;
    private Vector3 _diff;
    #endregion

    private void Start()
    {
        _mainCam = GameObjectBank.Instance.mainCamera.transform;
        _bgCam = GameObjectBank.Instance.backgroundCam.transform;
        _camStart = _bgCam.position.x;
        _bgDiff = _bgEnd.position.x - _bgStart.position.x;
        _levelDiff = (_levelEnd.position.x - _levelStart.position.x) / _bgDiff;
        _diff = new Vector3(0, -25, 0);
    }

    private void LateUpdate()
    {
        _diff.x = _mainCam.position.x / _levelDiff + _camStart;
        _bgCam.position = _diff;
    }

}
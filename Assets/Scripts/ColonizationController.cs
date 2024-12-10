using System.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine;

public class ColonizationController : MonoBehaviour
{
    [SerializeField] private MainBuildingPicker _mainBuildingPicker;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ConstructionPoint _constructionPoint;

    private bool _playerClickLeftMouseButton = false;
    private bool _settingFlagCanceled = false;
    private bool _coroutineWorking = true;
    private Coroutine _coroutine;

    private void OnEnable()
    {
        _mainBuildingPicker.Picked += TakeControlOverMainBuiding;
        _playerInput.LeftMouseButtonClicked += ClickLeftMouseButton;
    }

    private void OnDisable()
    {
        _mainBuildingPicker.Picked -= TakeControlOverMainBuiding;
        _playerInput.LeftMouseButtonClicked -= ClickLeftMouseButton;
    }

    private void TakeControlOverMainBuiding(MainBuilding mainBuidling)
    {
        _coroutine = StartCoroutine(TrySetFlagForANewMainBuilding(mainBuidling));
    }

    private IEnumerator TrySetFlagForANewMainBuilding(MainBuilding oldBuidling)
    {
        yield return new WaitForSeconds(0.1f);

        _playerClickLeftMouseButton = false;

        Debug.Log(nameof(TrySetFlagForANewMainBuilding));

        Debug.Log(_playerClickLeftMouseButton);

        //yield return new WaitUntil(() => _playerClickLeftMouseButton == true);
        yield return new WaitUntil(() => _playerClickLeftMouseButton == true);

        Debug.Log(_playerClickLeftMouseButton);

        Debug.Log(nameof(TrySetFlagForANewMainBuilding) + " continued");

        if (_constructionPoint.TryPlaceFlagForANewBuilding(out Vector3 placePosition))
        {
            oldBuidling.SetFlagForConstructionNewBase(placePosition);
        }

        //_playerClickLeftMouseButton = false;
        //_coroutineWorking = true;

        //Vector3 placePosition;

        //while (_coroutineWorking)
        //{
        //    if (_playerClickLeftMouseButton)
        //    {
        //        if (_constructionPoint.TryPlaceFlagForANewBuilding(out placePosition))
        //        {
        //            oldBuidling.SetFlagForConstructionNewBase(placePosition);
        //            _coroutineWorking = false;
        //        }
        //    }

        //    yield return null;
        //}

        _playerClickLeftMouseButton = false;
        StopCoroutine(_coroutine);
        _coroutine = null;
    }

    private void ClickLeftMouseButton()
    {
        _playerClickLeftMouseButton = true;
    }

    private void CancelSetFlag()
    {
        _coroutineWorking = false;
    }
}

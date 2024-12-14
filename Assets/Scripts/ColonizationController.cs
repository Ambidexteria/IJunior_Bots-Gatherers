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
        _coroutine = StartCoroutine(TrySetFlag(mainBuidling));
    }

    private IEnumerator TrySetFlag(MainBuilding mainBuilding)
    {
        yield return new WaitForSeconds(0.1f);

        _playerClickLeftMouseButton = false;

        yield return new WaitUntil(() => _playerClickLeftMouseButton == true);

        if (_constructionPoint.TryGetPlaceForFlag(out Vector3 placePosition))
        {
            mainBuilding.SetFlagForConstructionNewBase(placePosition);
        }

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

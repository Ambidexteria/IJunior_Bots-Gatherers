using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class ColonizationController : MonoBehaviour
{
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private FlagPlacer _flagPlacer;

    private bool _playerClickLeftMouseButton = false;
    private bool _settingFlagCanceled = false;
    private bool _coroutineWorking = true;
    private Coroutine _coroutine;

    public event Action<Vector3> PositionPicked;

    private void OnEnable()
    {
        _playerInput.LeftMouseButtonClicked += ClickLeftMouseButton;
    }

    private void OnDisable()
    {
        _playerInput.LeftMouseButtonClicked -= ClickLeftMouseButton;
    }

    public void PickPlaceForNewMainBuilding()
    {
        _coroutine = StartCoroutine(TrySetFlag());
    }

    [Inject]
    public void Construct(PlayerInput playerInput, FlagPlacer flagPlacer)
    {
        _playerInput = playerInput;
        _flagPlacer = flagPlacer;
    }

    private IEnumerator TrySetFlag()
    {
        yield return new WaitForSeconds(0.1f);

        _playerClickLeftMouseButton = false;

        yield return new WaitUntil(() => _playerClickLeftMouseButton == true);

        if (_flagPlacer.TryGetPlaceForFlag(out Vector3 placePosition))
        {
            Debug.Log(nameof(_flagPlacer.TryGetPlaceForFlag));
            PositionPicked?.Invoke(placePosition);
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

using System;
using System.Collections;
using UnityEngine;
using Zenject;

public class ColonizationController : MonoBehaviour
{
    [SerializeField] private float _secondClickDelay = 0.1f;

    private PlayerInput _playerInput;
    private FlagPlacer _flagPlacer;
    private Coroutine _coroutine;

    private WaitForSeconds _waitForSeconds;
    private WaitUntil _waitUntilNextClick;
    private bool _playerClickLeftMouseButton = false;

    public event Action<Vector3> PositionPicked;

    private void Awake()
    {
        _waitForSeconds = new(_secondClickDelay);
        _waitUntilNextClick = new(() => _playerClickLeftMouseButton == true);
    }

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
    private void Construct(PlayerInput playerInput, FlagPlacer flagPlacer)
    {
        _playerInput = playerInput;
        _flagPlacer = flagPlacer;
    }

    private IEnumerator TrySetFlag()
    {
        yield return _waitForSeconds;

        _playerClickLeftMouseButton = false;

        yield return _waitUntilNextClick;

        if (_flagPlacer.TryGetPlaceForFlag(out Vector3 placePosition))
        {
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
}

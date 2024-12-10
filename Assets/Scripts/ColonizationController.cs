using System.Collections;
using UnityEngine;

public class ColonizationController : MonoBehaviour
{
    [SerializeField] private MainBuildingPicker _mainBuildingPicker;
    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private ConstructionPoint _constructionPoint;

    private bool _playerClickLeftMouseButton = false;

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
        StartCoroutine(TrySetFlagForANewMainBuilding(mainBuidling));
    }

    private IEnumerator TrySetFlagForANewMainBuilding(MainBuilding oldBuidling)
    {
        _playerClickLeftMouseButton = false;

        Debug.Log(nameof(TrySetFlagForANewMainBuilding));

        yield return new WaitUntil(() => _playerClickLeftMouseButton);
        Debug.Log(nameof(TrySetFlagForANewMainBuilding) + " continued");
        if (_constructionPoint.TryPlaceFlagForANewBuilding(out Vector3 placePosition))
        {
            oldBuidling.SetFlagForConstructionNewBase(placePosition);
        }
    }

    private void ClickLeftMouseButton()
    {
        _playerClickLeftMouseButton = true;
    }
}

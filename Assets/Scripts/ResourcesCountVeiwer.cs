using UnityEngine;
using TMPro;

public class ResourcesCountVeiwer : MonoBehaviour
{
    [SerializeField] private Base _base;
    [SerializeField] private TextMeshProUGUI _resourcesCountText;

    private void OnEnable()
    {
        _base.ResourcesCountChanged += UpdateResourcesCount;
    }

    private void OnDisable()
    {
        _base.ResourcesCountChanged -= UpdateResourcesCount;
    }

    private void UpdateResourcesCount(int count)
    {
        _resourcesCountText.text = count.ToString();
    }
}
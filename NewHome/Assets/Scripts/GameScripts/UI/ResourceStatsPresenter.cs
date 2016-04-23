using UnityEngine;
using UnityEngine.UI;

public class ResourceStatsPresenter : MonoBehaviour
{
    [SerializeField] Text _statsText;
    [SerializeField] BaseResourceType _resourceType;
    [SerializeField] Color _goodColor = Color.green;
    [SerializeField] Color _badColor = Color.red;
    [SerializeField] bool _inverted = false;

    private BaseResourceContainer _resourceContainer;

    void Start()
    {
        _resourceContainer = Base.Instance.AvaliableResources.AvailableResources.Find(r => r.ResourceType == _resourceType);
        _resourceContainer.OnValueUpdated += TargetContainer_OnValueUpdated;
        DrawResourceCount(_resourceContainer.Count);
    }

    private void TargetContainer_OnValueUpdated(float value)
    {
        DrawResourceCount(value);
    }

    private void DrawResourceCount(float value)
    {        
        var percent = value/_resourceContainer.Capacity;

        var color = !_inverted ? Color.Lerp(_goodColor, _badColor, 1 - percent) : Color.Lerp(_goodColor, _badColor, percent);

        _statsText.color = color;
        _statsText.text = Mathf.RoundToInt(percent * 100) + " %";
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[Serializable]
public class BaseResourcesBalance
{
    public event Action<BaseResourceContainer> OnResourceMaxCountReached;
    public event Action<BaseResourceContainer> OnResourceDepleted;

    [SerializeField] ResourceData[] _initialResources;

    [SerializeField] List<BaseResourceContainer> _availableResources = null;

    public List<BaseResourceContainer> AvailableResources { get { return _availableResources; } }

    public void Init()
    {
        InitResources();
    }

    private void InitResources()
    {
        _availableResources = new List<BaseResourceContainer>();
        foreach (var resourceData in _initialResources)
        {
            if(_availableResources.Any(c=>c.ResourceType == resourceData.type))
                continue;

            var resContainer = new BaseResourceContainer(resourceData.type, resourceData.value, resourceData.capacity);
            resContainer.OnMaxCountReached += Resource_OnMaxCountReached;
            resContainer.OnResourceDepleted += Resource_OnResourceDepleted;
            _availableResources.Add(resContainer);
        }
    }    

    public void AddResource(BaseResourceType type, float value)
    {
        var resourceContainer = _availableResources.FirstOrDefault(r => r.ResourceType == type);
        if(resourceContainer != null)
            resourceContainer.Count += value;
        else
            Debug.LogError("Resource type not found: " + type.ToString());               
    }

    public bool TryUseResource(BaseResourceType type, float count)
    {
        var resourceContainer = _availableResources.FirstOrDefault(r => r.ResourceType == type);
        if (resourceContainer == null)
        {
            Debug.LogError("Resource type not found: " + type.ToString());
            return false;
        }

        if (resourceContainer.Count >= count)
        {
            resourceContainer.Count -= count;
            return true;
        }

        return false;
    }

    private void Resource_OnResourceDepleted(BaseResourceContainer baseResourceContainer)
    {
        if (OnResourceDepleted != null)
        {
            OnResourceDepleted(baseResourceContainer);
        }
    }

    private void Resource_OnMaxCountReached(BaseResourceContainer baseResourceContainer)
    {
        if (OnResourceMaxCountReached != null)
        {
            OnResourceMaxCountReached(baseResourceContainer);
        }
    }

    [Serializable]
    private class ResourceData
    {
        public BaseResourceType type;
        public float value;
        public float capacity;
    }
}
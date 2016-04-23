using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class BaseResourcesBalance
{
    public event Action<BaseResourceContainer> OnResourceMaxCountReached;
    public event Action<BaseResourceContainer> OnResourceDepleted;

    [SerializeField] ResourceInitialData[] _initialResources;

    private Dictionary<BaseResourceType, BaseResourceContainer> _availableResources = null;// = new Dictionary<BaseResourceType, BaseResourceContainer>();

    public Dictionary<BaseResourceType, BaseResourceContainer> AvailableResources
    {
        get
        {
            if (_availableResources == null)
            {
                Init();
            }

            return _availableResources;
        }
    }

    public void Init()
    {
        InitResources();
    }

    private void InitResources()
    {
        _availableResources = new Dictionary<BaseResourceType, BaseResourceContainer>();
        foreach (var resourceData in _initialResources)
        {
            var resContainer = new BaseResourceContainer(resourceData.type, resourceData.capacity, resourceData.value);
            resContainer.OnMaxCountReached += Resource_OnMaxCountReached;
            resContainer.OnResourceDepleted += Resource_OnResourceDepleted;
            _availableResources[resourceData.type] = resContainer;
        }
    }    

    public void AddResource(BaseResourceType type, int value)
    {
        _availableResources[type].Count += value;
    }

    public bool TryUseResource(BaseResourceType type, int count)
    {
        if (_availableResources.ContainsKey(type) && _availableResources[type].Count >= count)
        {
            _availableResources[type].Count -= count;
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
    private class ResourceInitialData
    {
        public BaseResourceType type;
        public int value;
        public int capacity;
    }
}
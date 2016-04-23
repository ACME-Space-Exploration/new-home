using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class BaseResourceContainer
{
    public event Action<BaseResourceContainer> OnMaxCountReached;
    public event Action<BaseResourceContainer> OnResourceDepleted;
    public event Action<float> OnValueUpdated;

    [SerializeField] BaseResourceType _type;
    [SerializeField] float _count;

    public BaseResourceType ResourceType { get { return _type; } }
    public float Capacity { get; set; }
    public float Count
    {
        get { return _count; }
        set
        {
            if (value <= Capacity)
            {
                if (value > 0)
                {
                    _count = value;
                }
                else
                {
                    _count = 0;
                    if (OnResourceDepleted != null)
                        OnResourceDepleted(this);
                }

            }
            else
            {
                _count = Capacity;
                if (OnMaxCountReached != null)
                    OnMaxCountReached(this);
            }
            if (OnValueUpdated != null)
            {
                OnValueUpdated(_count);
            }
        }
    }

    public BaseResourceContainer(BaseResourceType type, float initialCount, float maxCount)
    {
        _type = type;
        Capacity = maxCount;
        Count = initialCount;
    }
}

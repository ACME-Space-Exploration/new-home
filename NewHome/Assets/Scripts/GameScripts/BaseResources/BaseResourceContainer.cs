using System;
using UnityEngine;
using System.Collections;

[Serializable]
public class BaseResourceContainer
{
    public event Action<BaseResourceContainer> OnMaxCountReached;
    public event Action<BaseResourceContainer> OnResourceDepleted;

    private int _count;

    public BaseResourceType ResourceType { get; private set; }
    public int Capacity { get; set; }
    public int Count
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
        }
    }

    public BaseResourceContainer(BaseResourceType type, int initialCount, int maxCount)
    {
        ResourceType = type;
        Capacity = maxCount;
        Count = initialCount;
    }
}

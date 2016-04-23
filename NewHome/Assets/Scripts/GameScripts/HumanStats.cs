using System;
using UnityEngine;

[Serializable]
public class HumanStats
{
    public event Action<HumanStats> OnStatsChanged;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _hungry;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _thirsty;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _stress;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _agility;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _strength;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _health;
    [Range(0.0f, 1.0f)]
    [SerializeField] float _tiredness;

    public float Hungry
    {
        get { return _hungry; }
        set
        {
            _hungry = value;
            RaiseOnChangedEvent();
        }
    }

    public float Thirsty
    {
        get { return _thirsty; }
        set
        {
            _thirsty = value;
            RaiseOnChangedEvent();
        }
    }

    public float Stress
    {
        get { return _stress; }
        set
        {
            _stress = value;
            RaiseOnChangedEvent();
        }
    }

    public float Agility
    {
        get { return _agility; }
        set
        {
            _agility = value;
            RaiseOnChangedEvent();
        }
    }

    public float Strength
    {
        get { return _strength; }
        set
        {
            _strength = value;
            RaiseOnChangedEvent();
        }
    }

    public float Health
    {
        get { return _health; }
        set
        {
            _health = value;
            RaiseOnChangedEvent();
        }
    }

    public float Tiredness
    {
        get { return _tiredness; }
        set
        {
            _tiredness = value;
            RaiseOnChangedEvent();
        }
    }

    private void RaiseOnChangedEvent()
    {
        if (OnStatsChanged != null)
        {
            OnStatsChanged(this);
        }
    }
}

using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class BaseModule : MonoBehaviour
{
    public event Action<BaseModule> OnBroken;
    public event Action<BaseModule> OnRepaired;

    public event Action OnSwitchedOn;
    public event Action OnSwitchedOff;

    [SerializeField, Range(0f, 1f)]
    protected float _health = 1f;
    [SerializeField]
    protected List<Astronaut> _astronauts = new List<Astronaut>();    

    private Coroutine _productionUpdateCoroutine = null;

    public float Health
    {
        get { return _health; }
        protected set { _health = Mathf.Clamp(value, 0f, 1f); }
    }

    public bool SwitchedOn { get; private set; }

    public abstract ModuleType ModuleType { get; }

    void OnDisable()
    {
        if(_productionUpdateCoroutine != null)
            StopCoroutine(_productionUpdateCoroutine);
    }

    protected virtual IEnumerator ProductionUpdateCoroutine()
    {
        while (true)
        {
            ApplyResourcesConsumption(Base.Instance.AvaliableResources, Time.deltaTime);
            ProductionUpdate(Time.deltaTime);
            yield return null;
        }
    }

    protected abstract void ProductionUpdate(float deltaTime);
    protected abstract void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime);

    public virtual void SwitchOn()
    {
        _productionUpdateCoroutine = StartCoroutine(ProductionUpdateCoroutine());
        SwitchedOn = true;
        if (OnSwitchedOn != null)
        {
            OnSwitchedOn();
        }
    }

    public virtual void SwitchOff()
    {
        StopCoroutine(_productionUpdateCoroutine);
        SwitchedOn = false;
        if (OnSwitchedOff != null)
        {
            OnSwitchedOff();
        }
    }

    public virtual void AstronautEnter(Astronaut astronaut)
    {
        _astronauts.Add(astronaut);
    }

    public virtual void AstronautExit(Astronaut astronaut)
    {
        _astronauts.Remove(astronaut);
    }
}
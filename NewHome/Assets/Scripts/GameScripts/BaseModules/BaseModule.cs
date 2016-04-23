using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

public abstract class BaseModule : MonoBehaviour
{
    public event Action<BaseModule> OnBroken;
    public event Action<BaseModule> OnRepaired;

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

    public float ProductionProgress { get; protected set; }

    public bool SwitchedOn { get; private set; }

    public abstract ModuleType ModuleType { get; }

    void OnEnable()
    {
    }

    void OnDisable()
    {
        if(_productionUpdateCoroutine != null)
            StopCoroutine(_productionUpdateCoroutine);
    }

    protected virtual IEnumerator ProductionUpdateCoroutine()
    {
        while (true)
        {
            ProductionProgress = ProductionUpdate(Time.deltaTime);
            yield return null;
        }
    }

    protected abstract float ProductionUpdate(float deltaTime);

    public virtual void SwitchOn()
    {
        _productionUpdateCoroutine = StartCoroutine(ProductionUpdateCoroutine());
        SwitchedOn = true;
    }

    public virtual void SwitchOff()
    {
        StopCoroutine(_productionUpdateCoroutine);
        SwitchedOn = false;
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
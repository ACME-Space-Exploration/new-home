﻿using UnityEngine;

public class ResidentalBayModule : BaseModule
{
    [SerializeField] float _productionTime = 5;
    [SerializeField] float _fullRestoreTime = 1.0f;    
    [SerializeField] float _waterConsumption = 0.5f;
    [SerializeField] float _electricityConsumption = 1;

//    private float _productionCycleProgress;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.ResidentalBay; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
        if (_pauseProduction)
        {
            return;
        }
        UpdateAstronautsTiderness(deltaTime);
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Water, _waterConsumption * deltaTime);
    }

    private void UpdateAstronautsTiderness(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.decreaseTiredness(deltaTime); // -= _productionTime / _fullRestoreTime;
        }
    }    
}

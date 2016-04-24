using UnityEngine;
using System.Collections;

public class GreenhouseModule : BaseModule
{
    [SerializeField] float _foodProduction = 1.0f;

    [SerializeField] float _carbonConsumption = 0.02f;
    [SerializeField] float _electricityConsumption = 1.0f;
    [SerializeField] float _waterConsumption = 2.0f;

    //    private float _productionCycleProgress;
    private bool _pauseProduction = false;
    public override ModuleType ModuleType
    {
        get { return ModuleType.Greenhouse; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
        if (_pauseProduction)
        {
            return;
        }
        UpdateAstronautsTiderness(deltaTime);
    }
    private void UpdateAstronautsTiderness(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.increaseTiredness(deltaTime); // -= _productionTime / _fullRestoreTime;
        }
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Water, _waterConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Carbon, _carbonConsumption * deltaTime);
        resources.AddResource(BaseResourceType.Food, _foodProduction * deltaTime);
        //throw new System.NotImplementedException();
    }
}

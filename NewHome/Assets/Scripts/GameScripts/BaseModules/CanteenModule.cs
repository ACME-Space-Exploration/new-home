using UnityEngine;

public class CanteenModule : BaseModule
{
    [SerializeField] float _productionTime = 5;
    [SerializeField] float _fullRestoreTime = 1.0f;    
    [SerializeField] float _waterConsumption = 0.5f;
    [SerializeField] float _foodConsumption = 0.5f;
    [SerializeField] float _electricityConsumption = 1;

//    private float _productionCycleProgress;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.Canteen; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
        if (_pauseProduction)
        {
            return;
        }
        UpdateAstronautsHunger(deltaTime);
        UpdateAstronautsThirst(deltaTime);
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Water, _waterConsumption * deltaTime);
        if (_astronauts.Count > 0)
            _pauseProduction = !resources.TryUseResource(BaseResourceType.Food, _foodConsumption * _astronauts.Count * deltaTime);
    }

    private void UpdateAstronautsHunger(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.decreaseHunger(deltaTime);//Stats.Hungry -= _productionTime / _fullRestoreTime;
        }
    }

    private void UpdateAstronautsThirst(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.decreaseThurst(deltaTime);//.Stats.Thirsty -= _productionTime / _fullRestoreTime;
        }
    }
}

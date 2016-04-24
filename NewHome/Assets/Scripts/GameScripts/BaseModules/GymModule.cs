using UnityEngine;

public class GymModule : BaseModule
{
    [SerializeField] float _fullRestoreTime = 1.0f;    
    [SerializeField] float _electricityConsumption = 1;

//    private float _productionCycleProgress;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.Gym; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
        if (_pauseProduction)
        {
            return;
        }
        UpdateAstronautsTiderness(deltaTime);
        UpdateAstronautsThirst(deltaTime);
        UpdateAstronautsHealth(deltaTime);
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
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

    private void UpdateAstronautsThirst(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.increaseThirst(deltaTime); // -= _productionTime / _fullRestoreTime;
        }
    }

    private void UpdateAstronautsHealth(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return;
        }

        foreach (var astronaut in _astronauts)
        {
            astronaut.increaseHealth(deltaTime); // -= _productionTime / _fullRestoreTime;
        }
    }
}

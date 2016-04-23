using UnityEngine;

public class ResidentalBayModule : BaseModule
{
    [SerializeField] float _productionTime = 5;
    [SerializeField] float _fullRestoreTime = 60f;    
    [SerializeField] float _waterConsumption = 0.5f;
    [SerializeField] float _electricityConsumption = 1;

    private float _productionCycleProgress;
    private bool _pauseProduction = false;

    public override ModuleType ModuleType
    {
        get { return ModuleType.ResidentalBay; }
    }

    protected override float ProductionUpdate(float deltaTime)
    {
        return UpdateAstronautsTiderness(deltaTime);
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Electricity, _electricityConsumption * deltaTime);
        _pauseProduction = !resources.TryUseResource(BaseResourceType.Water, _waterConsumption * deltaTime);
    }

    private float UpdateAstronautsTiderness(float deltaTime)
    {
        if (_astronauts.Count == 0)
        {
            return 0;
        }
        _productionCycleProgress += deltaTime / _productionTime;
        if (_productionCycleProgress >= 1f)
        {
            foreach (var astronaut in _astronauts)
            {
                astronaut.Stats.Tiredness -= _productionTime / _fullRestoreTime;
            }

            _productionCycleProgress = 0;
        }

        return _productionCycleProgress;
    }    

    public override void AstronautEnter(Astronaut astronaut)
    {
        base.AstronautEnter(astronaut);
    }

    public override void AstronautExit(Astronaut astronaut)
    {
        base.AstronautExit(astronaut);
        if (_astronauts.Count == 0)
        {
            ProductionProgress = 0f;
            _productionCycleProgress = 0;
        }
    }
}

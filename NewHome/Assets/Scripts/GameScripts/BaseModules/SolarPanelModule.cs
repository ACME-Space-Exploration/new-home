using UnityEngine;
using System.Collections;

public class SolarPanelModule : BaseModule
{
    [SerializeField] float _electricityProduction = 5f;

    public override ModuleType ModuleType
    {
        get { return ModuleType.SolarPanel; }
    }

    protected override void ProductionUpdate(float deltaTime)
    {
//        throw new System.NotImplementedException();
    }

    protected override void ApplyResourcesConsumption(BaseResourcesBalance resources, float deltaTime)
    {
        resources.AddResource(BaseResourceType.Electricity, _electricityProduction * deltaTime);
        //throw new System.NotImplementedException();
    }
}

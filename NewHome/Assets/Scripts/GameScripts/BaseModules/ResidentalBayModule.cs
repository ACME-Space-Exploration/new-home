using UnityEngine;
using System.Collections;

public class ResidentalBayModule : BaseModule
{
    public override ModuleType ModuleType
    {
        get { return ModuleType.ResidentalBay; }
    }

    protected override float ProductionUpdate(float deltaTime)
    {
        throw new System.NotImplementedException();
    }

    // Use this for initialization
    void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    
}

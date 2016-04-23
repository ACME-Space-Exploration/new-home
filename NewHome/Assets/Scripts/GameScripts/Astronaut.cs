using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;

    public static float hungerPerSecond = 0.02f;
    public float thirstPerSecond = 0.03f;
    public float tirednessPerSecond = 0.01f;
    public float hungerHealthPerSecond = 0.01f;
    public float thirstHealthPerSecond = 0.005f;
    public float tirednessHealthPerSecond = 0.001f;
    public float hungerTreashold = 0.9f;
    public float thirstTreashold = 0.8f;
    public float tirednessTreashold = 0.99f;
    public BaseModule currentLocation;
    public BaseModule targetLocation = null;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut() {
        Debug.Log("INIT ASTRONAUT");
        System.Random rnd = new System.Random();
        _stats.Hungry = 0f;
        _stats.Thirsty = 0f;
        _stats.Stress = (float) rnd.NextDouble();
        _stats.Agility = (float) rnd.NextDouble();
        _stats.Strength = (float) rnd.NextDouble();
        _stats.Health = 1f;
        _stats.Tiredness = (float) rnd.NextDouble();
        currentLocation = Base.Instance.BaseModules[0];
    }


    void Start()
    {
        initAstronaut();
    }

    void increaseTiredness(float tirednessDelta) {
        _stats.Tiredness += tirednessDelta; 
    }

    void increaseHunger(float hungerDelta)
    {
        _stats.Hungry += hungerDelta;
    }

    void increaseThurst(float thirstDelta)
    {
        _stats.Thirsty += thirstDelta;
    }
    void increaseHealth(float healthDelta)
    {
        _stats.Health += healthDelta;
    }

    void decreaseTiredness(float tirednessDelta)
    {
        _stats.Tiredness -= tirednessDelta;
    }

    void decreaseHunger(float hungerDelta)
    {
        _stats.Hungry -= hungerDelta;
    }

    void decreaseThurst(float thirstDelta)
    {
        _stats.Thirsty -= thirstDelta;
    }

    void decreaseHealth(float healthDelta)
    {
        _stats.Health -= healthDelta;
    }

    void calculateHealthDelta(float deltaTime) {
        if (_stats.Hungry > hungerTreashold)
        {
            decreaseHealth(hungerHealthPerSecond * deltaTime);
        }
        if (_stats.Thirsty > thirstTreashold)
        {
            decreaseHealth(thirstHealthPerSecond * deltaTime);
        }
        if (_stats.Tiredness > tirednessTreashold)
        {
            decreaseHealth(tirednessHealthPerSecond * deltaTime);
        }
    }

    void setTargetLocation(BaseModule location)
    {
        targetLocation = location;
    }

    void chooseTargetLocation()
    {
        if (_stats.Tiredness > tirednessTreashold && currentLocation.ModuleType != ModuleType.ResidentalBay)
        {
            if (Base.Instance.BaseModules.Count > 0)
            {
                Base.Instance.BaseModules.ForEach(module =>
                {
                    if (module.ModuleType == ModuleType.ResidentalBay)
                        targetLocation = module;
                });
            }
        }
        else {
            targetLocation = null;
        }
    }

    void moveToTarget() {
        Debug.Log("MOVING TO " + targetLocation.ModuleType);
        currentLocation = targetLocation;
        targetLocation = null;
    }

    void Update() {
        float deltaTime = Time.deltaTime;
        increaseTiredness(tirednessPerSecond * deltaTime);
        increaseHunger(hungerPerSecond * deltaTime);
        increaseThurst(thirstPerSecond * deltaTime);
        calculateHealthDelta(deltaTime);
        if (Base.Instance.BaseModules.Count > 0) {
            chooseTargetLocation();
            if (targetLocation != null)
                moveToTarget();
        }
    }
}

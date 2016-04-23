using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;

    public static float hungerPerSecond;
    public float thirstPerSecond;
    public float tirednessPerSecond;
    public float hungerHealthPerSecond;
    public float thirstHealthPerSecond;
    public float tirednessHealthPerSecond;
    public float hungerTreashold;
    public float thirstTreashold;
    public float tirednessTreashold;

    public HumanStats Stats { get { return _stats; } }

    void initAstronaut() {
        System.Random rnd = new System.Random();
        _stats.Hungry = 0f;
        _stats.Thirsty = 0f;
        _stats.Stress = (float) rnd.NextDouble();
        _stats.Agility = (float) rnd.NextDouble();
        _stats.Strength = (float) rnd.NextDouble();
        _stats.Health = 1f;
        _stats.Tiredness = (float) rnd.NextDouble();
        tirednessPerSecond = 0.01f;
        hungerPerSecond = 0.02f;
        thirstPerSecond = 0.03f;
        hungerTreashold = 0.9f;
        thirstTreashold = 0.8f;
        tirednessTreashold = 0.99f;
        hungerHealthPerSecond = 0.01f;
        thirstHealthPerSecond = 0.005f;
        tirednessHealthPerSecond = 0.001f;
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

    void Update() {
        float deltaTime = Time.deltaTime;
        increaseTiredness(tirednessPerSecond * deltaTime);
        increaseHunger(hungerPerSecond * deltaTime);
        increaseThurst(thirstPerSecond * deltaTime);
        calculateHealthDelta(deltaTime);
    }
}

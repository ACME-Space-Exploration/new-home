using UnityEngine;

public class Astronaut : MonoBehaviour
{
    [SerializeField] HumanStats _stats;

    public HumanStats Stats { get { return _stats; } }
}

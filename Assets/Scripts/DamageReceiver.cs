using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum DamageTeam
{
    Enemy,
    Player,
    Neutral
}

public class DamageReceiver : MonoBehaviour
{
    
}

public class DamageEvent
{
    public float Damage { get; }
    public GameObject MainSource { get; }
}
using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float investigateDuration;
    public float hearingRange;
    public float lookRange;
    public float talkRate;
    public float instructionsRate;
}

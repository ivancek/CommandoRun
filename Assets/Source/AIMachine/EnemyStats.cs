using UnityEngine;


[CreateAssetMenu(menuName = "PluggableAI/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public float searchDuration;
    public float searchRange;
    public float lookRange;
    public float talkRate;
}

using UnityStandardAssets.Cameras;

/// <summary>
/// Play game mode implementation. Here we set rules like Win condition, Lose condition, Rewards, Obstacle spawning etc.
/// </summary>
public class PlayGameMode : GameMode
{
    /// <summary>
    /// Initializes the PlayGameMode
    /// </summary>
    public override void Init()
    {
        base.Init();

        if(PlayerPawn)
        {
            FindObjectOfType<AutoCam>().SetTarget(PlayerPawn.transform);
        }
    }
}

using UnityEngine;

[CreateAssetMenu(fileName = "GameValues", menuName = "ScriptableObjects/GameValuesSO")]
public class GameValuesSO : ScriptableObject
{
    [field: Header("Player")]
    [field: SerializeField] public float PlayerMaxMovementSpeed { get; private set; }
    [field: SerializeField] public float PlayerMovementAcceleration { get; private set; }

    [field: Header("Ball")]
    [field: SerializeField] public float BallInitialMovementSpeed { get; private set; }
    [field: SerializeField] public float BallMovementSpeedIncrement { get; private set; }
}

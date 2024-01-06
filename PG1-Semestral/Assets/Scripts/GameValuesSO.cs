using UnityEngine;

[CreateAssetMenu(fileName = "GameValues", menuName = "ScriptableObjects/GameValuesSO")]
public class GameValuesSO : ScriptableObject
{
    [field: SerializeField] public float PlayerMovementSpeed { get; private set; }
}

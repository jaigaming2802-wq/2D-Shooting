using UnityEngine;

public class CheckpointManager : MonoBehaviour
{
    public static CheckpointManager Instance;

    [SerializeField] private Transform startingPoint;

    private Transform currentCheckpoint;

    private void Awake()
    {
        Instance = this;

        currentCheckpoint = startingPoint;
    }

    public void SetCheckpoint(Transform checkpoint)
    {
        currentCheckpoint = checkpoint;

        
    }

    public Transform GetRespawnPoint()
    {
        return currentCheckpoint;
    }
}
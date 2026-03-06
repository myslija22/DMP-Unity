using UnityEngine;

public class CheckpointTrigger : MonoBehaviour
{
    public int index;
    public CheckpointTimer manager;

    void OnTriggerEnter(Collider other)
    {
        if (!enabled) return;
        if (other.CompareTag("Player"))
        {
            manager?.OnCheckpointTriggered(index);
        }
    }
}
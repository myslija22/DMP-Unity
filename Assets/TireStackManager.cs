using UnityEngine;

public class TireStackManager : MonoBehaviour
{
    private bool hasCollided = false;

    private void OnCollisionEnter(Collision collision)
    {
        if (!hasCollided && collision.gameObject.CompareTag("Player"))
        {
            hasCollided = true;

            Destroy(gameObject, 30f);
        }
    }
}

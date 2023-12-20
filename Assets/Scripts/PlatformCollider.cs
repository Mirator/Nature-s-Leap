using UnityEngine;

public class PlatformCollider : MonoBehaviour
{
    private Collider2D platformCollider;

    void Start()
    {
        // Ensure that we get the non-trigger collider component for the platform
        platformCollider = GetComponent<Collider2D>();
        if (platformCollider == null || platformCollider.isTrigger)
        {
            // Try to get a different collider that is not a trigger
            platformCollider = GetComponentInChildren<Collider2D>(includeInactive: true);
            if (platformCollider != null)
            {
                platformCollider.isTrigger = false;
            }
            else
            {
                Debug.LogError("No non-trigger Collider2D can be found on the platform prefab.", this);
            }
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platformCollider != null)
        {
            // Ignore collision when the player is coming from below
            Physics2D.IgnoreCollision(other, platformCollider, true);
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && platformCollider != null)
        {
            // Re-enable collision when the player is no longer inside the platform trigger area
            Physics2D.IgnoreCollision(other, platformCollider, false);
        }
    }
}

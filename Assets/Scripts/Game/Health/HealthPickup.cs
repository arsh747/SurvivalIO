using UnityEngine;

public class HealthPickup : MonoBehaviour
{
    HealthController hc;
    public float healthbonus = 15f;

    void Awake()
    {
        hc = FindObjectOfType<HealthController>();
    }

    void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            AudioManager.Instance?.PlayHealthPickup();

            // Show pickup event in feed
            HudOverlayController.Instance?.AddFeedEvent(
                HudOverlayController.FeedType.Pickup,
                "Picked up Health Pack",
                "Health Pack"
            );

            hc.AddHealth(15f);
            Destroy(gameObject);
        }
    }
}
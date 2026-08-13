using UnityEngine;

public class weaponpickup : MonoBehaviour
{
    public weapon weap;

    private void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Player")
        {
            AudioManager.Instance?.PlayWeaponPickup();

            target.GetComponent<Shoot>().currentWeapon = weap;
            target.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = weap.currentWeaponSpr;

            // Update top-right HUD: feed + weapon display
            HudOverlayController.Instance?.AddFeedEvent(
                HudOverlayController.FeedType.Pickup,
                $"Picked up {weap.name}",
                weap.name
            );
            HudOverlayController.Instance?.SetCurrentWeapon(weap);

            Destroy(gameObject);
        }
    }
}
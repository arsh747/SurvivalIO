using UnityEngine;
public class EnemyHealth : MonoBehaviour
{
    public GameObject hb;
    [SerializeField] private float health;
    void Update()
    {
        if (health < 1)
        {
            // Show kill event in feed before destroying
            HudOverlayController.Instance?.AddFeedEvent(
                HudOverlayController.FeedType.Kill,
                "Zombie died",
                "Zombie"
            );
            Destroy(gameObject);
        }
    }
    void OnTriggerEnter2D(Collider2D target)
    {
        if (target.tag == "Bullet")
        {
            health -= GameObject.Find("Player").GetComponent<Shoot>().currentWeapon.damage;
            AudioManager.Instance?.PlayZombieGroan();
            Destroy(target.gameObject);
            hb.transform.localScale = new Vector3(health / 100, hb.transform.localScale.y, hb.transform.localScale.z);
        }
    }
}
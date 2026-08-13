using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    private float speed = 8;
    private Vector2 dir;

    void Start()
    {
        dir = GameObject.Find("Dir").transform.position;
        transform.position = GameObject.Find("GunOffset").transform.position;
        transform.eulerAngles = new Vector3(0,0,GameObject.Find("Player").transform.eulerAngles.z);
    }

    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position, dir,speed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.GetComponent<EnemyMovemnt>())
        {
            //Destroy(collision.gameObject);
            Destroy(gameObject);
        }    
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSpawner : MonoBehaviour
{
    public GameObject[] weapons;
    public float xBound, yBound;
    void Start()
        {
            StartCoroutine (SpawnAWeapon ());
        }
   IEnumerator SpawnAWeapon ()
        {
            yield return new WaitForSeconds(10); 
            Vector2 spawnPoint = new Vector2 (Random.Range(-xBound, xBound), Random.Range(-yBound, yBound)); 
            Instantiate(weapons [Random.Range(0, weapons.Length)], spawnPoint, Quaternion.identity);
            StartCoroutine (SpawnAWeapon ());
        }
}

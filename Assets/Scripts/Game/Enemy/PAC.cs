using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PAC : MonoBehaviour
{
    public bool AwareOfPlayer { get; private set; }
    public Vector2 DirectionToPlayer { get; private set; }
    public Vector2 DirectionToBuilding { get; private set; }
    [SerializeField]
    private float _playerAwarenessDistance;
    private Transform _player;
    private Transform build;


    private void Awake()
    {
        _player = FindObjectOfType<PlayerMovement>().transform;
        //build = FindObjectOfType<building>().transform;
    }

    // Update is called once per frame
    void Update()
    {
        Vector2 enemyToPlayerVector = _player.position-transform.position;
        DirectionToPlayer = enemyToPlayerVector.normalized;
        //Vector2 enemyToBuildingVector = build.position-transform.position;
        //DirectionToBuilding = enemyToBuildingVector.normalized;
        if (enemyToPlayerVector.magnitude <= _playerAwarenessDistance)  //|| enemyToBuildingVector.magnitude <= _playerAwarenessDistance
            {
                AwareOfPlayer = true;
            }
        else
            {
                AwareOfPlayer = false;
            }
    }

}
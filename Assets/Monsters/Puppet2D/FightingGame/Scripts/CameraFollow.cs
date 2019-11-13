using System;
using UnityEngine;



public class CameraFollow : MonoBehaviour
{
    private FighterAIController[] _players;
    private Vector3 _offset;

    void Start()
    {
		_players = FindObjectsOfType<FighterAIController>();
        Vector3 pos =(_players[0].transform.position + _players[1].transform.position)/2f;

        _offset =pos - transform.position;

    }
    void Update()
    {
        Vector3 pos =(_players[0].transform.position + _players[1].transform.position)/2f;
        transform.position = Vector3.Lerp(transform.position, pos - _offset, Time.deltaTime);

    }
}


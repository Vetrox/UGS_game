using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamPositionUpdater : MonoBehaviour
{
    [SerializeField] private Transform player;
    [Range(1f, 10f)]
    public float horizontalDistance = 1.6f;
    [Range(1f, 10f)]
    public float verticalDistance = 1.6f;
    [Range(0.01f, 1f)]
    public float stiffnes = 0.75f;

    void Update()
    {
        Vector3 expected = player.position + Vector3.back * horizontalDistance + Vector3.up * verticalDistance;
        transform.position = Vector3.Lerp(transform.position, expected, stiffnes);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseDevice : MonoBehaviour
{
    [SerializeField]
    private float radius = 3.5f;

    private void OnMouseDown()
    {
        Transform player = GameObject.FindWithTag("Player").transform;
        if (player != null && Vector3.Distance(player.position, transform.position) < this.radius)
        {
            Vector3 direction = transform.position - player.position;
            if (Vector3.Dot(player.forward, direction) > 0.5f)
            {
                Operate();
            }
        }
    }

    public abstract void Operate();
}
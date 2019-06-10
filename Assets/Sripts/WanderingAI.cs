using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WanderingAI : MonoBehaviour
{
    [SerializeField]
    private float speed = 3.0f;
    [SerializeField]
    private float obstacleRange = 5.0f;
    [SerializeField]
    private float attackspeed = 1.5f;
    [SerializeField]
    private GameObject fireballPrefab;

    private bool _alive;
    private float _attackspeedTimer;

    private void Start()
    {
        _alive = true;
    }

    private void Update()
    {
        IncreaseAttackspeedTimer();
    }

    private void FixedUpdate()
    {
        if (_alive)
        {
            transform.Translate(0, 0, speed * Time.deltaTime);
            Ray ray = new Ray(transform.position, transform.forward);
            RaycastHit hit;
            if (Physics.SphereCast(ray, 0.75f, out hit))
            {
                GameObject hitObject = hit.transform.gameObject;
                if (hitObject.GetComponent<PlayerCharacter>())
                {
                    if (_attackspeedTimer == 0)
                    {
                        GameObject fireball = Instantiate(fireballPrefab) as GameObject;
                        fireball.transform.position = 
                            transform.TransformPoint(Vector3.forward * 1.5f);
                        fireball.transform.rotation = transform.rotation;
                    }
                }
                else if (hit.distance < obstacleRange)
                {
                    float angle = Random.Range(-110, 110);
                    transform.Rotate(0, angle, 0);
                }
            }
        }
    }

    public void SetAlive(bool alive)
    {
        _alive = alive;
    }

    private void IncreaseAttackspeedTimer()
    {
        _attackspeedTimer += Time.deltaTime;
        if (_attackspeedTimer > attackspeed)
        {
            _attackspeedTimer = 0;
        }
    }
}
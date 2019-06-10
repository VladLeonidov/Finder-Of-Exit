using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerCharacter : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        Messenger.AddListener(GameEvent.LEVEL_FAILED, OnPlayerDie);
        _animator = GetComponent<Animator>();
    }

    private void OnDestroy()
    {
        Messenger.RemoveListener(GameEvent.LEVEL_FAILED, OnPlayerDie);
    }

    public void Hurt(int damage)
    {
        _animator.SetBool("TakeDamage", true);
        ManagersProvider.Player.ChangeHealth(-damage);
        StartCoroutine(ResetDamageAnimation());
    }

    private IEnumerator ResetDamageAnimation()
    {
        yield return new WaitForSeconds(1);
        _animator.SetBool("TakeDamage", false);
    }

    private void OnPlayerDie()
    {
        _animator.SetBool("Die", true);
    }
}
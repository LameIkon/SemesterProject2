using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearController : MovementController
{
    [SerializeField] private PolarBearAI _polarBearAIScript;

    private bool _isAttacking;

    private void Start()
    {
        _polarBearAIScript.SetPolarBearController(this);
    }

    protected override void Update()
    {
        if (!_isAttacking)
        {
            base.Update();
        }
    }

    public void RunSpeed()
    {
        _moveSpeed = _speedReference.GetMaxValue();
    }

    public void WalkSpeed()
    {
        _moveSpeed = _speedReference.GetMinValue();
    }

    public void Attack(Vector3 ownPosition, float attackRange, float damage)
    {
        if (!_isAttacking)
        {
            StartCoroutine(AttackConsecutive(ownPosition, attackRange, damage));
        }
    }

    IEnumerator AttackConsecutive(Vector3 ownPosition, float attackRange, float damage)
    {
        _isAttacking = true;
        _movePoint.position = transform.position;
        AttackAnimation();
        yield return new WaitForSeconds (0.70f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(ownPosition, 1.8f * attackRange);
        foreach (var hit in hits)
        {
            if (hit.gameObject.name == "Player")
            {
                Debug.Log("Hit!");
                hit.GetComponent<IDamageable>()?.TakeDamage(damage);

            }
            else
            {
                //Debug.Log("missed");
            }
            yield return new WaitForSeconds(0.30f);
            _isAttacking = false;
        }
    }


    public override void Move(Vector3 direction)
    {
        if (CanMove(direction)) // if we DON't overlap with any colliders on nonwalkable layers, we CAN move.
        {
            _movePoint.position = MovePosition(direction); //basicly we dont actually move the "player" we move the invisible movePoint, and the player constantly "MoveTowards" that point in FixedUpdate.

            //Checks direction looking at
            if (direction.x > .5f)
            {
                direction.x = 1;
                _lookingDirection = "Right";
            }
            else if (direction.x < -.5f)
            {
                direction.x = -1;
                _lookingDirection = "Left";
            }
            StartAnimation();
        }
    }
    #region Animations
    protected override void IdleAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Idle_Right");
                return;
            case "Left":
                _animator.Play("Idle_Left");
                return;
            default:
                _animator.Play("Idle_Left");
                break;
        }
    }

    protected override void MoveAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Walking_Right");
                return;
            case "Left":
                _animator.Play("Walking_Left");
                return;
            default:
                _animator.Play("Walking_Left");
                break;
        }
    }

    protected override void RunningAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Running_Right");
                return;
            case "Left":
                _animator.Play("Running_Left");
                return;
            default:
                _animator.Play("Running_Left");
                break;
        }
    }

    public void AttackAnimation()
    {
        switch (_lookingDirection)
        {
            case "Right":
                _animator.Play("Attack_Right");
                return;
            case "Left":
                _animator.Play("Attack_Left");
                return;
            default:
                _animator.Play("Attack_Left");
                break;
        }
    }
    #endregion
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PolarBearController : MovementController
{
    [SerializeField] private PolarBearAI _polarBearAIScript;
    private AudioSource _runningAudioSource;
    private AudioSource _roarAudioSource;
    [SerializeField] private bool _forceRoarOnAggro;

    private bool _isAttacking;

    private void Start()
    {
        _polarBearAIScript.SetPolarBearController(this);

        AudioSource[] audioSources = GetComponents<AudioSource>(); // Get all audio components into an array
        if (audioSources.Length == 2) // we should have only 2 audio sources
        {
            _runningAudioSource = audioSources[0];
            _roarAudioSource = audioSources[1];
        }
    }

    protected override void Update()
    {
        if (!_isAttacking)
        {
            base.Update();
        }
    }

    protected override void FixedUpdate()
    {

        if (!_isAttacking)
        {
            base.FixedUpdate();
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
            _isAttacking = true;
            _runningAudioSource.Stop(); // stop running sound
            StartCoroutine(AttackConsecutive(ownPosition, attackRange, damage));
        }
    }

    // Ensure that the runtime of this coroutine is the same lenght as the attack animation - will cause visual animations bugs otherwise
    IEnumerator AttackConsecutive(Vector3 ownPosition, float attackRange, float damage)
    {
        _movePoint.position = transform.position; // Force stop the bear. Will however offset them from the grid
        yield return null;
        yield return null;
        AttackAnimation();
        yield return new WaitForSeconds (0.2f);
        Collider2D[] hits = Physics2D.OverlapCircleAll(ownPosition, 2.2f * attackRange);
        foreach (var hit in hits)
        {
            if (hit.gameObject.name == "Player")
            {
                hit.GetComponent<IDamageable>()?.TakeDamage(damage);

            }
            else
            {
                //Debug.Log("missed");
                Debug.Log(hit.name);
            }
            yield return new WaitForSeconds(0.8f);
            // looks bad to set it to the grid this way. should be set to round number when looking at the enemy instead
            //_movePoint.position = new Vector3(Mathf.Round(transform.position.x), Mathf.Round(transform.position.y), Mathf.Round(transform.position.z)); // Set the position to the grid again
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

    protected override void StartAnimation()
    {
        if (_animator != null)
        {
            if (Vector3.Distance(transform.position, _movePoint.position) >= 1f) // Only change when you move 1 tile
            {
                _isIdling = false;
                if (_walkingSpeed < _moveSpeed || _forceRunningAnimation) // If your moveSpeed is faster than your walkingpeed it means you are running
                {
                    RunningAnimation();
                    if (!_runningAudioSource.isPlaying) // Checks if the audio is not playing
                    {
                        if (Random.value < 0.25f || _forceRoarOnAggro) // 25% chance to roar or if boolean set to true. 
                        {
                            _roarAudioSource.Play(); 
                        }
                        _runningAudioSource.Play();
                    }
                }
                else
                {
                    MoveAnimation();
                    if (_runningAudioSource.isPlaying) // Check if the audio is playing
                    {
                        _roarAudioSource.Stop();
                        _runningAudioSource.Stop();
                    }
                }
            }
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

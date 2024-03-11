using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{

    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _bulletSpeed;
    [SerializeField] private Vector3 _bulletDirection;


    private Rigidbody2D _rb;

    void Awake() 
    {
        _rb = GetComponent<Rigidbody2D>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        collision.gameObject.GetComponent<IDamageable>()?.TakeDamage(_bulletDamage);
        Destroy(gameObject);
    }

    private void FixedUpdate()
    {
        _rb.velocity = _bulletDirection * _bulletSpeed;
    }

    public void InitBullet(float damage, float speed, Vector2 dir) 
    {
        _bulletDamage = damage;
        _bulletSpeed = speed; 
        _bulletDirection = new Vector3(dir.x, dir.y, 0);
    }

}

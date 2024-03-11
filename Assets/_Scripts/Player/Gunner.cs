using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    [SerializeField] private Transform _firingPoint;
    [SerializeField] private GameObject _bullet;
    [SerializeField] private float _bulletDamage;
    [SerializeField] private float _bulletSpeed;

    private Vector2 _mousePosition;


    void OnEnable() 
    {
        InputReader.OnLeftClickEvent += HandleShoot;
        InputReader.OnMousePositionEvent += HandleMousePosition;
    }

    void OnDisable() 
    {
        InputReader.OnLeftClickEvent -= HandleShoot;
        InputReader.OnMousePositionEvent -= HandleMousePosition;
    }


    void HandleShoot() 
    {
        var bullet = Instantiate(_bullet, _firingPoint.position, Quaternion.Euler(0, 0, 180));
        Vector3 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, 0));
        bullet.GetComponent<Bullet>().InitBullet(_bulletDamage, _bulletSpeed, (mousePosition - _firingPoint.position).normalized);
    }

    void HandleMousePosition(Vector2 dir) 
    {
        _mousePosition = dir;
    }

}

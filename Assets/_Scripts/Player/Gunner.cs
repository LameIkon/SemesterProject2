using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gunner : MonoBehaviour
{
    //[SerializeField] private Transform _firingPoint;
    //[SerializeField] private GameObject _bullet;
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
        Vector2 mousePosition = Camera.main.ScreenToWorldPoint(new Vector3(_mousePosition.x, _mousePosition.y, 0)) - new Vector3(transform.parent.position.x, transform.parent.position.y, 0);
        Vector2 position2D = new Vector2(transform.parent.position.x, transform.parent.position.y);
        //Vector2 direction = (mousePosition - position2D).normalized;
        //Vector2 firingPoint = position2D + direction;

        RaycastHit2D[] shotHit = Physics2D.RaycastAll(position2D, mousePosition);

        //Debug.DrawRay(position2D, mousePosition);

        foreach (var shot in shotHit) 
        {
            IDamageable iDamageable = shot.collider.GetComponent<IDamageable>();

            if (iDamageable != null)
            {
                if (shot.collider.GetComponentInChildren<Gunner>()) 
                {
                    continue;
                }
                //Debug.Log("Gunner: " + shot.collider.name);
                iDamageable.TakeDamage(_bulletDamage);
            }
        }

        //var bullet = Instantiate(_bullet, _firingPoint.position, Quaternion.Euler(0, 0, 180));
        //bullet.GetComponent<Bullet>().InitBullet(_bulletDamage, _bulletSpeed, (mousePosition - _firingPoint.position).normalized);
}

    void HandleMousePosition(Vector2 dir) 
    {
        _mousePosition = dir;
    }

}

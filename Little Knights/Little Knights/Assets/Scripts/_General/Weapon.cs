using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Weapon : MonoBehaviour
{

    [SerializeField]
    private float _damage;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private GameObject _master;
    [SerializeField]
    private bool _isRanged;
    [SerializeField]
    private GameObject _projectile;
    // Start is called before the first frame update


    public void rangeAtack() { 
        if (_isRanged)
        {
            _projectile.GetComponent<Projectile>().master = _master;
            Instantiate(_projectile, transform.position, Quaternion.identity);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!_isRanged) {
            if (other.tag == "Enemy" && other.tag != _master.tag)
            {
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 0.3f);
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.doDamage(_damage);
                }
            } else if (other.tag == "Player" && other.tag != _master.tag)
            {
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 0.3f);
                Player player = other.GetComponent<Player>();
                if (player != null)
                {
                    player.doDamage(_damage);
                }
            }
        }
    }
}

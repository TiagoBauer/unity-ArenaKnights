using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.AI;

public class Projectile : MonoBehaviour
{
    [SerializeField]
    private GameObject _firePostion;
    [SerializeField]
    private float _damage;
    [SerializeField]
    private float _speed;
    public GameObject master;
    private int arrowPosition;
    [SerializeField]
    private AudioClip _audioClip;
    [SerializeField]
    private AudioClip _audioClipDamage;
    [SerializeField]
    private bool _isMagical;
    [SerializeField]
    private int[] _MagicType;
    // Update is called once per frame
    private void Start()
    {
        StartCoroutine(lifeSpan());
        if (master.tag == "Enemy" && master.GetComponent<Enemy>().isLeft)
        {
            arrowPosition = -1;
            transform.localScale = new Vector3(transform.localScale.x * 1, transform.localScale.y, transform.localScale.z);
        }
        else if (master.tag == "Enemy" && !master.GetComponent<Enemy>().isLeft)
        {
            arrowPosition = 1;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 0.3f);
    }

    void Update()
    {
        transform.Translate(Vector3.right * Time.deltaTime * _speed * arrowPosition);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && other.tag != master.tag)
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.doDamage(_damage);
                AudioSource.PlayClipAtPoint(_audioClipDamage, Camera.main.transform.position, 0.3f);
                Destroy(gameObject);
            }
        }
        if (other.tag == "Enemy" && other.tag != master.tag)
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.doDamage(_damage);
                AudioSource.PlayClipAtPoint(_audioClipDamage, Camera.main.transform.position, 0.3f);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator lifeSpan()
    {
        yield return new WaitForSeconds(5f);
        Destroy(gameObject);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health_potion : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField]
    private GameObject[] _bounds;

    [SerializeField]
    private float _power;
    public float spawnRate;
    
    private float _maxUp;
    private float _maxDown;
    private float _maxLeft;
    private float _maxRight;
    [SerializeField]
    private AudioClip _audioClip;

    void Start()
    {
        if (_bounds.Length > 0)
        {
            _maxUp = _bounds[0].transform.position.y;
            _maxDown = _bounds[1].transform.position.y;
            _maxLeft = _bounds[2].transform.position.x;
            _maxRight = _bounds[3].transform.position.x;
            float initY = Random.Range(_maxDown, _maxUp);
            float initX = Random.Range(_maxLeft, _maxRight);
            transform.position = new Vector3(initX, initY, 0);
        }
        StartCoroutine(lifeSapan());
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if (player != null)
            {
                player.heal(_power);
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 1f);
                Destroy(gameObject);
            }
        }
    }

    private IEnumerator lifeSapan()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }
}

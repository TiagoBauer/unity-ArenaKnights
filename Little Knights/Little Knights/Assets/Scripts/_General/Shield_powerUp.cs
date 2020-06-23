using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Shield_powerUp : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _bounds;

    public float spawn;
    private float _maxUp;
    private float _maxDown;
    private float _maxLeft;
    private float _maxRight;

    [SerializeField]
    private AudioClip _audioClip;

    private EndLevel _endLevel;
    // Start is called before the first frame update
    void Start()
    {
        if(_bounds.Length > 0) { 
            _maxUp = _bounds[0].transform.position.y;
            _maxDown = _bounds[1].transform.position.y;
            _maxLeft = _bounds[2].transform.position.x;
            _maxRight = _bounds[3].transform.position.x;
            float initY = Random.Range(_maxDown, _maxUp);
            float initX = Random.Range(_maxLeft, _maxRight);
            transform.position = new Vector3(initX, initY, 0);
        }
        _endLevel = GameObject.Find("EndLevel").GetComponent<EndLevel>();
        StartCoroutine(lifeSapan());
    }

    // Update is called once per frame
    void Update()
    {
        if(_endLevel.clear)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Player player = other.GetComponent<Player>();
            if(player != null)
            {
                player.activeShield();
                AudioSource.PlayClipAtPoint(_audioClip, Camera.main.transform.position, 0.08f);
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

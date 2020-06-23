using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float strenght;
    [SerializeField]
    private GameObject _weapon;
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private GameObject _weaponAtack;
    [SerializeField]
    private float _atackRate;
    [SerializeField]
    private float _rangedRate;
    private float _canAtack;
    private float _canRanged;
    public float spawnRate;
    [SerializeField]
    private GameObject _bloodStain;
    private Color _initialColor;
    [SerializeField]
    private GameObject _healhIndicator;
    private float _initialhealth;
    private UiManager _uiManager;
    [SerializeField]
    private AudioClip _deathSound;
    private bool _canDamage = true;
    public bool isLeft;
    [SerializeField]
    private GameObject _shield;
    private bool _hasShield;
    private EndLevel _endLevel;
    [SerializeField]
    private bool _isRangedEnemy;
    [SerializeField]
    private bool _isMelleeEnemy;
    public GameObject rangedPosition;
    public bool stayLeft;
    
    public int minionCount;
    public GameObject[] minions;

    private float _maxUp;
    private float _maxDown;
    private float _maxLeft;
    private float _maxRight;
    // Start is called before the first frame update
    void Start()
    {
        _maxUp = GameObject.FindGameObjectWithTag("MaxUp").GetComponent<Transform>().position.y;
        _maxDown = GameObject.FindGameObjectWithTag("MaxDown").GetComponent<Transform>().position.y;
        _maxLeft = GameObject.FindGameObjectWithTag("MaxLeft").GetComponent<Transform>().position.x;
        _maxRight = GameObject.FindGameObjectWithTag("MaxRight").GetComponent<Transform>().position.x;

        if (_target == null) {
            GameObject.FindGameObjectWithTag("Player");
        }
        _initialColor = GetComponent<SpriteRenderer>().color;
        _healhIndicator.GetComponent<SpriteRenderer>().color = Color.green;
        _initialhealth = _health;

        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
 
        if(_shield != null)
        {
            _hasShield = true;
            _healhIndicator.GetComponent<SpriteRenderer>().color = Color.white;
        } else
        {
            _hasShield = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        _endLevel = GameObject.Find("EndLevel").GetComponent<EndLevel>();
        _target = GameObject.FindGameObjectWithTag("Player");
        if (_health > 0 && _target != null)
        {
            if ((Mathf.Abs(_target.transform.position.x - transform.position.x) >1.5f)
             || (Mathf.Abs(_target.transform.position.y - transform.position.y) > 1f))
                if (!_isRangedEnemy || (_isRangedEnemy && _isMelleeEnemy))
                {
                    moveTowardsPlayer();
                } else
                {
                    moveParalelToPlayer();
                }
                
            atack();
        }
        if (_endLevel.clear)
        {
            Destroy(gameObject);
        }

    }

    public void moveTowardsPlayer()
    {
        if(_target.transform.position.x < transform.position.x && isLeft)
        {
            isLeft = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        } else if (_target.transform.position.x > transform.position.x && !isLeft)
        {
            isLeft = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }

        GetComponent<Animator>().SetBool("isMoving", true);
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, _speed * Time.deltaTime);
    }

    public void moveParalelToPlayer()
    {
        Vector3 rangeLocal = new Vector3(0f,transform.position.y,0f);
        
        if (!stayLeft)
        {
            rangeLocal.x = _maxLeft + 1.8f;
        } else
        {
            rangeLocal.x = _maxRight - 1.8f;
        }
        
        if (_target.transform.position.x < transform.position.x && !isLeft)
        {
            isLeft = true;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        else if (_target.transform.position.x > transform.position.x && isLeft)
        {
            isLeft = false;
            transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
        }
        GetComponent<Animator>().SetBool("isMoving", true);
        if (transform.position.x != rangeLocal.x) {
            transform.position = Vector3.MoveTowards(transform.position, rangeLocal, _speed * Time.deltaTime);
        }
        if (_target.transform.position.y > transform.position.y)
        {
            transform.Translate(Vector3.up * Time.deltaTime * _speed * 1);
        }
        else if(_target.transform.position.y < transform.position.y)
        {
            transform.Translate(Vector3.up * Time.deltaTime * _speed * -1);
        }

    }

    public void doDamage(float hit)
    {
        if (_hasShield)
        {
            _hasShield = false;
            _shield.SetActive(false);
            checkLife();
        } else if (_canDamage) { 
            if(_health > 0) { 
                _health -= hit;
                StartCoroutine(flashHit());
                checkLife();
                if (_health <= 0)
                {
                    gameObject.GetComponent<BoxCollider2D>().enabled = false;
                    _uiManager.incremetScore(100);
                    GetComponent<Animator>().SetBool("isMoving", false);
                    _healhIndicator.GetComponent<SpriteRenderer>().color = Color.black;
                    StartCoroutine(deathAnim());
                }
            }
        }
    }

    private IEnumerator deathAnim()
    {
        GetComponent<Animator>().SetBool("isDead", true);
        Destroy(_weapon);
        transform.eulerAngles = new Vector3(0, 0, -90);
        transform.GetComponent<SpriteRenderer>().sortingOrder = 0;
        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position, 0.08f);
        Instantiate(_bloodStain, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        _uiManager.deathCount++;
        Destroy(gameObject);
    }

    private void atack()
    {
        if (Time.time > _canAtack)
        {
            if (_isMelleeEnemy)
            {
                StartCoroutine(atackAnim());
                _canAtack = Time.time + _atackRate;
            }
        }
        if (Time.time > _canRanged)
        {
            if (_isRangedEnemy)
            {
                _weaponAtack.GetComponent<Weapon>().rangeAtack();
                StartCoroutine(atackAnim());
                _canRanged = Time.time + _rangedRate;
            }
        }
    }

    IEnumerator atackAnim()
    {
        _weaponAtack.SetActive(true);
        _weapon.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        _weaponAtack.SetActive(false);
        _weapon.SetActive(true);
    }

    private IEnumerator flashHit()
    {
        GetComponent<SpriteRenderer>().color = Color.red;
        //_canDamage = false;
        yield return new WaitForSeconds(0.5f);
        //_canDamage = true;
        GetComponent<SpriteRenderer>().color = _initialColor;
    }

    private void checkLife()
    {
        if (_health < (_initialhealth * 25 / 100))
        {
            _healhIndicator.GetComponent<SpriteRenderer>().color = Color.red;
        } else if (_health < (_initialhealth * 50 / 100))
        { 
            _healhIndicator.GetComponent<SpriteRenderer>().color = Color.yellow;
        } else
        {
            _healhIndicator.GetComponent<SpriteRenderer>().color = Color.green;
        }
    }

    public bool isRanged()
    {
        if (_isRangedEnemy)
        {
            return true;
        } else
        {
            return false;
        }
    }
}

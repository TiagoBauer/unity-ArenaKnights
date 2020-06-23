using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [SerializeField]
    private float _speed;
    [SerializeField]
    private float _health;

    public float _strenght;
    [SerializeField]
    private float _jumpPower;
    [SerializeField]
    private GameObject[] _bounds;
    [SerializeField]
    private GameObject _weapon;
    [SerializeField]
    private GameObject _weaponAtack;
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private float _atackRate;
    [SerializeField]
    private Text _healthBar;
    [SerializeField]
    private GameObject _bloodStain;
    [SerializeField]
    private AudioClip _ShieldOff;
    [SerializeField]
    private AudioClip _deathSound;
    [SerializeField]
    private float _maxHealt;
    [SerializeField]
    private bool isLeft = false;

    private bool _shieldActive;
    private float _horizontalMove;
    private float _verticalMove;
    private float _side;
    private Vector3 _weaponInitialPosition;
    private bool _hasShield;
    private float _canAtack;
    private Color _initialColor;

    private float _maxUp;
    private float _maxDown;
    private float _maxLeft;
    private float _maxRight;
    private bool _canDamage = true;
    
    private UiManager _playerHealthBar;
   
    private EndLevel _endLevel;
    // Start is called before the first frame update
    void Start()
    {
        _health = _maxHealt;
        _initialColor = GetComponent<SpriteRenderer>().color;
        _playerHealthBar = GameObject.Find("Canvas").GetComponent<UiManager>();
        _endLevel = GameObject.Find("EndLevel").GetComponent<EndLevel>();
        _playerHealthBar.setHealth(_maxHealt);
        _maxUp = GameObject.FindGameObjectWithTag("MaxUp").transform.position.y;
        _maxDown = GameObject.FindGameObjectWithTag("MaxDown").transform.position.y;
        _maxLeft = GameObject.FindGameObjectWithTag("MaxLeft").transform.position.x;
        _maxRight = GameObject.FindGameObjectWithTag("MaxRight").transform.position.x;

    }

    // Update is called once per frame
    void Update()
    {
        if (_health > 0)
        {
            _verticalMove = Input.GetAxis("Vertical");
            _horizontalMove = Input.GetAxis("Horizontal");
            if (_horizontalMove < 0 && !isLeft)
            {
                isLeft = true;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            else if (_horizontalMove > 0 && isLeft)
            {
                isLeft = false;
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
            movement();
            limitMovement();
            atack();
        }
    }

    private void movement()
    {
        if (_horizontalMove != 0f || _verticalMove != 0f) {
            GetComponent<Animator>().SetBool("isMoving", true);
        } else
        {
            GetComponent<Animator>().SetBool("isMoving", false);
        }
        transform.Translate(Vector3.right * Time.deltaTime * _speed * _horizontalMove);
        transform.Translate(Vector3.up * Time.deltaTime * _speed * _verticalMove);
    }

    private void limitMovement()
    {
        if (transform.position.y >= _maxUp)
        {
            transform.position = new Vector3(transform.position.x, _maxUp, 0);
        }
        else if (transform.position.y <= _maxDown)
        {
            transform.position = new Vector3(transform.position.x, _maxDown, 0);
        }

        if (transform.position.x <= _maxLeft)
        {
            transform.position = new Vector3(_maxLeft, transform.position.y, 0);
        }
        else if (transform.position.x >= _maxRight)
        {
            transform.position = new Vector3(_maxRight, transform.position.y, 0);
        }
    }

    private void atack()
    {
        if (Input.GetButtonDown("Fire1")) {
            if (Time.time > _canAtack)
            {
                StartCoroutine(atackAnim());
                _canAtack = Time.time + _atackRate;
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

    public void activeShield()
    {
        if (!_hasShield)
        {
            _shield.SetActive(true);
            _hasShield = true;
        } else
        {
            _playerHealthBar.incremetScore(5);
        }
    }

    public void doDamage(float hit)
    {
        if (_hasShield)
        {
            StartCoroutine(flashHit());
            _shield.SetActive(false);
            _hasShield = false;
            AudioSource.PlayClipAtPoint(_ShieldOff, Camera.main.transform.position, 0.1f);
        } else
        {
            if (_canDamage)
            {
                if (_health > 0)
                {
                    _health -= hit;
                    StartCoroutine(flashHit());
                    _playerHealthBar.doDamage(hit);
                }
                if (_health <= 0)
                {
                    gameObject.GetComponent<Rigidbody2D>().mass = 9999f;
                    gameObject.GetComponent<BoxCollider2D>().isTrigger = true;
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
        AudioSource.PlayClipAtPoint(_deathSound, Camera.main.transform.position, 0.5f);
        Instantiate(_bloodStain, new Vector3(transform.position.x, transform.position.y - 0.5f, 0), Quaternion.identity);
        yield return new WaitForSeconds(3f);
        _endLevel.endType = 0;
        _endLevel.endLevel = true;
        Destroy(gameObject);
    }

    public void heal(float hp)
    {
        float _auxHP = 0;
        if (_health + hp <= _maxHealt)
        {
            _health += hp;
            _playerHealthBar.doHeal(hp);
        } else
        {
            _auxHP = (_health + hp) - _maxHealt;
            _health = _maxHealt;
            _playerHealthBar.doHeal(_auxHP);
            _playerHealthBar.incremetScore(50);
        }
    }

    private IEnumerator flashHit()
    {
        if(_hasShield)
            GetComponent<SpriteRenderer>().color = Color.green;
        else
            GetComponent<SpriteRenderer>().color = Color.red;
        _canDamage = false;
        yield return new WaitForSeconds(0.5f);
        GetComponent<SpriteRenderer>().color = _initialColor;
        _canDamage = true;
    }


    private void OnTriggerEnter2D(Collider2D other)
    {
        
    }
}

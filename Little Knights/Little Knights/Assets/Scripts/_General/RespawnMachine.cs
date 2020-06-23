using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnMachine : MonoBehaviour
{
    [SerializeField]
    private GameObject _shield;
    [SerializeField]
    private GameObject[] _enemiesVariations;
    [SerializeField]
    private GameObject[] _bosses;
    [SerializeField]
    private GameObject[] _weapons;
    [SerializeField]
    private GameObject[] _bounds;
    [SerializeField]
    private GameObject[] _itens;


    public int maxEnemiesBeforeBoos;
    public int quantitySpawn = 0;
    [SerializeField]
    public bool _survival;
    [SerializeField]
    private float _dificulty;

    private float _maxUp;
    private float _maxDown;
    private float _maxLeft;
    private float _maxRight;
    public int startSpawn = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void Update()
    {
        if (startSpawn == 0) {
            _maxUp = _bounds[0].transform.position.y;
            _maxDown = _bounds[1].transform.position.y;
            _maxLeft = _bounds[2].transform.position.x;
            _maxRight = _bounds[3].transform.position.x;
            startSpawn = 1;
            StartCoroutine(enemySpawnRoutine());
            StartCoroutine(shieldSpawnRoutine());
            StartCoroutine(itemSpawnRoutine());
        }
    }

    private IEnumerator enemySpawnRoutine()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        _dificulty = 1f;
        while ((player != null && maxEnemiesBeforeBoos >= quantitySpawn) || (_survival && player != null))
        {
            int index = Random.Range(0, _enemiesVariations.Length);
            yield return new WaitForSeconds(_enemiesVariations[index].GetComponent<Enemy>().spawnRate * _dificulty);
            float newY = Random.Range(_maxDown, _maxUp);
            int randomX = Random.Range(0, 10);
            float _newX;
            if (randomX >= 5)
            {
                _newX = _maxRight;
                _enemiesVariations[index].GetComponent<Enemy>().isLeft = true;
                _enemiesVariations[index].GetComponent<Enemy>().stayLeft = true;
            }
            else
            {
                _newX = _maxLeft;
                _enemiesVariations[index].GetComponent<Enemy>().isLeft = true;
                _enemiesVariations[index].GetComponent<Enemy>().stayLeft = false;
            }
            Instantiate(_enemiesVariations[index], new Vector3(_newX, newY, 0), Quaternion.identity);
            quantitySpawn++;
            player = GameObject.FindGameObjectWithTag("Player");
            if(_survival && (quantitySpawn > 15 && quantitySpawn <= 30))
            {
                _dificulty = 0.50f;
            } else if(_survival && (quantitySpawn > 30))
            {
                _dificulty = 0.30f;
            }

            if (!_survival && quantitySpawn == maxEnemiesBeforeBoos)
            {                 
                int minionSpawn = 0;
                
                yield return new WaitForSeconds(_bosses[0].GetComponent<Enemy>().spawnRate * _dificulty);
                float boosY = Random.Range(_maxDown, _maxUp);
                while(_bosses[0].GetComponent<Enemy>().minions.Length > minionSpawn)
                {
                    Instantiate(_bosses[0].GetComponent<Enemy>().minions[minionSpawn], new Vector3(_maxRight, boosY, 0), Quaternion.identity);
                    minionSpawn++;
                    quantitySpawn++;
                }
                Instantiate(_bosses[0], new Vector3(_maxRight, boosY, 0), Quaternion.identity);
                quantitySpawn++;
            }
        }
    }

    private IEnumerator shieldSpawnRoutine()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (player != null)
        {
            yield return new WaitForSeconds(_shield.GetComponent<Shield_powerUp>().spawn);
            float newY = Random.Range(_maxDown, _maxUp);
            float newX = Random.Range(_maxLeft, _maxRight);
            Instantiate(_shield, new Vector3(newX, newY, 0), Quaternion.identity);
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    private IEnumerator itemSpawnRoutine()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        while (player != null)
        {
            int index = Random.Range(0, _itens.Length);
            yield return new WaitForSeconds(_itens[index].GetComponent<Health_potion>().spawnRate);
            float newY = Random.Range(_maxDown, _maxUp);
            float newX = Random.Range(_maxLeft, _maxRight);
            Instantiate(_itens[index], new Vector3(newX, newY, 0), Quaternion.identity);
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    public void alterEnemies(GameObject[] enemyArea)
    {
        _enemiesVariations = enemyArea;
    }

    public void alterBosses(GameObject[] BossArea)
    {
        Debug.Log(_bosses);
        _bosses = BossArea;
    }

    public void isSurvival(bool survivalMap)
    {
        _survival = survivalMap;
    }
}

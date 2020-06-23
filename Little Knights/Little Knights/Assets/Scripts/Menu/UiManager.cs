using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiManager : MonoBehaviour
{
    public Text _ScoreText;
    public GameObject _Logo;
    private Camera _main;
    [SerializeField]
    private GameObject _spawn;
    [SerializeField]
    public GameObject _playerSelected;
    [SerializeField]
    public GameObject _mapSelected;
    public Slider player_healthBar;
    [SerializeField]
    private Image _fill;
    public float actualScore;
    [SerializeField]
    private GameObject _bar;
    [SerializeField]
    private GameObject _playerSelection;
    [SerializeField]
    private GameObject _mapSelection;
    public int deathCount = 0;
    // Start is called before the first frame update
    void Start()
    {
        actualScore = 0;
    }

    public void setHealth(float maxHealth)
    {
        player_healthBar.maxValue = maxHealth;
        player_healthBar.value = maxHealth;
        checkLife();
    } 

    public void doDamage(float damage)
    {
        player_healthBar.value -= damage;
        checkLife();
    }

    public void doHeal(float heal)
    {
        player_healthBar.value += heal;
        checkLife();
    }

    private void Update()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (Input.GetKeyDown("space") && (player == null || compareWinCondition())){
            deathCount = 0;
            _spawn.GetComponent<RespawnMachine>().quantitySpawn = 0;
            _Logo.SetActive(false);
            _mapSelection.SetActive(true);
            _playerSelection.SetActive(true);
            _spawn.GetComponent<RespawnMachine>().startSpawn = 0;
        }
    }

    public void incremetScore(float score)
    {
        actualScore += score;
        _ScoreText.text = "Score: " + actualScore;    
    }

    private void checkLife()
    {
        if (player_healthBar.value <= player_healthBar.maxValue * 25 / 100)
        {
            _fill.color = Color.red;
        }
        else if (player_healthBar.value <= player_healthBar.maxValue * 50 / 100)
        {
            _fill.color = Color.yellow;
        }
        else if (player_healthBar.value <= player_healthBar.maxValue)
        {
            _fill.color = Color.green;
        }
    }

    public void chooseMap()
    {
        _playerSelection.SetActive(false);
    }

    public void playLevel()
    {
        deathCount = 0;
        actualScore = 0;
        _ScoreText.text = "Score: " + actualScore;
        GameObject map = GameObject.FindGameObjectWithTag("Map");
        if(map != null)
            Destroy(map);
        Instantiate(_mapSelected, new Vector3(0, 0, 0), Quaternion.identity);
        _spawn.SetActive(true);
        _bar.SetActive(true);
        _mapSelection.SetActive(false);
        _playerSelection.SetActive(false);
        Instantiate(_playerSelected, new Vector3(0, 0, 0), Quaternion.identity);
    }

    public void deActiveSpawn()
    {
        _spawn.SetActive(false);
    }

    public bool compareWinCondition()
    {
        if (!_spawn.GetComponent<RespawnMachine>()._survival)
        {
            if (_spawn.GetComponent<RespawnMachine>().quantitySpawn - deathCount <= 0
             && _spawn.GetComponent<RespawnMachine>().maxEnemiesBeforeBoos <= deathCount)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                Destroy(player);
                return true;
            }
            else
            {
                return false;
            }
        } else
        {
            return false;
        }
    }

    public void updateSpawnLists(GameObject[] enemies, GameObject[] potions, GameObject[] ShieldsPwu, GameObject[] bosses)
    {
        if(enemies.Length > 0)
            _spawn.GetComponent<RespawnMachine>().alterEnemies(enemies);
        if(bosses.Length > 0)
            _spawn.GetComponent<RespawnMachine>().alterBosses(bosses);
        /*if(potions.Length > 0)
            _spawn.GetComponent<RespawnMachine>().alterPotions(potions);
        if (ShieldsPwu.Length > 0)
            _spawn.GetComponent<RespawnMachine>().alterPWU(ShieldsPwu);*/
    }

    public void survivalMap(bool survival)
    {
        _spawn.GetComponent<RespawnMachine>().isSurvival(survival);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MapSelection : MonoBehaviour
{

    public GameObject _map;
    public GameObject _Canvas; 
    private EndLevel _endLevel;

    [SerializeField]
    private GameObject[] _enemyList;
    [SerializeField]
    private GameObject[] _PotionsList;
    [SerializeField]
    private GameObject[] _ShieldPwu;
    [SerializeField]
    private GameObject[] _bosses;
    [SerializeField]
    private bool _isSurvival;
    public void mapSelected(GameObject _map)  
    {
        _Canvas.GetComponent<UiManager>()._mapSelected = _map;
        if (_isSurvival)
        {
            _Canvas.GetComponent<UiManager>().survivalMap(_isSurvival);
        }
        _Canvas.GetComponent<UiManager>().updateSpawnLists(_enemyList, _PotionsList, _ShieldPwu, _bosses);
        _Canvas.GetComponent<UiManager>().playLevel();
    }
}

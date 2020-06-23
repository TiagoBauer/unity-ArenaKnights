using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Selection : MonoBehaviour
{

    public GameObject _player;
    public GameObject _Canvas;
    private EndLevel _endLevel;
    public void playerSelected(GameObject _player)
    {
        _endLevel = GameObject.Find("EndLevel").GetComponent<EndLevel>();
        _endLevel.endLevel = false;
        _Canvas.GetComponent<UiManager>()._playerSelected = _player;
        _Canvas.GetComponent<UiManager>().chooseMap();
    }
}

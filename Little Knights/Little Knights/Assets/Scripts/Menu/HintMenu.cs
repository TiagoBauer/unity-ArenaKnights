using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HintMenu : MonoBehaviour
{
    public GameObject _helpScreen;
    private bool _isActive; 


    public void helpScreenShow()
    {
        _isActive = true;
        _helpScreen.SetActive(true);
        Time.timeScale = 0f;
    }

    public void Update()
    {
        if(_isActive && Input.GetKeyDown("space"))
        {
            gameObject.SetActive(false);
            _isActive = false;
        }
    }


}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class EndLevel : MonoBehaviour
{

    [SerializeField]
    private GameObject _panel;
    [SerializeField]
    private Image[] _endImgs;
    public bool endLevel = false;
    public int endType;
    [SerializeField]
    private Text _endText;
    [SerializeField]
    private Text _scoreText;
    private int flag = 0;
    private UiManager _uiManager;
    public bool clear = false;

    private void Start()
    {
        _uiManager = GameObject.Find("Canvas").GetComponent<UiManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (_uiManager.compareWinCondition())
        {
            endLevel = true;
            endType = 1;
        }
        if (endLevel && flag == 0)
        {
            flag = 1;
            if (endType == 1)
            {
                _endImgs[1].color = new Color(1f,1f,1f, 1f);
                _endImgs[0].color = new Color(1f, 1f, 1f, 0f);
                _endText.text = "Congratulations!\nYou became the champion of this arena!";
            } else
            {
                _endImgs[0].color = new Color(1f, 1f, 1f, 1f);
                _endImgs[1].color = new Color(1f, 1f, 1f, 0f);
                _endText.text = "You perished!\nwe'll be praying for your soul";
            }

            _scoreText.text = "Your final score is: " + _uiManager.actualScore + "\n\n Press \"SPACE\" to return to character selection";
            _uiManager.deActiveSpawn();
            StartCoroutine(waitEndAnimation());

        } else if(!endLevel)
        {
            flag = 0;
            _panel.SetActive(false);
            clear = false;
        }
    }

    IEnumerator waitEndAnimation()
    {
        _panel.SetActive(true);
        clear = false;
        yield return new WaitForSeconds(2f);
        clear = true;
    }

    public void closeEndLevel()
    {
        _panel.SetActive(false);
    }

}

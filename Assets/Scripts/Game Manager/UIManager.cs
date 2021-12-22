using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    public void Awake()
    {
        singletonInstantiation();
    }

    [SerializeField] private TMP_Text _scoreText;
    private void singletonInstantiation()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
    }

    public static UIManager getInstance()
    {
        return _instance;
    }

    public static GameObject getInstanceGameObject()
    {
        return _instance.gameObject;
    }

    public void UpdateScore(int score)
    {
        _scoreText.text = "Score : " + score.ToString();
    }
}

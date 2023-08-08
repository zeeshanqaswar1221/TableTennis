using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreTexts : MonoBehaviour
{
    public ScoreTexts Instance;
    public TextMeshProUGUI scoreMaster, scoreClient;

     void Awake()
    {
        Instance = this;
    }
}

using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DisplayScoreText : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI _text;

    private void Update()
    {
        _text.text = $"{SceneGod.SInstance.PlayerScore}";
    }
}

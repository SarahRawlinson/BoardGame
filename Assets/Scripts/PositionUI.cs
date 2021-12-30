using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PositionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text positionText;

    public void SetLabel(string text)
    {
        positionText.text = text;
    }
}

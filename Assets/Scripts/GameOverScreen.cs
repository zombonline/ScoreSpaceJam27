using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOverScreen : MonoBehaviour
{
    RectTransform rectTransform;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        LeanTween.move(rectTransform, Vector2.zero, 1f).setEaseOutBounce().setDelay(1f);
        FindObjectOfType<WaveSystem>().SetGameMode(GameMode.Build);
    }
}

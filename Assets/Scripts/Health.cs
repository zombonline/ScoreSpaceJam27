using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;

public class Health : MonoBehaviour
{
    [SerializeField] int startHitPoints;
    int hitPoints;

    [SerializeField] UnityEvent onDeath;
    [SerializeField] SpriteRenderer sprite;
    [SerializeField] float lengthSpriteFlashSeconds = .5f, lengthBetweenFlashesSeconds = .1f;


    private void Awake()
    {
        hitPoints = startHitPoints;
    }

    public void AdjustHitPoints(int adjustment)
    {
        Debug.Log(gameObject.name + "says ouch!");
        hitPoints += adjustment;
        StartCoroutine(FlashSpriteRoutine());
        if(hitPoints <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
        }

    }
    private IEnumerator FlashSpriteRoutine()
    {
        for (float i = 0; i < lengthSpriteFlashSeconds; i += lengthBetweenFlashesSeconds)
        {
            sprite.color = Color.red;
            yield return new WaitForSeconds(lengthBetweenFlashesSeconds);
            sprite.color = Color.white;
        }

    }
}

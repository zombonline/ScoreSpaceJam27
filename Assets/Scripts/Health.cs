using Spine.Unity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.U2D;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [SerializeField] int startHitPoints;
    int hitPoints;

    [SerializeField] UnityEvent onDeath;
    [SerializeField] float lengthSpriteFlashSeconds = .5f, lengthBetweenFlashesSeconds = .1f;

    [SerializeField] Slider healthSlider;

    [SerializeField] GameObject coinDrop;
    private void Awake()
    {
        hitPoints = startHitPoints;
        healthSlider.maxValue = startHitPoints;
        healthSlider.value = startHitPoints;
        healthSlider.gameObject.SetActive(false);
    }

    public void AdjustHitPoints(int adjustment)
    {
        Debug.Log(gameObject.name + "says ouch!");
        hitPoints += adjustment;
        UpdateHealthSlider();
        if(hitPoints <= 0)
        {
            onDeath.Invoke();
            Destroy(gameObject);
            var newCoin = Instantiate(coinDrop, new Vector3(transform.position.x,transform.position.y,-1f), Quaternion.identity);
            newCoin.GetComponent<Coin>().tile = GetComponent<EnemyMovement>().GetCurrentTile();
        }
    }
  

    private void UpdateHealthSlider()
    {
        healthSlider.gameObject.SetActive(true);
        healthSlider.value = hitPoints;
    }

}

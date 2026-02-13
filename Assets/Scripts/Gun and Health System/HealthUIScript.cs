using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class HealthUIScript : MonoBehaviour
{
    [SerializeField] private Image healthBar;
    [SerializeField] private TextMeshProUGUI dead_HealthTxt;

    private void OnEnable()
    {
        HealthSystem.OnTakeDamage += OnTakeDamage; 
        HealthSystem.OnDeath += OnDeath;
    }

    private void OnDeath()
    {
        dead_HealthTxt.SetText("DEAD");
    }

    private void OnTakeDamage(float obj)
    {
       healthBar.fillAmount = obj/100;
    }

    private void OnDisable()
    {
        HealthSystem.OnTakeDamage -= OnTakeDamage; 
        HealthSystem.OnDeath -= OnDeath;
    }
}

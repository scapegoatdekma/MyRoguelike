using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Animation;
using UnityEngine;
using UnityEngine.UI;   

public class PlayerHealthBar : MonoBehaviour
{
    PlayerStats player;
    float maxHealth;
    public Image health;
    public Image backgroundBar;
    Vector2 healthBarPos;

    void Start()
    {
        player = FindAnyObjectByType<PlayerStats>();
        maxHealth = player.characterData.MaxHealth;

        Debug.Log(maxHealth.ToString() + " " + player.CurrentHealth.ToString());
    }
    void Update()
    {
        healthBarPos = new Vector2(player.transform.position.x, player.transform.position.y - 0.8f);
    }

    void LateUpdate()
    {
        health.fillAmount = player.CurrentHealth / maxHealth;
        health.transform.position = healthBarPos;
        backgroundBar.transform.position = health.transform.position;
    }

}

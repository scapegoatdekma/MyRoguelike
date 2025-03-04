using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EnemyHealthBar : MonoBehaviour
{
    public GameObject prefab;
    public EnemyScriptableObject enemyData;
    [SerializeField] Image healthBar;
    [SerializeField] Image background;

    EnemyStats enemy;

    private void Start()
    {
        enemy  = FindObjectOfType<EnemyStats>();
        enemyData = FindObjectOfType<EnemyScriptableObject>();

        background.transform.position = healthBar.transform.position;

        //Instantiate(healthBar, healthBar.transform.position, Quaternion.identity);
       // Instantiate(background, background.transform.position, Quaternion.identity);
    }

    private void Update()
    {
        healthBar.transform.position = new Vector2(prefab.transform.position.x, prefab.transform.position.y - 2);
        healthBar.fillAmount = enemy.currentHealth / 100;
    }
    public void HideBar()
    {
        healthBar.enabled = false;
        background.enabled = false;
    }
}
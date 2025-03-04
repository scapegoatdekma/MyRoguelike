using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimator : MonoBehaviour
{
    Animator am;
    EnemyStats enemy;
    Transform enemyTransform;
    SpriteRenderer sr;

    Vector2 oldVector;

    private void Start()
    {
        am = GetComponent<Animator>();
        enemy = GetComponent<EnemyStats>();
        sr = GetComponent<SpriteRenderer>();
    }
    void Update()
    {
        SpriteDirectionChecker();
        oldVector = transform.position;
    }
    void SpriteDirectionChecker()
    {
        if (enemy.transform.position.x > oldVector.x)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}

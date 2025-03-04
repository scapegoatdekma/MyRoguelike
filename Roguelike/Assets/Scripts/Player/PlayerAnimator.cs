 using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimator : MonoBehaviour
{
    Animator am;
    PlayerMovement pm;
    SpriteRenderer sr;
    PlayerStats player;

    void Start()
    {
        player = GetComponent<PlayerStats>();
        am = GetComponent<Animator>();
        pm = GetComponent<PlayerMovement>();
        sr = GetComponent<SpriteRenderer>();
    }

    
    void Update()
    {
        if (pm.moveDir.x != 0 || pm.moveDir.y != 0)
        {
            am.SetBool("Move", true);

            SpriteDirectionChecker();
        }
        else
        {
            am.SetBool("Move", false);
        }
        if(player.CurrentHealth <= 0)
        {
            am.SetBool("Die", true);
        }
        else
        {
            am.SetBool("Die", false);
        }
        
    }
    void SpriteDirectionChecker()
    {
        if (pm.lastHorizontalVector < 0)
        {
            sr.flipX = true;
        }
        else
        {
            sr.flipX = false;
        }
    }
}

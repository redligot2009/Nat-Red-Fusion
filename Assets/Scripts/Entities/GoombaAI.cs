﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaAI : MonoBehaviour
{
    public int dirx = -1;

    public Vector2 moveSpeed = new Vector2(5,5);
    PhysicsObject po;

    private enum EnemyState
    {
        walking,
        falling,
        dead
    }

    private EnemyState state = EnemyState.falling;
    
    public Health health;

    void Start ()
    {
        po = GetComponent<PhysicsObject>();
        health = GetComponent<Health>();
        po.velocity.x = moveSpeed.x;
	}

    void GetHurt()
    {

    }

	void Update ()
    {
        if (health.health == 0)
        {
            state = EnemyState.dead;
        }
        if (state != EnemyState.dead)
        {
            if (po.collisions.below || po.collisions.above)
            {
                po.velocity.y = 0;
            }
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;
            if(po.collisions.below) state = EnemyState.walking;
            if (state == EnemyState.falling)
            {
            }
            if (state == EnemyState.walking || state != EnemyState.walking && po.gravityModifier == 0)
            {
                if (dirx == -1)
                {
                    po.velocity.x = -moveSpeed.x;
                    scale.x = -1;
                }
                else
                {
                    po.velocity.x = moveSpeed.x;
                    scale.x = 1;
                }
            }
            if (po.collisions.left) dirx = 1;
            else if (po.collisions.right) dirx = -1;

            // hit bullet

            bool hitBullet = po.CheckHorizontal(LayerMask.GetMask("bullet"));

            if(hitBullet)
            {
                health.Hurt();
            }

            //physics shit
            po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
            po.Move(po.velocity * Time.deltaTime);
        }
        else
        {

        }
    }
}

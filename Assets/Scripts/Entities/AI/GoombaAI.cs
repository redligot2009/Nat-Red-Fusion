﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaAI : MonoBehaviour
{
    public int dirx = -1;

    public Vector2 moveSpeed = new Vector2(5,5);
    PhysicsObject po;
    
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
        if (!health.dead)
        {
            Vector3 pos = transform.localPosition;
            Vector3 scale = transform.localScale;
            if (dirx == -1)
            {
                po.velocity.x = Mathf.Lerp(po.velocity.x,-moveSpeed.x,Time.deltaTime*2f);
                scale.x = -1;
            }
            else
            {
                po.velocity.x = Mathf.Lerp(po.velocity.x,moveSpeed.x,Time.deltaTime*2f);
                scale.x = 1;
            }
            if (po.collisions.left)
            {
                po.velocity.x = moveSpeed.x;
                dirx = 1;
            }
            else if (po.collisions.right)
            {
                po.velocity.x = -moveSpeed.x;
                dirx = -1;
            }

            // hit bullet

            RaycastHit2D hitBullet = po.CheckBoxHit(LayerMask.GetMask("bullet"));

            if(hitBullet)
            {
                if (health.hitTimer <= 0)
                {
                    if (hitBullet.point.y > po.coll.bounds.max.y - 0.15f)
                    {
                        po.velocity.x = 0f;
                    }
                    else
                    {
                        //Debug.Log(hitBullet.point.y.ToString() + " vs. " + po.coll.bounds.max.y);
                        
                        if (hitBullet.point.x >= transform.position.x)
                        {
                            po.velocity.x = -5f;
                        }
                        else
                        {
                            po.velocity.x = 5f;
                        }
                    }
                    Destroy(hitBullet.transform.gameObject);
                }
                health.Hurt();
            }
            
        }
        else
        {
            po.velocity.x = Mathf.Lerp(po.velocity.x, 0, Time.deltaTime * 5f);
            po.gravityModifier = 1;
        }
        if (po.collisions.below || po.collisions.above)
        {
            po.velocity.y = 0;
        }
        //physics shit
        po.velocity += Physics2D.gravity * po.gravityModifier * Time.deltaTime;
        po.Move(po.velocity * Time.deltaTime);
    }
}
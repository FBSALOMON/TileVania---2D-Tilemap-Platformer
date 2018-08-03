using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour {

    [SerializeField]
    float moveSpeed = 1f;

    Rigidbody2D myRigidbody2D;
	// Use this for initialization
	void Start () {
        myRigidbody2D = GetComponent<Rigidbody2D>();
        myRigidbody2D.velocity = new Vector2(moveSpeed, 0f);
    }
	
	// Update is called once per frame
	void Update () {
              
	}

    bool isFacingRight()
    {
        return transform.localScale.x > 0;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        FlipEnemyDirection();
    }

    private void FlipEnemyDirection()
    {
        myRigidbody2D.velocity = new Vector2(-myRigidbody2D.velocity.x, 0f);
        transform.localScale = new Vector2(-transform.localScale.x, 1f);
    }  
}

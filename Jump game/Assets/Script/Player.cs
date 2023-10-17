using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rigidBody = null;
    public float jumpPower = 2500;
    public bool isJump = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Jump();
            
    }



    public void Jump()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            if(!isJump)
            {
                isJump = true;
                rigidBody.AddForce(new Vector2(0, jumpPower));
            }
        }
    }


    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.tag == "Floor")
        {
            isJump = false;
        }
    }


}

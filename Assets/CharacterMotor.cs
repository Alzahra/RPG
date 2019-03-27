using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterMotor : MonoBehaviour {
    //Animation du Perso
    Animation animations;

    //Vitesse de déplacement
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    //Inputs de déplacement
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    //Variable concernant l'attaque
    public float attackCooldown;
    private bool isAttacking;
    private float currentCooldown;

    public Vector3 jumpSpeed; //hauteur de saut
    CapsuleCollider playerCollider;

    //Le personnage est il mort ?
    public bool isDead = false; 

    // Use this for initialization
    void Start () {
        animations = gameObject.GetComponent<Animation>();
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
		
	}

    bool IsGrounded()
    {
        return Physics.CheckCapsule(playerCollider.bounds.center, new Vector3(playerCollider.bounds.center.x, playerCollider.bounds.min.y - 0.1f, playerCollider.bounds.center.z), 0.09f, layerMask:3);
        //Vector3 dwn = transform.TransformDirection(Vector3.down); //résoud mais raycast
        //return (Physics.Raycast(transform.position, dwn, 1));
    }
	
	// Update is called once per frame
	void Update () {

        if (!isDead)
        {
            //avancer
            if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, walkSpeed * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }
                
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }

            //sprint
            if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
            {
                transform.Translate(0, 0, runSpeed * Time.deltaTime);
                animations.Play("run");
            }

            //reculer
            if (Input.GetKey(inputBack))
            {
                transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
                if (!isAttacking)
                {
                    animations.Play("walk");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
            }

            //tourner a gauche
            if (Input.GetKey(inputLeft))
            {
                transform.Rotate(0, -turnSpeed * Time.deltaTime,0);
          
            }

            //tourner a droite
            if (Input.GetKey(inputRight))
            {
                transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
            }

            //idle
            if (!Input.GetKey(inputFront) && !Input.GetKey(inputBack))
            {
                if (!isAttacking)
                {
                    animations.Play("idle");
                }

                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                    Attack();
                }
                
            }

            //sauter
            if(Input.GetKeyDown(KeyCode.Space) && IsGrounded()) //sans le down, tant qu'on appuie et pas juste a l'appui
            {
                //Preparation du saut (Necessaire en C#)
                Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
                v.y = jumpSpeed.y;

                //Saut
                gameObject.GetComponent<Rigidbody>().velocity = jumpSpeed;
            }
        }

        

        if (isAttacking)
        {
            currentCooldown -= Time.deltaTime;
        }

        if(currentCooldown <= 0)
        {
            currentCooldown = attackCooldown;
            isAttacking = false;
        }
    }

    public void Attack()
    {
        isAttacking = true;
        animations.Play("attack");
    }
}

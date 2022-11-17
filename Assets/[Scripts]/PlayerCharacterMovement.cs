using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterMovement : MonoBehaviour
{
    [SerializeField]
    [Range(0,10)] 
    private float MoveSpeed = 10;

    [SerializeField]
    public GameObject player;

    [SerializeField]
    new public Rigidbody2D rigidbody;

    [SerializeField]
    Animator animator;

    Traveller traveller;

    Vector2 startPos;

    // Save/Load
    [SerializeField,HideInInspector]
    public bool isLoading = false;

    public float xPosition;
    public float yPosition;

    private string xKey = "Players X Position";
    private string yKey = "Players Y Position";

    Vector2 LastSavedLocation;

    void Awake()
    {
        startPos = transform.position;
        if (isLoading)
        {
            Loading();
        }
        else
        {
            transform.position = startPos;
        }
    }


    public void Loading()
    {
        if (xKey != null && yKey != null)
        {
            xPosition = PlayerPrefs.GetFloat(xKey);
            yPosition = PlayerPrefs.GetFloat(yKey);
            Debug.Log("xKey: " + xPosition);
            Debug.Log("yKey: " + yPosition);
            LastSavedLocation = new Vector2(xPosition, yPosition);
            Debug.Log("playerPosition: " + LastSavedLocation);
            player.transform.position = LastSavedLocation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(isLoading);

        if (Input.GetKeyUp(KeyCode.K))
        {
            Save(); 
        }

        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");

        Vector3 oldPosition = transform.position;

        Vector3 velocity = new Vector3(xInput, yInput, 0) * MoveSpeed;
        animator.SetFloat("Speed", velocity.sqrMagnitude);

        // Choose Direction
        CardinalDirection facing = CardinalDirection.SOUTH;

        if (velocity.x > 0)
        {
            facing = CardinalDirection.EAST;
        }
        else if (velocity.x < 0)
        {
            facing = CardinalDirection.WEST;
        }
        else if (velocity.y < 0)
        {
            facing = CardinalDirection.SOUTH;
        }
        else if (velocity.y > 0)
        {
            facing = CardinalDirection.NORTH;
        }

        animator.SetInteger("FacingDirection", (int)facing);

        rigidbody.MovePosition(oldPosition + velocity * Time.deltaTime);

        xPosition = player.transform.position.x;
        yPosition = player.transform.position.y;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Player Collided with " + collision.gameObject.name);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Player Triggered with: " + collision.gameObject.name);
    }

    void Save()
    {
        PlayerPrefs.SetFloat(xKey, xPosition);
        PlayerPrefs.SetFloat(yKey, yPosition);
        Debug.Log("Players X Position: " + xPosition);
        Debug.Log("Players Y Position: " + yPosition);
    }

}

using UnityEngine;
using System.Collections;

public class FarmerInsideCage : MonoBehaviour
{
    private Vector3 moveVector;
    private FarmerScript farmerScript;
    public bool isMoving = true;
    public float moveIntevals;
    public float moveCounter;

    void Start()
    {
        farmerScript = GetComponent<FarmerScript>();
        moveVector = RandomDirection();
        moveCounter = moveIntevals;
    }

    void Update()
    {
        if (moveCounter < 0)
        {
            moveVector = RandomDirection();
            moveCounter = Random.Range(0f, moveIntevals);
        }
        if (isMoving)
        {
            transform.Translate(moveVector * farmerScript.speed * Time.deltaTime);
            moveCounter -= Time.deltaTime;
        }
    }

    // Sets a random direction
    public Vector2 RandomDirection()
    {
        int r = Random.Range(0, 4);
        switch (r)
        {
            case 0:
                return Vector2.right;
            case 1:
                return Vector2.up;
            case 2:
                return Vector2.left;
            case 3:
                return Vector2.down;
            default:
                return Vector2.zero;
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.tag == "Geometry")
        {
            moveVector = Vector2.zero;
        }
    }
}
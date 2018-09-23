using UnityEngine;
using System.Collections;

public class FarmerScript : MonoBehaviour
{
    private Rigidbody2D rb;
    private GameManager gameManager;
    private int frontTarget;
    private int behindTarget;
    private Vector3 currentTargetVector;
    private GameObject lightObject;

    [Header("Walking")]
    public GameObject startPosition;
    public GameObject startFrontTarget;
    public GameObject startBehindTarget;
    public float speed;

    [Header("Farmer Boundaries")]
    [Range(0f, 20f)]
    public float width;
    [Range(0f, 20f)]
    public float height;
    public GameObject[] points;
    public Vector3 offset;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        gameManager = GameObject.Find("Game Manager").GetComponent<GameManager>();
        lightObject = transform.FindChild("Light").transform.gameObject;

        points[0].transform.position = new Vector3(width, height, 1f) + offset;
        points[1].transform.position = new Vector3(-width, height, 1f) + offset;
        points[2].transform.position = new Vector3(-width, -height, 1f) + offset;
        points[3].transform.position = new Vector3(width, -height, 1f) + offset;

        transform.position = startPosition.transform.position;
        frontTarget = 1;
        behindTarget = 0;

        currentTargetVector = startFrontTarget.transform.position - transform.position;
        currentTargetVector /= currentTargetVector.magnitude;
    }

    void Update()
    {
        transform.position += currentTargetVector * speed * Time.deltaTime;

        if (speed > 0)
        {
            switch (frontTarget)
            {
                case 1:
                    if (transform.position.x < points[1].transform.position.x)
                    {
                        currentTargetVector = points[2].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 2;
                        behindTarget = 1;
                        lightObject.transform.Rotate(0, 0, 90);
                    }
                    break;
                case 2:
                    if (transform.position.y < points[2].transform.position.y)
                    {
                        currentTargetVector = points[3].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 3;
                        behindTarget = 2;
                        lightObject.transform.Rotate(0, 0, 90);
                    }
                    break;
                case 3:
                    if (transform.position.x > points[3].transform.position.x)
                    {
                        currentTargetVector = points[0].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 0;
                        behindTarget = 3;
                        lightObject.transform.Rotate(0, 0, 90);
                    }
                    break;
                case 0:
                    if (transform.position.y > points[0].transform.position.y)
                    {
                        currentTargetVector = points[1].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 1;
                        behindTarget = 0;
                        lightObject.transform.Rotate(0, 0, 90);
                    }
                    break;
                default:
                    break;
            }
        }
        else
        {
            switch (behindTarget)
            {
                case 1:
                    if (transform.position.y > points[1].transform.position.y)
                    {
                        currentTargetVector = points[0].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 1;
                        behindTarget = 0;
                        lightObject.transform.Rotate(0, 0, -90);
                    }
                    break;
                case 2:
                    if (transform.position.x < points[2].transform.position.x)
                    {
                        currentTargetVector = points[1].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 2;
                        behindTarget = 1;
                        lightObject.transform.Rotate(0, 0, -90);
                    }
                    break;
                case 3:
                    if (transform.position.y < points[3].transform.position.y)
                    {
                        currentTargetVector = points[2].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 3;
                        behindTarget = 2;
                        lightObject.transform.Rotate(0, 0, -90);
                    }
                    break;
                case 0:
                    if (transform.position.x > points[0].transform.position.x)
                    {
                        currentTargetVector = points[1].transform.position - transform.position;
                        currentTargetVector /= currentTargetVector.magnitude;
                        frontTarget = 0;
                        behindTarget = 3;
                        lightObject.transform.Rotate(0, 0, -90);
                    }
                    break;
                default:
                    break;
            }
        }




        points[0].transform.position = new Vector3(width, height, 1f) + offset;
        points[1].transform.position = new Vector3(-width, height, 1f) + offset;
        points[2].transform.position = new Vector3(-width, -height, 1f) + offset;
        points[3].transform.position = new Vector3(width, -height, 1f) + offset;

        // Debug
        Debug.DrawLine(points[0].transform.position, points[1].transform.position, Color.magenta);
        Debug.DrawLine(points[1].transform.position, points[2].transform.position, Color.magenta);
        Debug.DrawLine(points[2].transform.position, points[3].transform.position, Color.magenta);
        Debug.DrawLine(points[3].transform.position, points[0].transform.position, Color.magenta);
    }
}
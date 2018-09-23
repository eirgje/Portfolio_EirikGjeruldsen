using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_FinalAbility : MonoBehaviour {

    [SerializeField]
    private GameObject GiantSnowball = null;
    [SerializeField]
    private GameObject snowWall = null;
    private Transform spawnPoint = null;

    [SerializeField]
    private Transform LookTargets = null;
    [SerializeField]
    private Transform lookDirectionLeft = null;
    [SerializeField]
    private Transform lookDirectionRight = null;

    [SerializeField]
    private Transform lookDirectionMiddle = null;

    private enum direction
    {
        left,
        right,
        middle
    }

    private direction nextDirection = direction.middle;

    int lastDirection = 2;

    private void Awake()
    {
        spawnPoint = transform.GetChild(3).transform;
        if(lookDirectionLeft == null)
            lookDirectionLeft = LookTargets.GetChild(0).transform;
        if (lookDirectionRight == null)
            lookDirectionRight = LookTargets.GetChild(1).transform;
        if (lookDirectionMiddle == null)
            lookDirectionMiddle = LookTargets.GetChild(2).transform;
    }

    public void SpawnNewSnowball()
    {

        if (nextDirection == direction.left)
        {
            transform.LookAt(new Vector3(lookDirectionRight.position.x, transform.position.y, lookDirectionRight.position.z));

            GameObject giantSnowball = Instantiate(GiantSnowball, lookDirectionLeft.position, Quaternion.identity, null);
            giantSnowball.GetComponent<GiantSnowballScript>().SetForward(lookDirectionRight.forward);
            giantSnowball.GetComponent<GiantSnowballScript>().SetStartPoint(lookDirectionRight.position);

            lastDirection = 0;
        }
        else if (nextDirection == direction.right)
        {
            transform.LookAt(new Vector3(lookDirectionLeft.position.x, transform.position.y, lookDirectionLeft.position.z));

            GameObject giantSnowball = Instantiate(GiantSnowball, lookDirectionLeft.position, Quaternion.identity, null);
            giantSnowball.GetComponent<GiantSnowballScript>().SetForward(lookDirectionLeft.forward);
            giantSnowball.GetComponent<GiantSnowballScript>().SetStartPoint(lookDirectionLeft.position);

            lastDirection = 1;
        }
        else if (nextDirection == direction.middle)
        {
            transform.LookAt(new Vector3(lookDirectionMiddle.position.x, transform.position.y, lookDirectionMiddle.position.z));

            GameObject giantSnowball = Instantiate(snowWall, lookDirectionMiddle.position, Quaternion.identity, null);
            giantSnowball.GetComponent<GiantSnowballScript>().SetForward(lookDirectionMiddle.forward);
            giantSnowball.GetComponent<GiantSnowballScript>().SetStartPoint(lookDirectionMiddle.position);

            lastDirection = 2;
        }

        SetRandomNextDirection();

    }

    private void SetRandomNextDirection()
    {
        bool foundNewDirection = false;

        int randomDirection = 0;

        while (!foundNewDirection)
        {
            float randomNumber = Random.Range(0, 3);

            if (randomNumber < 1f)
                randomDirection = 0;
            else if (randomNumber >= 1f && randomNumber < 2f)
                randomDirection = 1;
            else if (randomNumber >= 2f && randomNumber <= 3f)
                randomDirection = 2;

            if (randomDirection != lastDirection)
                foundNewDirection = true;
        }

        switch (randomDirection)
        {
            case 0:
                nextDirection = direction.left;
                break;
            case 1:
                nextDirection = direction.right;
                break;
            case 2:
                nextDirection = direction.middle;
                break;

            default:
                break;
        }

        Debug.Log("Next direction is: " + nextDirection);

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(lookDirectionRight.position, lookDirectionRight.forward * 150f);
        Gizmos.color = Color.blue;
        Gizmos.DrawLine(lookDirectionLeft.position, lookDirectionLeft.forward * 150f);
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(lookDirectionMiddle.position, lookDirectionMiddle.forward * 150f);
    }
}

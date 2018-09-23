using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Snowman_ShadowHandler : MonoBehaviour {

    [SerializeField]
    private Transform shadowsTransform = null;

    private void Update()
    {
        Ray shadowPositionRay = new Ray(transform.position, Vector3.down);
        RaycastHit shadowHit;

        if (Physics.Raycast(shadowPositionRay, out shadowHit, 30f))
        {
            shadowsTransform.position = shadowHit.point + Vector3.up * 0.1f;

        }
    }

}

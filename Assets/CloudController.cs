using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CloudController : MonoBehaviour
{

    public Vector2 velocity;

    // Update is called once per frame
    void FixedUpdate()
    {

        Vector2 deltaPosition = Time.deltaTime* velocity;

        this.transform.position += new Vector3(deltaPosition.x, deltaPosition.y, 0.0f);

    }
}

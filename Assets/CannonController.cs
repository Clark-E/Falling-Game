using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CannonController : MonoBehaviour
{

    public GameObject cloudPrefab;

    float fireTimer = 0.0f;
    public float PERIOD = 3.0f;
    public float SPEED;

    // Update is called once per frame
    void FixedUpdate()
    {

        fireTimer += Time.deltaTime;

        //print(this.transform.right);

        if (fireTimer >= PERIOD) {

            fireTimer -= PERIOD;

            GameObject newCloud = Instantiate(cloudPrefab, this.transform.position, this.transform.rotation);

            CloudController cloudController = newCloud.GetComponent<CloudController>();

            Vector2 cloudVelocity = new Vector2(this.transform.right.x, this.transform.right.y);

            cloudVelocity *= SPEED;
            cloudVelocity.x *= Mathf.Sign(this.transform.localScale.x);
            cloudVelocity.y *= Mathf.Sign(this.transform.localScale.y);

            cloudController.velocity = cloudVelocity;

        }

    }
}

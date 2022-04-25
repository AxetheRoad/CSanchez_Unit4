using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    Rigidbody rbPlayer;
    GameObject focalPoint;
    Renderer renderPlayer;
    public float speed = 50f;
    public float powerUpSpeed = 10.0f;
    public GameObject powerUpIn;

    bool hasPowerUp = false;
    // Start is called before the first frame update
    void Start()
    {
        rbPlayer = GetComponent<Rigidbody>();
        focalPoint = GameObject.Find("Focal Point");
        renderPlayer = GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardInput = Input.GetAxis("Vertical");
        float magnitude = forwardInput * speed * Time.deltaTime;
        rbPlayer.AddForce(focalPoint.transform.forward * magnitude, ForceMode.Force);

        if (forwardInput > 0)
        {
            renderPlayer.material.color = new Color(1.0f - forwardInput, 1.0f, 1.0f - forwardInput);
        }
       else
        {
            renderPlayer.material.color = new Color(1.0f + forwardInput, 1.0f, 1.0f + forwardInput);
        }
        powerUpIn.transform.position = transform.position;
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            StartCoroutine(PowerUpCountDown());
            powerUpIn.SetActive(true);
        }

    }

    private void OnCollisionEnter(Collision collision)
    {
        
        if (hasPowerUp && collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("Player has collided with " + collision.gameObject + "with powerup set to " + hasPowerUp);
            Rigidbody rbEnemy = collision.gameObject.GetComponent<Rigidbody>();
            Vector3 awayDr = collision.gameObject.transform.position - transform.position;
            rbEnemy.AddForce(awayDr * powerUpSpeed, ForceMode.Impulse);
        }

    }

    IEnumerator PowerUpCountDown()
    {
       yield return new WaitForSeconds(8);
        hasPowerUp = false;
        powerUpIn.SetActive(false);
    }
}

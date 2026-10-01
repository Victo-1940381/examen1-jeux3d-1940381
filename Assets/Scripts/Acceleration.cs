using UnityEngine;

public class Acceleration : MonoBehaviour
{
    private BoxCollider trigger;
    private Rigidbody boule;


    private void Start()
    {
        trigger = GetComponent<BoxCollider>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Boule"))
        {
            if (other.GetComponent<Boule>().NombreAccel <= 2)
            {
                other.GetComponent<Boule>().AjouterAccel();
                Destroy(this.gameObject);
            }

        }
    }
}

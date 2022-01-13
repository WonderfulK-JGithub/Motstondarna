using UnityEngine;

public class JumpPad : MonoBehaviour
{
    [Range(0, 20)]
    public float strength;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<Rigidbody>() != null)
        {
            Rigidbody rb = collision.gameObject.GetComponent<Rigidbody>();
            rb.velocity = new Vector3(rb.velocity.x, strength, rb.velocity.z);
        }
    }
}

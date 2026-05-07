using UnityEngine;

public class TrashBin : MonoBehaviour
{
    public string acceptedTag; 

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(acceptedTag))
        {
            AcceptTrash(other.gameObject);
        }
        else
        {
            RejectTrash(other.gameObject);
        }
    }

    void AcceptTrash(GameObject trash)
    {
        Debug.Log("Correct! " + trash.name + " sorted.");
        Destroy(trash); 
    }

    void RejectTrash(GameObject trash)
    {
        Debug.Log("Wrong Bin! " + trash.name + " does not belong here.");
        
        Rigidbody rb = trash.GetComponent<Rigidbody>();
        if (rb != null)
        {
            rb.AddForce(Vector3.up * 5f, ForceMode.Impulse);
        }
    }
}
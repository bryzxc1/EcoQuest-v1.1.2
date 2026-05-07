using UnityEngine;

public class Sorting : MonoBehaviour
{
    public string acceptedTag; 

    private void OnTriggerEnter(Collider other)
    {
        
        Debug.Log("Something entered the bin: " + other.name + " with Tag: " + other.tag);

        if (other.CompareTag(acceptedTag))
        {
            Debug.Log("<color=green>SUCCESS!</color> Matching tag found. Destroying " + other.name);
            Destroy(other.gameObject);
        }
        else
        {
            Debug.Log("<color=yellow>TAG MISMATCH:</color> Bin wants [" + acceptedTag + "] but object is [" + other.tag + "]");
        }
    }
}
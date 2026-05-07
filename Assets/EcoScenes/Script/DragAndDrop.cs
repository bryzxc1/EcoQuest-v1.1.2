using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private Vector3 mOffset;
    private float mZCoord;
    private Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void OnMouseDown()
    {
        if (rb == null) return;
        rb.useGravity = false; 
        
        mZCoord = Camera.main.WorldToScreenPoint(gameObject.transform.position).z;
        mOffset = gameObject.transform.position - GetMouseWorldPos();
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = mZCoord;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }

    void OnMouseDrag()
    {
        if (rb == null) return;
        // Use MovePosition so it stays solid!
        rb.MovePosition(GetMouseWorldPos() + mOffset);
    }

    void OnMouseUp()
    {
        if (rb == null) return;
        rb.useGravity = true; 
    }
}
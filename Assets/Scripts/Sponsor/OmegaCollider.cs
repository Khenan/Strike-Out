using UnityEngine;

public class OmegaCollider : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D col)
    {
        col.transform.localScale *= 0.5f;
    }
    
    private void OnTriggerExit2D(Collider2D col)
    {
        col.transform.localScale /= 0.5f;
        if (col.transform.localScale.magnitude > 0.5f)
        {
            col.transform.localScale = Vector2.one * 0.5f;
        }
    }
}
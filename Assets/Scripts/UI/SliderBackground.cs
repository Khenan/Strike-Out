using UnityEngine;

public class SliderBackground : MonoBehaviour
{
    [SerializeField, Tooltip("Position en X pour respawn")] private float m_posXRespawn;
    [SerializeField, Tooltip("Position en X pour s'enlever")] private float m_posXEnd;
    [SerializeField, Tooltip("Si ça va dans l'autre sens")] private bool m_reverse;
    [SerializeField, Tooltip("Vitesse de défilement")] private float m_speed;
    void FixedUpdate()
    {
        if (m_reverse)
        {
            transform.Translate(m_speed * Time.deltaTime * Vector3.left);

            if (transform.localPosition.x < m_posXEnd)
                transform.localPosition = new Vector3(m_posXRespawn, transform.localPosition.y, transform.localPosition.z);
                
            return;
        }
        transform.Translate(m_speed * Time.deltaTime * Vector3.right);
        if (transform.localPosition.x > m_posXEnd)
            transform.localPosition = new Vector3(m_posXRespawn, transform.localPosition.y, transform.localPosition.z);
        
    }
}

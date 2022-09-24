using UnityEngine;

public class ParticleDestroyer : MonoBehaviour
{
    void OnEnable()
    {
        Destroy(this.gameObject, 2);
    }
}

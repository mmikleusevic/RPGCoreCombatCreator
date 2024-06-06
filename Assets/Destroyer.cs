using UnityEngine;

public class Destroyer : MonoBehaviour
{
    [SerializeField] private Transform parent;

    public void Destroy()
    {
        Destroy(parent);
    }
}

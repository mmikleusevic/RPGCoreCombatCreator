using Cinemachine;
using UnityEngine;

namespace RPG.GameEditor
{
    public class CameraSeeThrough : MonoBehaviour
    {
        [SerializeField] private Transform player;
        [SerializeField] private LayerMask obstacleLayer;

        private CinemachineVirtualCamera cam;
        private Vector3 originalPosition;

        private void Start()
        {
            cam = GetComponent<CinemachineVirtualCamera>();
            originalPosition = cam.transform.position;
        }

        private void Update()
        {
            if (player == null || cam == null) return;

            Vector3 directionToPlayer = player.position - transform.position;

            RaycastHit hit;
            if (Physics.Raycast(transform.position, directionToPlayer, out hit, Mathf.Infinity, obstacleLayer))
            {
                Vector3 newPosition = hit.point;
                transform.position = newPosition;
                transform.LookAt(player);
            }
            else
            {
                transform.position = originalPosition;
                transform.LookAt(player);
            }
        }
    }
}
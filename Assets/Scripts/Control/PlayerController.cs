using RPG.Attributes;
using RPG.Combat;
using RPG.Movement;
using System;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;

namespace RPG.Control
{
    public class PlayerController : MonoBehaviour
    {
        private Mover mover;
        private Fighter fighter;
        private Health health;

        [Serializable]
        private struct CursorMapping
        {
            public CursorType cursorType;
            public Texture2D texture;
            public Vector2 hotspot;
        }

        [SerializeField] private CursorMapping[] cursorMappings = null;
        [SerializeField] private float maxNavMeshProjectionDistance = 1f;
        [SerializeField] private float raycastRadius = 1f;

        private void Start()
        {
            mover = GetComponent<Mover>();
            fighter = GetComponent<Fighter>();
            health = GetComponent<Health>();
        }

        private void Update()
        {
            if (InteractWithUI()) return;
            if (health.IsDead())
            {
                SetCursor(CursorType.None);
                return;
            }

            if (InteractWithComponent()) return;
            if (InteractWithMovement()) return;

            SetCursor(CursorType.None);
        }

        private bool InteractWithUI()
        {
            if (EventSystem.current.IsPointerOverGameObject())
            {
                SetCursor(CursorType.UI);
                return true;
            }

            return false;
        }

        private bool InteractWithComponent()
        {
            RaycastHit[] hits = RaycastAllSorted();
            foreach (RaycastHit hit in hits)
            {
                IRaycastable[] raycasts = hit.transform.GetComponents<IRaycastable>();

                foreach (IRaycastable raycast in raycasts)
                {
                    if (raycast.HandleRaycast(this))
                    {
                        SetCursor(raycast.GetCursorType());
                        return true;
                    }
                }
            }

            return false;
        }

        private RaycastHit[] RaycastAllSorted()
        {
            RaycastHit[] hits = Physics.SphereCastAll(GetMouseRay(), raycastRadius);
            float[] distances = new float[hits.Length];

            for (int i = 0; i < hits.Length; i++)
            {
                distances[i] = hits[i].distance;
            }

            Array.Sort(distances, hits);

            return hits;
        }

        private void SetCursor(CursorType cursorType)
        {
            CursorMapping cursorMapping = GetCursorMapping(cursorType);
            Cursor.SetCursor(cursorMapping.texture, cursorMapping.hotspot, CursorMode.Auto);
        }

        private CursorMapping GetCursorMapping(CursorType cursorType)
        {
            foreach (CursorMapping cursorMapping in cursorMappings)
            {
                if (cursorMapping.cursorType == cursorType) return cursorMapping;
            }

            return cursorMappings[0];
        }

        private bool InteractWithMovement()
        {
            Vector3 target;
            bool hasHit = RaycastNavMesh(out target);

            if (hasHit)
            {
                if (!mover.CanMoveTo(target)) return false;

                if (Input.GetMouseButton(0))
                {
                    mover.StartMoveAction(target, 1f);
                }

                SetCursor(CursorType.Movement);

                return true;
            }

            return false;
        }

        private bool RaycastNavMesh(out Vector3 target)
        {
            target = new Vector3();

            RaycastHit hit;
            bool hasHit = Physics.Raycast(GetMouseRay(), out hit);
            if (!hasHit) return false;

            NavMeshHit navMeshHit;
            bool hasCastToNavMesh = NavMesh.SamplePosition(hit.point, out navMeshHit, maxNavMeshProjectionDistance, NavMesh.AllAreas);
            if (!hasCastToNavMesh) return false;

            target = navMeshHit.position;

            if (!mover.CanMoveTo(target)) return false;

            return true;
        }

        private Ray GetMouseRay()
        {
            return Camera.main.ScreenPointToRay(Input.mousePosition);
        }
    }
}
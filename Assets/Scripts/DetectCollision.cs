using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace DetectCollisionExtension
{
    public static class DetectCollision
    {
        public static bool isColliding(Vector2 direction, Transform transform, Vector3 offset, float margin = 1f, float NOofRays = 5)
        {
            bool sideway = direction.x != 0;
            Ray ray;
            BoxCollider boxCollider = transform.GetComponent<BoxCollider>();
            RaycastHit HitInfo;
            Vector3 origin;

            float DistanceBetweenRays;
            if (sideway)
            {
                DistanceBetweenRays = (boxCollider.bounds.size.y) / (NOofRays - 1);
                origin = new Vector3(boxCollider.bounds.center.x + offset.x, boxCollider.bounds.min.y + offset.y, transform.position.z);
            }
            else
            {
                DistanceBetweenRays = (boxCollider.bounds.size.x) / (NOofRays - 1);
                origin = new Vector3(boxCollider.bounds.min.x + offset.x, boxCollider.bounds.center.y + offset.y, transform.position.z);
            }



            for (int i = 0; i < NOofRays; i++)
            {
                // Ray to be casted.
                ray = new Ray(origin, direction);
                //Draw ray on screen to see visually. Remember visual length is not actual length.

                if (sideway)
                {
                    Debug.DrawRay(origin, direction * boxCollider.size.x / 2, Color.yellow);
                    if (Physics.Raycast(ray, out HitInfo, boxCollider.size.x / 2))
                    {
                        return true;
                    }
                }
                Debug.DrawRay(origin, direction* boxCollider.size.y / 2, Color.yellow);
                if (Physics.Raycast(ray, out HitInfo, boxCollider.size.y / 2))
                {
                    return true;
                }

                if (sideway)
                    origin += new Vector3(0, DistanceBetweenRays, 0);
                else origin += new Vector3(DistanceBetweenRays, 0, 0);
            }

            return false;
        }
    }
}
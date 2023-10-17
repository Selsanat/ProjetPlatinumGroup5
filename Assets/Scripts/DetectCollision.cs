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
            CharacterController characterController = transform.GetComponent<CharacterController>();
            RaycastHit HitInfo;
            Vector3 origin;

            float DistanceBetweenRays;
            if (sideway)
            {
                DistanceBetweenRays = (characterController.bounds.size.y) / (NOofRays - 1);
                origin = new Vector3(characterController.bounds.center.x + offset.x, characterController.bounds.min.y + offset.y, transform.position.z);
            }
            else
            {
                DistanceBetweenRays = (characterController.bounds.size.x) / (NOofRays - 1);
                origin = new Vector3(characterController.bounds.min.x + offset.x, characterController.bounds.center.y + offset.y, transform.position.z);
            }



            for (int i = 0; i < NOofRays; i++)
            {
                // Ray to be casted.
                ray = new Ray(origin, direction);
                //Draw ray on screen to see visually. Remember visual length is not actual length.

                if (sideway)
                {
                    Debug.DrawRay(origin, direction * characterController.radius / 2, Color.yellow);
                    if (Physics.Raycast(ray, out HitInfo, characterController.radius + characterController.skinWidth / 2))
                    {
                        return true;
                    }
                }
                Debug.DrawRay(origin, direction* characterController.height/ 2, Color.yellow);
                if (Physics.Raycast(ray, out HitInfo, characterController.height / 2 + characterController.skinWidth))
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
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StackedBeans.Utils
{
    public static class StackedBeansExtensions
    {
        public static T GetRandom<T>(this List<T> list)
        {
            return list[Random.Range(0, list.Count)];
        }
        public static T GetRandomAndRemove<T>(this List<T> list)
        {
            T temp = list.GetRandom();
            list.Remove(temp);
            return temp;
        }

        public static void DestroyVisualChildren(this Transform thisTransform)
        {
            foreach (Transform childTransform in thisTransform.GetComponentsInChildren<Transform>())
            {
                if (childTransform != thisTransform)
                {
                    if (childTransform.GetComponentInChildren<MeshRenderer>() != null)
                        Object.Destroy(childTransform.GetComponentInChildren<MeshRenderer>());

                    if (childTransform.GetComponent<MeshFilter>() != null)
                        Object.Destroy(childTransform.GetComponent<MeshFilter>());
                }
            }
        }

        public static Transform GetClosestTransform(this List<Transform> transforms, Transform targetTransform)
        {
            Transform closestTransform = null;
            float closestDistance = float.MaxValue;
            foreach (var transform in transforms)
            {
                float dist = Vector3.Distance(transform.position, targetTransform.position);
                if (dist < closestDistance)
                {
                    closestDistance = dist;
                    closestTransform = transform;
                }
            }
            return closestTransform;
        }
        
        public static Vector3 GetBoundsWorldPosition(this Collider collider,DirectionLeftToRight LtoR,DirectionBackToFront BtoF,DirectionTopToBottom TtoB = DirectionTopToBottom.Top)
        {
            Vector3 pos = collider.transform.position;
            
            switch (LtoR)
            {
                case DirectionLeftToRight.Left:
                    pos.x -= collider.bounds.extents.x;
                    break;
                case DirectionLeftToRight.Right:
                    pos.x += collider.bounds.extents.x;
                    break;
            }
            
            switch (BtoF)
            {
                case DirectionBackToFront.Back:
                    pos.z += collider.bounds.extents.z;
                    break;
                case DirectionBackToFront.Front:
                    pos.z -= collider.bounds.extents.z;
                    break;
            }
            
            switch (TtoB)
            {
                case DirectionTopToBottom.Bottom:
                    pos.y -= collider.bounds.extents.y;
                    break;
                case DirectionTopToBottom.Top:
                    pos.y += collider.bounds.extents.y;
                    break;
            }
            return pos;
        }
    }

}

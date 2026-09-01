using UnityEngine;

namespace PROJ.UI
{
    public class WorldSpaceUIBillboard : MonoBehaviour
    {
        private Transform mainCameraTransform;

        private void Start()
        {
            if (Camera.main != null)
            {
                mainCameraTransform = Camera.main.transform;
            }
        }

        private void LateUpdate()
        {
            if (mainCameraTransform != null)
            {
                transform.LookAt(transform.position + mainCameraTransform.rotation * Vector3.forward,
                                 mainCameraTransform.rotation * Vector3.up);
            }
        }
    }
}

using System.Collections;
using UnityEngine;

internal static class WeaponRecoilRoutine
{
    public static IEnumerator Play(
        Transform gunModel,
        Vector3 originalLocalPosition,
        Quaternion originalLocalRotation,
        Vector3 recoilPositionOffset,
        Vector3 recoilRotationOffset,
        float restoreSpeed)
    {
        Vector3 targetRecoilPosition = originalLocalPosition + recoilPositionOffset;
        Quaternion targetRecoilRotation = originalLocalRotation * Quaternion.Euler(recoilRotationOffset);

        gunModel.localPosition = targetRecoilPosition;
        gunModel.localRotation = targetRecoilRotation;

        while (Vector3.Distance(gunModel.localPosition, originalLocalPosition) > 0.001f ||
               Quaternion.Angle(gunModel.localRotation, originalLocalRotation) > 0.1f)
        {
            gunModel.localPosition = Vector3.Lerp(
                gunModel.localPosition,
                originalLocalPosition,
                Time.deltaTime * restoreSpeed);
            gunModel.localRotation = Quaternion.Slerp(
                gunModel.localRotation,
                originalLocalRotation,
                Time.deltaTime * restoreSpeed);
            yield return null;
        }

        gunModel.localPosition = originalLocalPosition;
        gunModel.localRotation = originalLocalRotation;
    }
}

using UnityEngine;

public static class ScriptHelper
{
    public static Quaternion LookAt2D(Transform aim, Transform TransformObject)
    {
        Vector3 diff = aim.position - TransformObject.position;
        diff.Normalize();
        float rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;

        return Quaternion.Euler(0f, 0f, rot_z - 180);
    }
}
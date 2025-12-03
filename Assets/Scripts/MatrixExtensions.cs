using UnityEngine;

public static class MatrixExtensions
{
    public static Vector3 GetPosition(this Matrix4x4 m)
    {
        return m.GetColumn(3);
    }

    public static Vector3 GetScale(this Matrix4x4 m)
    {
        return new Vector3(
            m.GetColumn(0).magnitude,
            m.GetColumn(1).magnitude,
            m.GetColumn(2).magnitude
        );
    }
}

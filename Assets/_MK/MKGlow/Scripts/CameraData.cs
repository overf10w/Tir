using UnityEngine;

namespace MK.Glow
{
    /// <summary>
    /// Pipeline Independent pass of necessary camera data
    /// </summary>
    internal abstract class CameraData
    {
        internal int width, height;
        internal bool stereoEnabled;
        internal float aspect;
        internal Matrix4x4 worldToCameraMatrix;
    }
}

namespace Ode.Net.Collision
{
    /// <summary>
    /// Specifies the possible spatial axes order for the
    /// sweep-and-prune collision space.
    /// </summary>
    public enum AxisOrder
    {
        /// <summary>
        /// Specifies the XYZ order.
        /// </summary>
        Xyz = ((0) | (1 << 2) | (2 << 4)),

        /// <summary>
        /// Specifies the XZY order.
        /// </summary>
        Xzy = ((0) | (2 << 2) | (1 << 4)),

        /// <summary>
        /// Specifies the YXZ order.
        /// </summary>
        Yxz = ((1) | (0 << 2) | (2 << 4)),

        /// <summary>
        /// Specifies the YZX order.
        /// </summary>
        Yzx = ((1) | (2 << 2) | (0 << 4)),

        /// <summary>
        /// Specifies the ZXY order.
        /// </summary>
        Zxy = ((2) | (0 << 2) | (1 << 4)),

        /// <summary>
        /// Specifies the ZYX order.
        /// </summary>
        Zyx = ((2) | (1 << 2) | (0 << 4))
    }
}

namespace Ode.Net.Joints
{
    /// <summary>
    /// Specifies the relative orientation mode of an angular or linear motor joint axis.
    /// </summary>
    public enum RelativeOrientation
    {
        /// <summary>
        /// Specifies that the axis is anchored to the global frame.
        /// </summary>
        Global = 0,

        /// <summary>
        /// Specifies that the axis is anchored to the first body.
        /// </summary>
        FirstBody,

        /// <summary>
        /// Specifies that the axis is anchored to the second body.
        /// </summary>
        SecondBody
    }
}

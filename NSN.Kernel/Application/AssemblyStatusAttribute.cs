using System;

namespace NSN.Application
{
    public enum ReleaseMode
    {
        None,
        Alpha,
        Beta,
        RC,
        Stable
    }

    /// <summary>
    /// The status of current assembly.
    /// </summary>
    /// <example>
    /// [assembly: AssemblyStatus(ReleaseMode.Stable)]
    /// </example>
    [AttributeUsage(AttributeTargets.Assembly)]
    public class AssemblyStatusAttribute : Attribute
    {
        private readonly ReleaseMode _releaseMode;

        public AssemblyStatusAttribute(ReleaseMode releaseMode)
        {
            this._releaseMode = releaseMode;
        }

        public ReleaseMode Status
        {
            get { return _releaseMode; }
        }
    }
}

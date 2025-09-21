using System;
using UnityEngine;

namespace HUtil.Inspector {
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = false, Inherited = true)]
    public sealed class HHideLabelAttribute : PropertyAttribute { }
}

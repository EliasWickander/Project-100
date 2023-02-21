using System;
using System.Reflection;

namespace Util.UnityMVVM
{
    public sealed class BindableProperty
    {
        /// <summary>
        /// The bindable member info (usually a PropertyInfo or MethodInfo)
        /// </summary>
        public readonly PropertyInfo Member;

        /// <summary>
        /// View model that the property or method belongs to.
        /// </summary>
        public readonly Type ViewModelType;

        /// <summary>
        /// Name of the view model type.
        /// </summary>
        public string ViewModelTypeName
        {
            get
            {
                return ViewModelType.Name;
            }
        }

        /// <summary>
        /// Name of the member.
        /// </summary>
        public string MemberName
        {
            get
            {
                return Member.Name;
            }
        }

        public BindableProperty(PropertyInfo member, Type viewModelType)
        {
            Member = member;
            ViewModelType = viewModelType;
        }

        public override string ToString()
        {
            return string.Concat(ViewModelType.ToString(), ".", MemberName);
        }
    }
}
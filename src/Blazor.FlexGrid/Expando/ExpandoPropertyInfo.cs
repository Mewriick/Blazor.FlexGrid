using System;
using System.Reflection;

namespace Blazor.FlexGrid.Expando
{
    public class ExpandoPropertyInfo : PropertyInfo
    {
        private readonly string name;
        private readonly Type propertyType;

        public ExpandoPropertyInfo(string name, Type propertyType)
        {
            this.name = string.IsNullOrWhiteSpace(name)
                ? throw new ArgumentNullException(nameof(name))
                : name;

            this.propertyType = propertyType ?? throw new ArgumentNullException(nameof(propertyType));
        }

        public override string Name
        {
            get
            {
                return name;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return propertyType;
            }
        }

        public override PropertyAttributes Attributes
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanRead
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override bool CanWrite => false;

        public override Type DeclaringType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override Type ReflectedType
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public override MethodInfo[] GetAccessors(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetGetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override ParameterInfo[] GetIndexParameters()
        {
            throw new NotImplementedException();
        }

        public override MethodInfo GetSetMethod(bool nonPublic)
        {
            throw new NotImplementedException();
        }

        public override object GetValue(object obj, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override void SetValue(object obj, object value, BindingFlags invokeAttr, Binder binder, object[] index, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }

        public override object[] GetCustomAttributes(bool inherit)
        {
            throw new NotImplementedException();
        }

        public override bool IsDefined(Type attributeType, bool inherit)
        {
            throw new NotImplementedException();
        }
    }
}

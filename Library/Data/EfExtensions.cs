using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Reflection;

namespace Library.Data
{
    public static class PropertyCopy
    {
        private static object _propertyCopier;

        public static void CopyTo<TSource, TTarget>(this TSource source, TTarget target)
            where TSource : class
            where TTarget : class
        {
            if (_propertyCopier == null)
            {
                _propertyCopier = new PropertyCopier<TSource, TTarget>(source);
            }

            ((PropertyCopier<TSource, TTarget>)_propertyCopier).Copy(source, target);
        }
    }

    public static class PropertyCopy<TTarget> where TTarget : class, new()
    {
        private static object _propertyCopier;

        public static TTarget CopyFrom<TSource>(TSource source) where TSource : class
        {
            if (_propertyCopier == null)
            {
                _propertyCopier = new PropertyCopier<TSource, TTarget>(source);
            }

            return ((PropertyCopier<TSource, TTarget>)_propertyCopier).Copy(source);
        }
    }

    internal class PropertyCopier<TSource, TTarget>
    {
        private readonly Func<TSource, TTarget> _creator;

        private readonly List<PropertyInfo> _sourceProperties = new List<PropertyInfo>();
        private readonly List<PropertyInfo> _targetProperties = new List<PropertyInfo>();
        private readonly Exception _initializationException;

        internal TTarget Copy(TSource source)
        {
            if (_initializationException != null)
            {
                throw _initializationException;
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            return _creator(source);
        }

        internal void Copy(TSource source, TTarget target)
        {
            if (_initializationException != null)
            {
                throw _initializationException;
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            for (int i = 0; i < _sourceProperties.Count; i++)
            {
                _targetProperties[i].SetValue(target, _sourceProperties[i].GetValue(source, null), null);
            }

        }

        public PropertyCopier(TSource source)
        {
            try
            {
                _creator = BuildCreator(source);
                _initializationException = null;
            }
            catch (Exception e)
            {
                _creator = null;
                _initializationException = e;
            }
        }

        private Func<TSource, TTarget> BuildCreator(TSource source)
        {
            ParameterExpression sourceParameter = Expression.Parameter(typeof(TSource), "source");

            var bindings = new List<MemberBinding>();

            foreach (PropertyInfo sourceProperty in typeof(TSource).GetProperties(BindingFlags.Public | BindingFlags.Instance))
            {
                var value = sourceProperty.GetValue(source, null);

                var isSystemType = value != null && value.GetType().Module.ScopeName == "CommonLanguageRuntimeLibrary";

                if (!sourceProperty.CanRead || !isSystemType || value is DateTime && (DateTime)value == DateTime.MinValue || value is int && (int)value <= 0 || value is decimal && (decimal)value <= 0m || value is string && string.IsNullOrEmpty(value as string))
                {
                    continue;
                }

                PropertyInfo targetProperty = typeof(TTarget).GetProperty(sourceProperty.Name);

                if (targetProperty == null)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is not present and accessible in " + typeof(TTarget).FullName);
                }

                if (!targetProperty.CanWrite)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is not writable in " + typeof(TTarget).FullName);
                }

                if ((targetProperty.GetSetMethod().Attributes & MethodAttributes.Static) != 0)
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " is static in " + typeof(TTarget).FullName);
                }

                if (!targetProperty.PropertyType.IsAssignableFrom(sourceProperty.PropertyType))
                {
                    throw new ArgumentException("Property " + sourceProperty.Name + " has an incompatible type in " + typeof(TTarget).FullName);
                }

                bindings.Add(Expression.Bind(targetProperty, Expression.Property(sourceParameter, sourceProperty)));
                _sourceProperties.Add(sourceProperty);
                _targetProperties.Add(targetProperty);
            }

            Expression initializer = Expression.MemberInit(Expression.New(typeof(TTarget)), bindings);

            return Expression.Lambda<Func<TSource, TTarget>>(initializer, sourceParameter).Compile();
        }
    }
}

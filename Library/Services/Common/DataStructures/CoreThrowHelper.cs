using System;

namespace Library.Services.Common.DataStructures {
   public static class CoreThrowHelper {
      public static void CheckNullArg(object arg, string argName) {
         if (arg == null)
            throw new ArgumentNullException(argName, LogHelper.GetMessage("The parameter '{0}' must not be null", argName));
      }

      public static void CheckNullOrEmptyArg(string argument, string parameterName) {
         if (string.IsNullOrEmpty(argument))
            throw new ArgumentNullException(parameterName, LogHelper.GetMessage("String parameter {0} cannot be null or empty", parameterName));
      }

      public static void CheckNullProperty<TProp, TComp>(TProp property, TComp containingComponent, string propertyName)
            where TProp : class {
         CheckNullProperty(property, containingComponent, propertyName, string.Empty);
      }

      public static void CheckNullProperty<TProp, TComp>(TProp property, TComp containingComponent, string propertyName, string additionalInfo)
            where TProp : class {
         if (property == null)
            throw new InvalidOperationException(
                LogHelper.GetMessage("Property '{0}' of '{1}' was not initialized. {2}",
                                     propertyName,
                                     containingComponent.GetType(),
                                     additionalInfo ?? string.Empty));
      }

      public static void CheckType<TExpected>(object parameter, string parameterName) {
         CheckType<TExpected>(parameter, parameterName, false);
      }

      public static void CheckType<TExpected>(object parameter, string parameterName, bool skipNull) {
         if (skipNull && null == parameter)
            return;

         CheckNullArg(parameter, parameterName);

         if (parameter is TExpected)
            return;

         throw new ArgumentException(
             LogHelper.GetMessage("Parameter '{0}' has to be from type '{1}' but was '{2}'",
                                  parameterName,
                                  typeof(TExpected).FullName,
                                  parameter.GetType().FullName),
             parameterName);
      }
      
      public static void CheckIndexOutOfRange(int index, int boundary) {
         if (index >= boundary)
            throw new IndexOutOfRangeException();
      }
   }
}

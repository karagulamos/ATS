using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Library.Services.Common.DataStructures
{

    public static class LogHelper
    {

        public const TraceEventType DefaultSeverity = TraceEventType.Critical
                                                      | TraceEventType.Error
                                                      | TraceEventType.Warning;

        public static string GetMessage(string message, params object[] args)
        {
            if (args == null)
            {
                if (message.Contains('{'))
                    message = string.Format(CultureInfo.CurrentCulture, message, "<NULL>");
            }
            else if (args.Length != 0)
            {
                for (int i = 0; i < args.Length; i++)
                    if (args[i] == null)
                        args[i] = "<NULL>";
                message = string.Format(CultureInfo.CurrentCulture, message, args);
            }
            return message;
        }

        public static string GetCategoryForComponent(Type component)
        {
            CoreThrowHelper.CheckNullArg(component, "component");
            return component.Namespace;
        }

        public static string GetDetailedExceptionText(Exception ex)
        {
            List<string> details = GetDetailedExceptionTextLines(ex);
            return string.Join(Environment.NewLine, details);
        }

        public static List<string> GetDetailedExceptionTextLines(Exception ex)
        {
            List<string> details = new List<string>();
            GetDetailedExceptionTextInternal(ex, details, false);
            return details;
        }

        public static void WritePanicLog(string name, Exception ex)
        {
            try
            {
                if (string.IsNullOrEmpty(name))
                    name = "unknown";
                // write panic log and give up
                StringWriter panic = new StringWriter();
                panic.WriteLine("PANIC LOG: " + name);
                if (ex != null)
                    panic.WriteLine(GetDetailedExceptionText(ex));
                Assembly asm = Assembly.GetEntryAssembly() ?? typeof(LogHelper).Assembly;
                Uri uri = new Uri(asm.CodeBase);
                string dirName = Path.GetDirectoryName(uri.LocalPath);
                string fileName = Path.Combine(dirName, name + ".panic.log");
                File.WriteAllText(fileName, panic.GetStringBuilder().ToString());
            }
            catch
            {
                // ignored
            }
        }

        private static void GetDetailedExceptionTextInternal(Exception ex, List<string> details, bool isInner)
        {
            WriteHeader(ex, details, isInner);

            details.Add(string.Format("Message: {0}", ex.Message));

            WriteTargetSite(ex, details);
            WriteStackTrace(ex, details);

            if (ex.InnerException != null)
                GetDetailedExceptionTextInternal(ex.InnerException, details, true);

            WriteFooter(details, isInner);
        }

        private static void WriteFooter(List<string> details, bool isInner)
        {
            if (!isInner)
                details.Add("====================================================");
        }

        private static void WriteStackTrace(Exception ex, List<string> details)
        {
            details.Add("Stack Trace:");
            StackTrace stackTrace = new StackTrace(ex, true);
            for (int i = 0; i < stackTrace.FrameCount; i++)
            {
                StackFrame frame = stackTrace.GetFrame(i);
                MethodBase method = frame.GetMethod();
                Type type = method.DeclaringType;
                details.Add(string.Format("[{0}] {1}.{2}.{3} ({4} line: {5})",
                                          i,
                                          type.Namespace,
                                          type.Name,
                                          method.Name,
                                          frame.GetFileName() ?? "Unknown File",
                                          frame.GetFileLineNumber()));
            }
        }

        private static void WriteTargetSite(Exception ex, List<string> details)
        {
            if (null != ex.TargetSite)
            {
                MethodBase target = ex.TargetSite;
                Type t = target.DeclaringType;
                if (t != null)
                    details.Add(string.Format("Target: {0}.{1}.{2}", t.Namespace, t.Name, target.Name));
                else
                    details.Add(string.Format("Target: <UNKNOWN>.{0}", target.Name));
            }
        }

        private static void WriteHeader(Exception ex, List<string> details, bool isInner)
        {
            if (!isInner)
            {
                details.Add("====================================================");
                details.Add(string.Format("EXCEPTION ({0}.{1})", ex.GetType().Namespace, ex.GetType().Name));
            }
            else
            {
                details.Add("----------------------------------------------------");
                details.Add(string.Format("INNER EXCEPTION ({0}.{1}", ex.GetType().Namespace, ex.GetType().Name));
            }
        }
    }
}

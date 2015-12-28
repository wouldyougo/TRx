using Microsoft.CSharp.RuntimeBinder;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;

namespace Ecng.Common
{
    public static class DelegateHelper
    {
        public static CallSite<Action<CallSite, object, object, EventArgs>> pSite6;

        public static void Do(this Action action, Action<Exception> error)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (error == null)
                throw new ArgumentNullException("error");
            try
            {
                action();
            }
            catch (Exception ex)
            {
                error(ex);
            }
        }

        public static void DoAsync(this Action action, Action<Exception> error)
        {
            if (action == null)
                throw new ArgumentNullException("action");
            if (error == null)
                throw new ArgumentNullException("error");
            DelegateHelper.Do((Action)(() => action.BeginInvoke((AsyncCallback)(result =>
            {
                try
                {
                    action.EndInvoke(result);
                }
                catch (Exception ex)
                {
                    error(ex);
                }
            }), (object)null)), error);
        }

        public static void SafeInvoke(this Action handler)
        {
            Action action = handler;
            if (action == null)
                return;
            action();
        }

        public static void SafeInvoke<T>(this Action<T> handler, T arg)
        {
            Action<T> action = handler;
            if (action == null)
                return;
            action(arg);
        }

        public static void SafeInvoke<T1, T2>(this Action<T1, T2> handler, T1 arg1, T2 arg2)
        {
            Action<T1, T2> action = handler;
            if (action == null)
                return;
            action(arg1, arg2);
        }

        public static void SafeInvoke<T1, T2, T3>(this Action<T1, T2, T3> handler, T1 arg1, T2 arg2, T3 arg3)
        {
            Action<T1, T2, T3> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3);
        }

        public static void SafeInvoke<T1, T2, T3, T4>(this Action<T1, T2, T3, T4> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4)
        {
            Action<T1, T2, T3, T4> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5>(this Action<T1, T2, T3, T4, T5> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5)
        {
            Action<T1, T2, T3, T4, T5> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6>(this Action<T1, T2, T3, T4, T5, T6> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6)
        {
            Action<T1, T2, T3, T4, T5, T6> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7>(this Action<T1, T2, T3, T4, T5, T6, T7> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7)
        {
            Action<T1, T2, T3, T4, T5, T6, T7> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8>(this Action<T1, T2, T3, T4, T5, T6, T7, T8> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14);
        }

        public static void SafeInvoke<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15>(this Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> handler, T1 arg1, T2 arg2, T3 arg3, T4 arg4, T5 arg5, T6 arg6, T7 arg7, T8 arg8, T9 arg9, T10 arg10, T11 arg11, T12 arg12, T13 arg13, T14 arg14, T15 arg15)
        {
            Action<T1, T2, T3, T4, T5, T6, T7, T8, T9, T10, T11, T12, T13, T14, T15> action = handler;
            if (action == null)
                return;
            action(arg1, arg2, arg3, arg4, arg5, arg6, arg7, arg8, arg9, arg10, arg11, arg12, arg13, arg14, arg15);
        }

        public static void SafeInvoke(this EventHandler<EventArgs> handler, object sender)
        {
            DelegateHelper.SafeInvoke<EventArgs>(handler, sender, EventArgs.Empty);
        }

        public static void SafeInvoke<T>(this EventHandler<T> handler, object sender, T args) where T : EventArgs
        {
            DelegateHelper.SafeInvoke<T>(handler, sender, args, (Action<T>)(args2 => { }));
        }

        public static void SafeInvoke<T>(this EventHandler<T> handler, object sender, T args, Action<T> action) where T : EventArgs
        {
            if (sender == null)
                throw new ArgumentNullException("sender");
            if ((object)args == null)
                throw new ArgumentNullException("args");
            if (action == null)
                throw new ArgumentNullException("action");
            EventHandler<T> eventHandler = handler;
            if (eventHandler == null)
                return;
            eventHandler(sender, args);
            action(args);
        }

        public static EventHandler<TArgs> Cast<TArgs>(this Delegate handler) where TArgs : EventArgs
        {
            if (handler == null)
                return (EventHandler<TArgs>)null;
            object h = (object)handler;
            return (EventHandler<TArgs>)((sender, e) =>
            {
                // ISSUE: reference to a compiler-generated field
                if (DelegateHelper.pSite6 == null)
                {
                    // ISSUE: reference to a compiler-generated field
                    DelegateHelper.pSite6 = CallSite<Action<CallSite, object, object, EventArgs>>.Create(Microsoft.CSharp.RuntimeBinder.Binder.Invoke(CSharpBinderFlags.ResultDiscarded, typeof(DelegateHelper), (IEnumerable<CSharpArgumentInfo>)new CSharpArgumentInfo[3]
                    {
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.None, (string) null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null),
                        CSharpArgumentInfo.Create(CSharpArgumentInfoFlags.UseCompileTimeType, (string) null)
                    }));
                }
                // ISSUE: reference to a compiler-generated field
                // ISSUE: reference to a compiler-generated field
                DelegateHelper.pSite6.Target((CallSite)DelegateHelper.pSite6, h, sender, e);
            });
        }

        public static EventHandler<EventArgs> Cast(this EventHandler handler)
        {
            return DelegateHelper.Cast<EventArgs>((Delegate)handler);
        }

        public static void SafeInvoke(this PropertyChangedEventHandler handler, object sender, string name)
        {
            if (handler == null)
                return;
            handler(sender, new PropertyChangedEventArgs(name));
        }

        public static void SafeInvoke(this PropertyChangingEventHandler handler, object sender, string name)
        {
            if (handler == null)
                return;
            handler(sender, new PropertyChangingEventArgs(name));
        }

        public static T CreateDelegate<T>(this MethodInfo method)
        {
            return Converter.To<T>((object)Delegate.CreateDelegate(typeof(T), method, true));
        }

        public static T CreateDelegate<I, T>(this MethodInfo method, I instance)
        {
            return Converter.To<T>((object)Delegate.CreateDelegate(typeof(T), (object)instance, method, true));
        }

        public static T AddDelegate<T>(this T source, T value)
        {
            return Converter.To<T>((object)Delegate.Combine(Converter.To<Delegate>((object)source), Converter.To<Delegate>((object)value)));
        }

        public static T RemoveDelegate<T>(this T source, T value)
        {
            return Converter.To<T>((object)Delegate.Remove(Converter.To<Delegate>((object)source), Converter.To<Delegate>((object)value)));
        }

        public static void RemoveAllDelegates<T>(this T source)
        {
            foreach (T obj in DelegateHelper.GetInvocationList<T>(source))
                DelegateHelper.RemoveDelegate<T>(source, obj);
        }

        public static IEnumerable<T> GetInvocationList<T>(this T @delegate)
        {
            Delegate delegate1 = Converter.To<Delegate>((object)@delegate);
            return Enumerable.Cast<T>(delegate1 != null ? (IEnumerable)delegate1.GetInvocationList() : (IEnumerable)ArrayHelper.Empty<Delegate>());
        }
    }
}
using System;
using System.Threading.Tasks;

namespace DatahawkPoc
{
    public static class Functional
    {
        public static TResult Map<TSource, TResult>(
            this TSource @this,
            Func<TSource, TResult> fn) =>
            fn(@this);

        public static async Task<TResult> Map<TSource, TResult>(
            this TSource @this,
            Func<TSource, Task<TResult>> fn) =>
            await fn(@this);

        public static async Task<TResult> Map<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, TResult> fn) =>
            fn(await @this);

        public static async Task<TResult> Map<TSource, TResult>(
            this Task<TSource> @this,
            Func<TSource, Task<TResult>> fn) =>
            await fn(await @this);

        public static T Tee<T>(
            this T @this,
            Action<T> action)
        {
            action(@this);
            return @this;
        }

        public static async Task<T> Tee<T>(
            this T @this,
            Func<T, Task> fn)
        {
            await fn(@this);
            return @this;
        }

        public static async Task<T> Tee<T>(
            this Task<T> @this,
            Action<T> action)
        {
            var t = await @this;
            action(t);
            return t;
        }

        public static async Task<T> Tee<T>(
            this Task<T> @this,
            Func<T, Task> fn)
        {
            var t = await @this;
            await fn(t);
            return t;
        }
    }
}

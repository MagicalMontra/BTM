using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace HotPlay.Utilities
{
    public static class TaskHelper
    {
        public delegate bool TryGetValueHandler<TKey, TOutput>(TKey key, out TOutput output);

        public static Task<TOutput> CallAsTask<TKey, TOutput>(this TryGetValueHandler<TKey, TOutput> tryGetValueHandler, TKey key)
        {
            if (tryGetValueHandler is null)
            {
                throw new ArgumentNullException(nameof(tryGetValueHandler));
            }

            return Task.Run(() =>
            {
                bool result = tryGetValueHandler.Invoke(key, out var output);
                return result ? output : throw new KeyNotFoundException(key.ToString());
            });
        }

        public static async Task CreateTask(this Action<Action> work)
        {
            bool isFinished = false;

            work?.Invoke(() =>
            {
                isFinished = true;
            });

            await When(() => isFinished);
        }

        public static Task When(Func<bool> predicate)
        {
            return When(
                predicate: predicate,
                cancellationToken: CancellationToken.None
            );
        }

        public static async Task When(Func<bool> predicate, CancellationToken cancellationToken)
        {
            while (!predicate.Invoke())
            {
                cancellationToken.ThrowIfCancellationRequested();
                await Task.Yield();
            }
        }

        public static Task WhenCompleted(this Task task, CancellationToken cancellationToken)
        {
            return When(
                predicate: () => task.IsCompleted,
                cancellationToken
            );
        }

        public static async Task<T> WhenCompleted<T>(this Task<T> task, CancellationToken cancellationToken)
        {
            await When(
                predicate: () => task.IsCompleted,
                cancellationToken: cancellationToken
            );
            return await task;
        }

        public static async Task WhenOrTimeoutAsync(Func<bool> predicate, int millisecondsTimeout)
        {
            CancellationTokenSource cancellationTokenSource = new CancellationTokenSource();
            CancellationToken cancellationToken = cancellationTokenSource.Token;
            cancellationTokenSource.CancelAfter(millisecondsTimeout);
            try
            {
                await When(
                    predicate: predicate,
                    cancellationToken: cancellationToken
                );
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                if (cancellationTokenSource != null)
                {
                    cancellationTokenSource.Cancel();
                    cancellationTokenSource.Dispose();
                }
            }
        }

        public static async Task RunWithRetryOnExceptionAsync(
            Func<int, CancellationToken, Task> taskFunction,
            int retryCount,
            int retryDelayMilliseconds,
            Predicate<Exception> acceptableExceptionPredicate,
            CancellationToken cancellationToken
        )
        {
            Queue<Exception> exceptions = new Queue<Exception>();

            for (int i = 0; i < retryCount + 1; ++i)
            {
                try
                {
                    cancellationToken.ThrowIfCancellationRequested();
                    await taskFunction?.Invoke(i, cancellationToken);
                    return;
                }
                catch (Exception ex)
                {
                    exceptions.Enqueue(ex);
                    if (acceptableExceptionPredicate.Invoke(ex))
                    {
                        if (i < retryCount)
                        {
                            await Task.Delay(retryDelayMilliseconds);
                        }
                    }
                    else
                    {
                        throw new RetryExceededException(i, exceptions);
                    }
                }
            }

            throw new RetryExceededException(retryCount, exceptions);
        }
    }
}

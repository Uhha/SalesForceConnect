using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationModel
{
    public static class ConnectionHelper
    {
        public static Task<T> FormTask<T>(T obj)
        {
            var completionResult = new TaskCompletionSource<T>();
            completionResult.SetResult(obj);
            return completionResult.Task;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    internal static class ConnectionChecker
    {
        internal async static Task<T> TryNTimesAsync<T>(Func<Task<T>> func, int times, string initialMessage = "")
        {
            while (times > 0)
            {
                LoggerController.Log(initialMessage);
                try
                {
                    return await func();
                }
                catch (Exception e)
                {
                    if (--times <= 0)
                    {
                        LoggerController.Log(e);
                        throw;
                        //return default(T);
                    }
                    LoggerController.Log(e);
                    LoggerController.Log("Repeating in 5 seconds");
                    Thread.Sleep(5000);

                }

            }
            //No check connect with 0 times.
            return await func();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SFConnectMobile.Base
{
    public interface IErrorHandler
    {
        void HandleError(Exception ex);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace SFConnectMobile.Base
{
    public class GeneralErrorHandler : IErrorHandler
    {
        public void HandleError(Exception ex)
        {
            throw ex;
        }
    }
}

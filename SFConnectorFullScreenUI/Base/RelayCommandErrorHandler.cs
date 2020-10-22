using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public class RelayCommandErrorHandler : IErrorHandler
    {
        public void HandleError(Exception e)
        {
            LoggerController.Log(e);
            //throw new Exception();
        }
    }
}

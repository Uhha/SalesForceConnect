using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SFConnectorFullScreenUI
{
    public interface IErrorHandler
    {
        void HandleError(Exception e);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationModel
{
    public interface IPerson
    {
        string Id { get; set; }
        string Name { get; set; }

        string GetFistName();
        string GetMiddleName();
        string GetLastName();

        string GetFormattedName();
    }
}

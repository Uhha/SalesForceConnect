using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationModel.SalesForce
{
    public class SFComment
    {
        public string ID { get; set; }
        public string CommentBody { get; set; }
        public SFPerson Creator { get; set; }
        public string DtCreated { get; set; }
    }
}

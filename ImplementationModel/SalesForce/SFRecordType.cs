using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace ImplementationModel.SalesForce
{
    public class SFRecordType
    {
        public string Id { get; set; }
        public string Name { get; set; }

        //public static List<RecordType> RecordTypes;

        //public static async Task<List<RecordType>> InitRecordTypes()
        //{
        //    string qry = "SELECT Id, Name  FROM RecordType";
        //    var client = await Connector.GetClientAsync();
        //    //012500000001s4FAAQ
        //    return await client.Query<RecordType>(qry);
        //}

    }
}

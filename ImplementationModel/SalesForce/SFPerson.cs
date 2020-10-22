using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;

namespace ImplementationModel.SalesForce
{
    public class SFPerson : IPerson
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public string GetFistName()
        {
            if (string.IsNullOrEmpty(Name)) return "";
            string[] split = Name.Split(' ');
            if (split.Length > 0)
            {
                return split[0];
            }
            return "";
        }

        public string GetLastName()
        {
            if (string.IsNullOrEmpty(Name)) return "";
            string[] split = Name.Split(' ');
            if (split.Length == 2)
            {
                return split[1];
            }
            else if (split.Length > 2)
            {
                return split[2];
            }
            return Name;
        }

        public string GetMiddleName()
        {
            if (string.IsNullOrEmpty(Name)) return "";
            string[] split = Name.Split(' ');
            if (split.Length > 2)
            {
                return split[1];
            }
            return "";
        }

        public string GetFormattedName()
        {
            if (string.IsNullOrEmpty(Name)) return "";
            return GetFistName() + ' ' + GetLastName();
        }

        [JsonConstructor]
        public SFPerson()
        {

        }
    }
}

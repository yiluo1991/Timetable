using System;
using System.Collections.Generic;
using System.Text;

namespace Timetable.Security
{
    public class ConfigDES : DES
    {
        public override string Decrypt(string val)
        {
            return DES.Decrypt(val, "JA(id8!}", "/i_s8#iS");
        }

        public override string Encrypt(string val)
        {
            return DES.Encrypt(val, "JA(id8!}", "/i_s8#iS");
        }
    }
}

using LC.SOA.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC.SOA.Service
{
    public class TbLogService : ITbLogService
    {
        public object Query(int id)
        {
            return new
            {
                Id = 123,
                Namew = "Jack"
            };
        }
    }
}

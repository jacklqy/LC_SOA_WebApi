using LC.SOA.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC.SOA.Service
{
    public class Tb_LogService : BaseService, ITb_LogService
    {
        public Tb_LogService(DbContext context) : base(context)
        {

        }
    }
}

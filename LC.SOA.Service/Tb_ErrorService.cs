using LC.SOA.Interface;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC.SOA.Service
{
    public class Tb_ErrorService : BaseService,ITb_ErrorService
    {
        public Tb_ErrorService(DbContext context) : base(context)
        {

        }
    }
}

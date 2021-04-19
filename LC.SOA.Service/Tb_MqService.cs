using LC.SOA.Interface;
using LC.SOA.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LC.SOA.Service
{
    public class Tb_MqService : BaseService, ITb_MqService
    {
        public Tb_MqService(DbContext context) : base(context)
        {

        }

        public void UpdateLastData(tb_mq mq)
        {
            tb_mq mqDB = base.Find<tb_mq>(mq.id);
            mqDB.mqname = "test";
            this.Commit();
        }
    }
}

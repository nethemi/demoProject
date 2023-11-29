using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ОТК_ЗАО__Русская_косметика_.Classes
{
    public class CheckingWorker
    {
        public bool CheckWorker(int workerPost)
        {
            if (workerPost == 3 || workerPost == 5) return true;
            return false;
        }
    }
}

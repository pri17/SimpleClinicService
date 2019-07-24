using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core
{
    public class DatabaseExcception:Exception
    {

        public DatabaseExcception(string mesg)
            : base(mesg)
        {
            
        }
    }
}

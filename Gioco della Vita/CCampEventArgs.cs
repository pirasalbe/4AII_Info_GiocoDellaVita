using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gioco_della_Vita
{
    class CCampEventArgs : EventArgs
    {
        public CCamp Table;

        public CCampEventArgs(CCamp Table)
        {
            this.Table = Table;
        }
    }
}

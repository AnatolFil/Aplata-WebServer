using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebServer.Models
{
    public class SortParam
    {
        public int Order { get; set; } = 1;
        public int Num { get; set; } = 1;
        public int IDT { get; set; } = 129;

        public void Sort(ref CommandsViewModel Cmd)
        {
            if(Order == 1)
            {
                if(Num == 1)
                {
                    Cmd.LCmdHistory.Sort(delegate (CommandHistory x, CommandHistory y)
                        {
                            if (x.time_created == null && y.time_created == null) return 0;
                            else if (x.time_created == null) return -1;
                            else if (y.time_created == null) return 1;
                            else
                            {
                                DateTime xDate = DateTime.ParseExact(x.time_created, "yyyy-MM-dd HH:mm:ss.fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                DateTime yDate = DateTime.ParseExact(y.time_created, "yyyy-MM-dd HH:mm:ss.fff",
                                       System.Globalization.CultureInfo.InvariantCulture);
                                if (xDate > yDate)
                                    return 1;
                                else
                                    return -1;//return y.time_created.CompareTo(x.time_created);
                            }
                        }
                    );
                }
            }
            else
            {
                if(Num == 1)
                {
                    Cmd.LCmdHistory.Sort(delegate (CommandHistory x, CommandHistory y)
                    {
                        if (x.time_created == null && y.time_created == null) return 0;
                        else if (x.time_created == null) return -1;
                        else if (y.time_created == null) return 1;
                        else
                        {
                            DateTime xDate = DateTime.ParseExact(x.time_created, "yyyy-MM-dd HH:mm:ss.fff",
                                   System.Globalization.CultureInfo.InvariantCulture);
                            DateTime yDate = DateTime.ParseExact(y.time_created, "yyyy-MM-dd HH:mm:ss.fff",
                                   System.Globalization.CultureInfo.InvariantCulture);
                            if (xDate > yDate)
                                return -1;
                            else
                                return 1;//return y.time_created.CompareTo(x.time_created);
                        }
                    }
                    );
                }
            }
        }
    }
}

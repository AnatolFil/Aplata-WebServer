using System;
using System.Collections.Generic;
using System.Text;

namespace DataApiService.Models
{
    public class CommandHistory
    {
        public int id { get; set; } = 0;
        public int terminal_id { get; set; } = 0;
        public int command_id { get; set; } = 0;
        public Command Cmd { get; set; }
        public int parameter1 { get; set; } = 0;
        public int parameter2 { get; set; } = 0;
        public int parameter3 { get; set; } = 0;
        public int parameter4 { get; set; } = 0;
        public string str_parameter1 { get; set; }
        public string str_parameter2 { get; set; }
        public int state { get; set; } = 0;
        public string state_name { get; set; }
        public string time_created { get; set; }
        public string time_delivered { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WebServer.Models
{
    public class Command
    {
        public int id { get; set; } = 0;
        public string name { get; set; }
        public string parameter_name1 { get; set; }
        public string parameter_name2 { get; set; }
        public string parameter_name3 { get; set; }
        public int parameter_default_value1 { get; set; } = 0;
        public int parameter_default_value2 { get; set; } = 0;
        public int parameter_default_value3 { get; set; } = 0;
        public int parameter1 { get; set; } = 0;
        public int parameter2 { get; set; } = 0;
        public int parameter3 { get; set; } = 0;
    }
}

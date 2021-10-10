using System;
using System.Collections.Generic;
using System.Text;

namespace DataApiService.Models
{
    public class CommandsViewModel
    {
        public CommandsViewModel()
        {
            Commands = new List<Command>();
            LCmdHistory = new List<CommandHistory>();
            CmdParams = new Command();
        }
        public List<Command> Commands { get; set; }
        public int IDTerminal { get; set; } = 129;
        public List<CommandHistory> LCmdHistory { get; set; }
        public int CmdId { get; set; }
        public Command CmdParams { get; set; }
    }
}

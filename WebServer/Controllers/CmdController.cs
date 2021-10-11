using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DataApiService;
using DataApiService.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Logging;
using WebServer.Models;

namespace WebServer.Controllers
{
    public class CmdController:BaseController
    {
        private readonly ILogger<CmdController> _logger;
        private IDataManager _dataManager;

        public CmdController(ILogger<CmdController> logger, IDataManager dataManager)
        {
            _logger = logger;
            _dataManager = dataManager;
            _dataManager.Auth("demo", "demo15");
        }

        /// <summary>
        /// Список всех команд
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //Получаем все типы команд
            var CmdTypes = await _dataManager.GetItems<Command>("commands/types", new Dictionary<string, string>());
            CommandsViewModel model = new CommandsViewModel();
            model.Commands = CmdTypes.ToList();
            //Запрашиваем историю все команда для заданного терминала
            var resultHist = await _dataManager.GetItems<CommandHistory>("terminals/" + model.IDTerminal.ToString() + "/commands", new Dictionary<string, string>());
            model.LCmdHistory = resultHist.ToList();

            //Сбрасываем текущий выбор команды
            model.CmdParams = null;
            //Инициализируем лист с историей команд для возможности отображения названия команд
            foreach (CommandHistory item in model.LCmdHistory)
            {
                item.Cmd = model.Commands.FirstOrDefault<Command>(x => x.id == item.command_id);
            }
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CommandsViewModel CmdView)
        {
            if (!ModelState.IsValid)
            {
                //Можно сделать валидацию
                return View("Index", CmdView);
            }

            //Формируем параметры Get запроса
            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("command_id", CmdView.CmdId.ToString());
            if(CmdView.CmdParams != null)
            {
                param.Add("parameter1", CmdView.CmdParams.parameter1.ToString());
                param.Add("parameter2", CmdView.CmdParams.parameter2.ToString());
                param.Add("parameter3", CmdView.CmdParams.parameter3.ToString());
            }

            //Отправляем команду
            var CmdSentResult = await _dataManager.SendItems<CommandResult>("terminals/" + CmdView.IDTerminal.ToString() + "/command", param);

            //Запрашиваем историю всех команд для заданного терминала
            var resultHist = await _dataManager.GetItems<CommandHistory>("terminals/" + CmdView.IDTerminal.ToString() + "/commands", new Dictionary<string, string>());

            CmdView.LCmdHistory = resultHist.ToList();

            //Сбрасываем текущий выбор команды
            CmdView.CmdParams = null;
            //Инициализируем лист с историей команд для возможности отображения названия команд
            foreach(CommandHistory item in CmdView.LCmdHistory)
            {
                item.Cmd = CmdView.Commands.FirstOrDefault<Command>(x => x.id == item.command_id);
            }

            return View("Index", CmdView);
        }

        /// <summary>
        /// Возвращает во View по Ajax подробности выбранной команды
        /// </summary>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> NewCmd(int idCmd)
        {
            Command cmd = new Command();
            //Получаем все типы команд
            var CmdTypes = await _dataManager.GetItems<Command>("commands/types", new Dictionary<string, string>());
            cmd = CmdTypes.FirstOrDefault<Command>(x => x.id == idCmd);
            //Отправляем команду с выбранным id
            return PartialView("_CmdPartial", cmd);
        }

        /// <summary>
        /// Сортировка таблицы
        /// </summary>
        /// <returns></returns>
        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> Sort(SortParam sortParam)
        {
            CommandsViewModel History = new CommandsViewModel();
            //Запрашиваем историю всех команд для заданного терминала
            var resultHist = await _dataManager.GetItems<CommandHistory>("terminals/" + sortParam.IDT.ToString() + "/commands", new Dictionary<string, string>());

            History.LCmdHistory = resultHist.ToList();

            //Получаем все типы команд
            var CmdTypes = await _dataManager.GetItems<Command>("commands/types", new Dictionary<string, string>());
            //Инициализируем лист с историей команд для возможности отображения названия команд
            foreach (CommandHistory item in History.LCmdHistory)
            {
                item.Cmd = CmdTypes.ToList().FirstOrDefault<Command>(x => x.id == item.command_id);
            }

            //History.LCmdHistory.Sort(sortParam.dek());
            sortParam.Sort(ref History);
            return PartialView("_SortPartial", History);
        }
    }
}

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
    public class EventsController:BaseController
    {
        private readonly ILogger<EventsController> _logger;
        private IDataManager _dataManager;
        private List<Command> Commands;

        public EventsController(ILogger<EventsController> logger, IDataManager dataManager)
        {
            _logger = logger;
            _dataManager = dataManager;
            _dataManager.Auth("demo", "demo15");
            Commands = new List<Command>();
        }

        /// <summary>
        /// Список всех событий по умолчанию
        /// </summary>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            //Заполняем дроп лист выбора автоматов
            await SetMachinesDropList();
            
            //Получаем все типы команд
            var CmdTypes = await _dataManager.GetItems<Command>("commands/types", new Dictionary<string, string>());
            Commands = CmdTypes.ToList();
            CommandsViewModel model = new CommandsViewModel();
            model.Commands = CmdTypes.ToList();
            //var model1 = await _dataManager.GetItems<CommandHistory>("terminals/129/commands", new Dictionary<string, string>());

            return View(model);
        }

        /// <summary>
        /// Список событий по фильтру
        /// </summary>
        /// <param name="pars">Набор параметров</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> Index(EventsActionParameters pars)
        {
            //Проверка параметров запроса
            if (!ModelState.IsValid)
            {
                //Можно формировать сообщение или отправить на страницу ошибок, пока так
                //TODO Не в продакшен
                return BadRequest();
            }

            //Заполняем дроп лист выбора автоматов
            await SetMachinesDropList();

            //Формируем запрос событий
            //Конвертация даты из формата ДД.ММ.ГГГГ в ГГГГ-ММ-ДД происходит при маппинге параметров в классе EventsActionParameters
            var model =await _dataManager.GetItems<EventResults>("events",pars.ToDictionary());

            //Сохраняем выбранный в фильтре аппарат
            ViewData["Select_Machine_Id"] = pars.Machine_id ?? "";
            

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Submit(CommandsViewModel CmdView)
        {
            if (!ModelState.IsValid)
            {
                return View("Index", CmdView);
            }

            Dictionary<string, string> param = new Dictionary<string, string>();
            param.Add("command_id", CmdView.CmdId.ToString());
            if(CmdView.CmdParams != null)
            {
                param.Add("parameter1", CmdView.CmdParams.parameter1.ToString());
                param.Add("parameter2", CmdView.CmdParams.parameter2.ToString());
                param.Add("parameter3", CmdView.CmdParams.parameter3.ToString());
            }

            var result = await _dataManager.SendItems<CommandResult>("terminals/" + CmdView.IDTerminal.ToString() + "/command", param);
            var resultHist = await _dataManager.GetItems<CommandHistory>("terminals/" + CmdView.IDTerminal.ToString() + "/commands", new Dictionary<string, string>());

            CmdView.LCmdHistory = resultHist.ToList();
            CmdView.CmdParams = null;

            return View("Index", CmdView);
        }

        [ResponseCache(NoStore = true, Duration = 0)]
        public async Task<IActionResult> NewCmd(int idCmd)
        {
            Command cmd = new Command();
            var CmdTypes = await _dataManager.GetItems<Command>("commands/types", new Dictionary<string, string>());
            Commands = CmdTypes.ToList();
            cmd = Commands.FirstOrDefault<Command>(x => x.id == idCmd);
            return PartialView("_CmdPartial", cmd);
        }

        /// <summary>
        /// Заполнение списка доступных автоматов (машин)
        /// </summary>
        /// <remarks>
        /// Заполняет список в ViewData["Machines"]
        /// </remarks>
        /// <returns></returns>
        private async Task SetMachinesDropList()
        {
            //Запрос машин
            var machines =await _dataManager.GetItems<Machine>("machines");

            //Мапинг в дроп лист
            var resultList = machines.Select(x => new SelectListItem( $"{x.Machine_name} | {x.Machine_address} | {x.Machine_model}", x.Machine_id.ToString())).ToList();
            
            //Значение для "Все"
            resultList.Insert(0,new SelectListItem(" Все ",""));
            
            ViewData["Machines"] = resultList;
        } 
    }
}

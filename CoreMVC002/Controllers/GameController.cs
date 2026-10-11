using Microsoft.AspNetCore.Mvc;
using CoreMVC002.Models;
using System.Text.Json;

namespace CoreMVC002.Controllers
{
    public class GameController : Controller
    {
        private const string SessionKey = "XAXB_Game";

        // 取得目前的遊戲引擎物件，若無則建立新的
        private XAXBEngine GetEngine()
        {
            var json = HttpContext.Session.GetString(SessionKey);
            if (string.IsNullOrEmpty(json))
            {
                var engine = new XAXBEngine();
                engine.RestartGame();
                SaveEngine(engine);
                return engine;
            }
            return JsonSerializer.Deserialize<XAXBEngine>(json) ?? new XAXBEngine();
        }

        private void SaveEngine(XAXBEngine engine)
        {
            var json = JsonSerializer.Serialize(engine);
            HttpContext.Session.SetString(SessionKey, json);
        }

        // GET: /GuessNumber 或 /Game/Index
        public IActionResult Index()
        {
            var engine = GetEngine();
            return View(engine);
        }

        // POST: 送出猜測數字
        [HttpPost]
        public IActionResult Guess(string guess)
        {
            var engine = GetEngine();
            if (!engine.IsGameOver)
            {
                engine.CheckGuess(guess);
                SaveEngine(engine);
            }
            return View("Index", engine);
        }

        // POST/GET: 重新開始遊戲
        public IActionResult Restart()
        {
            var engine = GetEngine();
            engine.RestartGame();
            SaveEngine(engine);
            return RedirectToAction("Index");
        }
    }
}
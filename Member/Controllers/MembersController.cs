using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Member.Data;
using Member.Models;
using Newtonsoft.Json.Linq;

namespace Member.Controllers
{
	public class MembersController : Controller
	{
		private static Dictionary<string, string> ErrorCode = new Dictionary<string, string>()
		{
			{ "003", "帳號長度錯誤" },
			{ "004", "密碼長度錯誤" },
			{ "005", "信箱格式錯誤" }
		};
		private readonly MemberContext _context;

		public MembersController(MemberContext context)
		{
			_context = context;
		}

		// GET: Members
		public async Task<IActionResult> Index()
		{


			if (HttpContext.Session.GetString("Account") != null)
			{
				ViewData["Account"] = HttpContext.Session.GetString("Account");
				ViewData["Password"] = HttpContext.Session.GetString("Password");
				ViewData["Phone"] = HttpContext.Session.GetString("Phone");
				ViewData["Email"] = HttpContext.Session.GetString("Email");
				return View(await _context.MembersModel.ToListAsync());
			}
			else
			{
				return RedirectToAction("Login", "Members");
			}

		}




		// GET: Member/Signup
		public IActionResult Signup()
		{

			return View();
		}

		// POST: Mmember/Signup
		[HttpPost, ActionName("Signup")]
		public async Task<IActionResult> Signup([Bind("Account,Password,Email,Phone")]  MembersModel membersModel)
		{

			if (ModelState.IsValid)
			{
				//search
				var CurrentUser = await _context.MembersModel
				.Where(member => member.Account == membersModel.Account)
				.ToListAsync();

				//代表此帳號已有資料
				if (CurrentUser.Count > 0)
				{
					ViewData["Message"] = "此帳號已存在";
					return View();
				}


				HttpContext.Session.SetString("Account", membersModel.Account);
				HttpContext.Session.SetString("Password", membersModel.Password);
				HttpContext.Session.SetString("Email", membersModel.Email);
				HttpContext.Session.SetString("Phone", membersModel.Phone);

				_context.Add(membersModel);
				await _context.SaveChangesAsync();
				return RedirectToAction(nameof(Index));
			}
			return View(membersModel);
		}

		//登入
		//GET
		public IActionResult Login()
		{
			return View();
		}


		//登出
		//POST
		[HttpPost]
		public async Task<IActionResult> Login([Bind("Account,Password")]  MembersModel membersModel)
		{
			if (membersModel.Account == null || membersModel.Password == null)
			{
				return NotFound();
			}


			//query
			var result = await _context.MembersModel
			.Where(member => member.Account == membersModel.Account && member.Password == membersModel.Password)
			.ToListAsync();


			if (result.Count > 0)
			{
				HttpContext.Session.SetString("Account", result[0].Account);
				HttpContext.Session.SetString("Password", result[0].Password);
				HttpContext.Session.SetString("Phone", result[0].Phone);
				HttpContext.Session.SetString("Email", result[0].Email);
				return RedirectToAction(nameof(Index));
			}
			else
			{
				ViewData["Message"] = "未找到帳號";
				return View();
			}

		}

		//GET 登出
		public IActionResult Logout()
		{
			HttpContext.Session.Clear();

			return View(nameof(Login));
		}


		private bool MembersModelExists(string Account)
		{
			return _context.MembersModel.Any(e => e.Account == Account);
		}

		private string IsAvailable(MembersModel memberModels)
		{
			if (memberModels.Account.Length < 6 || memberModels.Account.Length > 12)
			{
				return "003";
			}
			return "000";
		}
	}
}

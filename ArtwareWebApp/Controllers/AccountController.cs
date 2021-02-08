using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ArtwareWebApp.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
        {
            this._userManager = userManager;
            this._signInManager = signInManager;
        }
        /// <summary>
        /// 注册
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(Models.Identity.RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                //将数据从register 复制到IdentityUser 
                var user = new IdentityUser { UserName = model.Email, Email = model.Email };
                //将用户数据存入AspnetUsers表中
                var result = await _userManager.CreateAsync(user, model.Password);
                //如果成功创建用户则使用登录服务登录用户信息 
                //并重定向Homecontroller
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("index", "home");
                }
                //如果有任何错误，则将他们添加到ModelState对象中
                //将由验证摘要标记助手显示到视图中
                foreach (var item in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                }
            }
            return View(model);
        }

    }
}

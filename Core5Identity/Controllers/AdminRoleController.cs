using Core5Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core5Identity.Controllers
{
    public class AdminRoleController : Controller
    {

        private RoleManager<IdentityRole> roleManager;
        private UserManager<ApplicationUser> userManager;

        public AdminRoleController(RoleManager<IdentityRole> _roleManager, UserManager<ApplicationUser> _userManager)
        {
            roleManager = _roleManager;
            userManager= _userManager; 
        }
        public IActionResult Index()
        {
            return View(roleManager.Roles);
        }

        public IActionResult Create()
        {
            return View();
        }

        //Create Role
        [HttpPost]
        public async Task<IActionResult> Create(string rolename)
        {
            if (ModelState.IsValid)
            {
                var result =await roleManager.CreateAsync(new IdentityRole(rolename));

                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            return View(rolename);
        }



        //Delete Role
        [HttpPost]
        public async Task<IActionResult> Delete(string id)
        {
            var role = await roleManager.FindByIdAsync(id);

            if(role != null)
            {
                var result = await roleManager.DeleteAsync(role);

                if (result.Succeeded)
                {
                    TempData["message"] = $"{role.Name} has been deleted";
                    return RedirectToAction("Index");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        ModelState.AddModelError("",error.Description);
                    }
                }

            }

            return RedirectToAction("Index");
        }


        //Edit Role
        [HttpGet]
        public async Task<IActionResult> Edit(string id)
        {
            IdentityRole role = await roleManager.FindByIdAsync(id);

            var members = new List<ApplicationUser>();
            var nonMembers= new List<ApplicationUser>();

            foreach (var user in userManager.Users)
            {
                //user role.Name rolünde mi ? true ise listeyi members a eşitle,false ise nonmembers a eşitle
                var list=await userManager.IsInRoleAsync(user,role.Name)?members:nonMembers;
                list.Add(user);
            }

            var model = new RoleDetails()
            {
                Role = role,
                NonMembers = nonMembers,
                Members = members
            };

            return View(model);
        }


        //Edit Role POST
        [HttpPost]
        public async Task<IActionResult> Edit(RoleEditModel model)
        {

            IdentityResult result;

            if (ModelState.IsValid)
            {
                //ids to add
                foreach (var userId in model.IdsToAdd ?? new string[] {})  // model.IsToAdd boş değilse bunu bir string dizisi olarak tanımla
                {
                    var user = await userManager.FindByIdAsync(userId);

                    if(user != null)
                    {
                        result= await userManager.AddToRoleAsync(user,model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                //ids to delete
                foreach (var userId in model.IdsToDelete ?? new string[] { })
                {
                    var user = await userManager.FindByIdAsync(userId);

                    if (user != null)
                    {
                        result = await userManager.RemoveFromRoleAsync(user, model.RoleName);

                        if (!result.Succeeded)
                        {
                            foreach (var error in result.Errors)
                            {
                                ModelState.AddModelError("", error.Description);
                            }
                        }
                    }
                }

                
            }

            if (ModelState.IsValid)
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Edit", model.RoleId);
            }
        }
    }
}

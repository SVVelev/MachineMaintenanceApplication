namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using System;
    using System.Threading.Tasks;

    using MachineMaintenanceApp.Services.Data.Users;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Users.Details;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Users.Edit;
    using MachineMaintenanceApp.Web.ViewModels.Administration.Users.UsersPage;
    using Microsoft.AspNetCore.Mvc;

    public class UserController : AdministrationController
    {
        private const int ItemsPerPage = 6;

        private readonly IUserService userService;

        public UserController(IUserService userService)
        {
            this.userService = userService;
        }

        public IActionResult Edit(string id)
        {
            var viewModel = this.userService.GetById<AdminUserEditViewModel>(id);

            return this.View(viewModel);
        }

        public async Task<IActionResult> Delete(string id)
        {
            try
            {
                var companyId = await this.userService.DeleteAsync(id);

                return this.RedirectToAction(nameof(this.UsersPage), new { id = companyId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public async Task<IActionResult> Undelete(string id)
        {
            try
            {
                var userId = await this.userService.UndeleteAsync(id);

                return this.RedirectToAction(nameof(this.Details), new { id = userId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        public IActionResult Details(string id)
        {
            var viewModel = this.userService.GetByIdWithDeleted<AdminUserDetailViewModel>(id);

            if (viewModel == null)
            {
                return this.Redirect("/Home/NotFound");
            }

            return this.View(viewModel);
        }

        public IActionResult UsersPage(string id, int page = 1)
        {
            var count = this.userService.GetCountAllForCompanyWithDeleted(id);

            var viewModel = new UserPageViewModel
            {
                Users =
                    this.userService.GetAllForCompanyWithDeleted<AdminUserPageViewModel>(id),

                PagesCount = (int)Math.Ceiling((double)count / ItemsPerPage),
                CurrentPage = page,
                CompanyId = id,
            };

            if (viewModel.PagesCount == 0)
            {
                viewModel.PagesCount = 1;
            }

            return this.View(viewModel);
        }

        public async Task<IActionResult> ResetPassword(string id)
        {
            try
            {
                var userId = await this.userService.ResetPassword(id);

                return this.RedirectToAction(nameof(this.Details), new { id = userId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(AdminUserEditViewModel input)
        {
            if (!this.ModelState.IsValid)
            {
                return this.View(input);
            }

            try
            {
                var userId = await this.userService.EditAsync(input.PhoneNumber, input.FirstName, input.LastName, input.LastName, input.ImageUrl, input.Position, input.Id, input.Username);
                return this.RedirectToAction(nameof(this.Details), new { id = userId });
            }
            catch (ArgumentNullException)
            {
                return this.Redirect("/Home/NotFound");
            }
        }
    }
}

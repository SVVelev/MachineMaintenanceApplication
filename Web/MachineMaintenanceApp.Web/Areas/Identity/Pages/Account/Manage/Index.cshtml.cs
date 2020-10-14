namespace MachineMaintenanceApp.Web.Areas.Identity.Pages.Account.Manage
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Threading.Tasks;
    using MachineMaintenanceApp.Data.Models;
    using MachineMaintenanceApp.Data.Models.Enums;
    using MachineMaintenanceApp.Services.Data.Users;
    using MachineMaintenanceApp.Services.Mapping;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.RazorPages;

    public partial class IndexModel : PageModel
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> signInManager;
        private readonly IUserService userService;

        public IndexModel(
            UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager,
            IUserService userService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.userService = userService;
        }

        public string Username { get; set; }

        [TempData]
        public string StatusMessage { get; set; }

        [BindProperty]
        public InputModel Input { get; set; }

        public class InputModel
        {
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Required]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            [StringLength(20, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 3)]
            [Required]
            public string LastName { get; set; }

            [Display(Name = "Card Number")]
            [RegularExpression("[0-9]{6}")]
            [Required]
            public string CardNumber { get; set; }

            [Display(Name = "Profil Image")]
            [Required]
            public string ImageUrl { get; set; }

            [Required]
            public PositionType Position { get; set; }

            [Required]
            public string Username { get; set; }
        }

        private async Task LoadAsync(ApplicationUser user)
        {
            var userName = await this.userManager.GetUserNameAsync(user);
            var phoneNumber = await this.userManager.GetPhoneNumberAsync(user);

            this.Username = userName;

            this.Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                CardNumber = user.CardNumber,
                ImageUrl = user.ImageUrl,
                Position = user.Position,
                Username = userName,
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            await this.LoadAsync(user);
            return this.Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await this.userManager.GetUserAsync(this.User);
            if (user == null)
            {
                return this.NotFound($"Unable to load user with ID '{this.userManager.GetUserId(this.User)}'.");
            }

            if (!this.ModelState.IsValid)
            {
                await this.LoadAsync(user);
                return this.Page();
            }

            var updatedUserId = await this.userService.EditAsync(this.Input.PhoneNumber, this.Input.FirstName, this.Input.LastName, this.Input.CardNumber, this.Input.ImageUrl, this.Input.Position, user.Id, this.Input.Username);

            if (updatedUserId == null)
            {
                var userId = await this.userManager.GetUserIdAsync(user);
                throw new InvalidOperationException($"Unexpected error occurred while updating user with ID '{userId}'.");
            }

            //if (!userResult.Succeeded)
            //{
            //    var userId = await this.userManager.GetUserIdAsync(user);
            //    throw new InvalidOperationException($"Unexpected error occurred while updating user with ID '{userId}'.");
            // }
            await this.signInManager.RefreshSignInAsync(user);
            this.StatusMessage = "Your profile has been updated";
            return this.RedirectToPage();
        }
    }
}

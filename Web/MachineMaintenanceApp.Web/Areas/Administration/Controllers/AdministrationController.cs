namespace MachineMaintenanceApp.Web.Areas.Administration.Controllers
{
    using MachineMaintenanceApp.Common;
    using MachineMaintenanceApp.Web.Controllers;

    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    [Authorize(Roles = GlobalConstants.AdministratorRoleName)]
    [Area("Administration")]
    public class AdministrationController : BaseController
    {
    }
}

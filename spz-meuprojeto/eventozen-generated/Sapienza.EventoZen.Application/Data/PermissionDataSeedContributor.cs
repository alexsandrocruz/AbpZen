using System.Threading.Tasks;
using Sapienza.EventoZen.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Authorization.Permissions;

namespace Sapienza.EventoZen.Data
{
    public class PermissionDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IPermissionManager _permissionManager;

        public PermissionDataSeedContributor(IPermissionManager permissionManager)
        {
            _permissionManager = permissionManager;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            // Grant all generated permissions to the admin role
            var adminRoleName = "admin";

            await _permissionManager.SetAsync(ClientPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ClientPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ClientPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ClientPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(ArtistPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(ArtistSpecialtyPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistSpecialtyPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistSpecialtyPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(ArtistSpecialtyPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(AvailabilityPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(AvailabilityPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(AvailabilityPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(AvailabilityPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(EventPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(EventCommissionPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventCommissionPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventCommissionPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(EventCommissionPermissions.Delete, "R", adminRoleName, true);

            await _permissionManager.SetAsync(LocationPermissions.Default, "R", adminRoleName, true);
            await _permissionManager.SetAsync(LocationPermissions.Create, "R", adminRoleName, true);
            await _permissionManager.SetAsync(LocationPermissions.Update, "R", adminRoleName, true);
            await _permissionManager.SetAsync(LocationPermissions.Delete, "R", adminRoleName, true);
        }
    }
}

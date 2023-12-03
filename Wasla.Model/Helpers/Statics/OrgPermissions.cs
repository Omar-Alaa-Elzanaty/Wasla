
using Wasla.Model.Helpers.Enums;

namespace Wasla.Model.Helpers.Statics
{
    public class OrgPermissions
    {
        public static List<string> GeneratePermissionsList(string module)
        {
            return new List<string>()
            {
                $"{PermissionsName.Org_Permission}.{module}.Create.1",
                $"{PermissionsName.Org_Permission}.{module}.Update.2",
                $"{PermissionsName.Org_Permission}.{module}.View.3",
                $"{PermissionsName.Org_Permission}.{module}.Delete.4"
            };
        }
        public static List<string> GenerateAllPermissions()
        {
            var allPermissions = new List<string>();
            var modules = Enum.GetValues(typeof(ModulesPermissions));
            foreach (var module in modules)
              allPermissions.AddRange(GeneratePermissionsList(module.ToString()));
            return allPermissions;
        }
    }
}


using Wasla.Model.Helpers;

namespace Wasla.Model.Dtos
{
    public class OrgEmployeeResponseDto : DataAuthResponse
    {
        public List<string> OrgPermissions { get; set; }

    }
}

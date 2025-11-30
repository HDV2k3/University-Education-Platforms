using Microsoft.AspNetCore.Mvc;
using UNIVERSITY.EDUCATION.PLATFORMS.Constants;

namespace UNIVERSITY.EDUCATION.PLATFORMS.Authorization
{
    public class ClaimRequirementAttribute : TypeFilterAttribute
    {
        public ClaimRequirementAttribute(CommandCode CommandCode)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { CommandCode };
        }

        public ClaimRequirementAttribute(CommandCode[] CommandCodes)
            : base(typeof(ClaimRequirementFilter))
        {
            Arguments = new object[] { CommandCodes };
        }
    }
}

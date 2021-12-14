﻿namespace SFA.DAS.ApplyService.Web.Infrastructure
{
    using InternalApi.Types.CharityCommission;
    using SFA.DAS.ApplyService.InternalApi.Types;
    using System.Threading.Tasks;

    public interface ICharityCommissionOuterApiClient
    {
        Task<ApiResponse<Charity>> GetCharityDetails(int charityNumber);
    }
}

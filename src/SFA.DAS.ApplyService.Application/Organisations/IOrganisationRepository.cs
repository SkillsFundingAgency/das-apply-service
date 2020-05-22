﻿using SFA.DAS.ApplyService.Domain.Entities;
using System;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Application.Organisations
{
    public interface IOrganisationRepository
    {
        Task<Guid> CreateOrganisation(Organisation organisation, Guid userId);
        Task<Organisation> GetOrganisationByContactEmail(string email);
        Task<Organisation> GetOrganisationByName(string name);
        Task<Organisation> GetOrganisationByApplicationId(Guid applicationId);
        Task UpdateOrganisation(Organisation organisation, Guid userId);
        Task UpdateOrganisation(Organisation organisation);
        Task<Organisation> GetUserOrganisation(Guid userId);
        Task<Organisation> GetOrganisationByUserId(Guid userId);
        Task<Organisation> GetOrganisationByUkprn(string ukprn);
        Task<Guid> CreateOrganisation(Organisation organisation);
        Task UpdateOrganisation(Guid organisationId, Guid userId);
        Task<Organisation> GetOrganisation(Guid organisationId);
    }
}
using Dapper;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Domain.Interfaces;
using SFA.DAS.ApplyService.Infrastructure.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SFA.DAS.ApplyService.Data
{
    public class OrganisationAddressesRepository : IOrganisationAddressesRepository
    {
        private readonly IDbConnectionHelper _dbConnectionHelper;

        public OrganisationAddressesRepository(IDbConnectionHelper dbConnectionHelper)
        {
            _dbConnectionHelper = dbConnectionHelper;
        }

        public async Task<int> CreateOrganisationAddresses(OrganisationAddresses organisationAddresses)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                organisationAddresses.Id = await connection.QuerySingleAsync<int>(@"INSERT INTO OrganisationAddresses (OrganisationId, AddressType, AddressLine1, AddressLine2, AddressLine3, City, Postcode)" + 
                                               "OUTPUT INSERTED.[Id] " + "VALUES (@OrganisationId, @AddressType, @AddressLine1, @AddressLine2, @AddressLine3, @City, @Postcode)",
                    new { organisationAddresses.OrganisationId, organisationAddresses.AddressType, organisationAddresses.AddressLine1, organisationAddresses.AddressLine2, 
                        organisationAddresses.AddressLine3, organisationAddresses.City, organisationAddresses.Postcode });

                return organisationAddresses.Id;
            }
        }

        public async Task UpdateOrganisationAddresses(OrganisationAddresses organisationAddresses)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                await connection.ExecuteAsync(
                    @"UPDATE OrganisationAddresses SET AddressType = @AddressType, AddressLine1 = @AddressLine1, AddressLine2 = @AddressLine2, AddressLine3 = @AddressLine3, City = @City,Postcode = @Postcode WHERE OrganisationId = @OrganisationId",
                    new { organisationAddresses.AddressType, organisationAddresses.AddressLine1, organisationAddresses.AddressLine2, organisationAddresses.AddressLine3,organisationAddresses.City,organisationAddresses.Postcode,organisationAddresses.OrganisationId});
            }
        }

        public async Task<List<OrganisationAddresses>> GetOrganisationAddressesByOrganisationId(Guid organisationId)
        {
            using (var connection = _dbConnectionHelper.GetDatabaseConnection())
            {
                var sql =
                    @"SELECT *
                    FROM OrganisationAddresses 
                    WHERE OrganisationId = @OrganisationId";

                var orgAddresses = (await connection.QueryAsync<OrganisationAddresses>(sql, new { organisationId })).ToList();

                return orgAddresses;
            }
        }
    }
}

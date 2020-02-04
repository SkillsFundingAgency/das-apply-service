using AutoMapper;
using System;
using System.Linq;

namespace SFA.DAS.ApplyService.InternalApi.AutoMapper
{
    using Domain.CompaniesHouse;
    using Models.CompaniesHouse;

    public class CompaniesHouseCompanyProfile : Profile
    {
        public CompaniesHouseCompanyProfile()
        {
            CreateMap<Models.CompaniesHouse.CompanyDetails, Types.CompaniesHouse.Company>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(source => source.company_name))
                .ForMember(dest => dest.CompanyNumber, opt => opt.MapFrom(source => source.company_number))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.company_name))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(source => source.company_status))
                .ForMember(dest => dest.Type, opt => opt.MapFrom(source => source.type))
                .ForMember(dest => dest.NatureOfBusiness, opt => opt.MapFrom(source => source.sic_codes))
                .ForMember(dest => dest.IncorporatedOn, opt => opt.MapFrom(source => source.date_of_creation))
                .ForMember(dest => dest.DissolvedOn, opt => opt.MapFrom(source => source.date_of_cessation))
                .ForMember(dest => dest.IsLiquidated, opt => opt.MapFrom(source => source.has_been_liquidated))
                .ForMember(dest => dest.PreviousNames, opt => opt.ResolveUsing(source => source.previous_company_names?.Select(pc => pc.name)))
                .ForMember(dest => dest.RegisteredOfficeAddress, opt => opt.MapFrom(source => Mapper.Map<Models.CompaniesHouse.RegisteredOfficeAddress, Types.CompaniesHouse.Address>(source.registered_office_address))) 
                .ForMember(dest => dest.Accounts, opt => opt.MapFrom(source => Mapper.Map<Models.CompaniesHouse.CompanyDetails, Types.CompaniesHouse.Accounts>(source)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHouseAccountsProfile : Profile
    {
        public CompaniesHouseAccountsProfile()
        {
            CreateMap<Models.CompaniesHouse.CompanyDetails, Types.CompaniesHouse.Accounts>()
                .ForMember(dest => dest.LastConfirmationStatementDate, opt => opt.ResolveUsing(source => source.confirmation_statement?.last_made_up_to))
                .ForMember(dest => dest.LastAccountsDate, opt => opt.ResolveUsing(source => source.accounts?.last_accounts?.made_up_to))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHouseRegisteredOfficeAddressProfile : Profile
    {
        public CompaniesHouseRegisteredOfficeAddressProfile()
        {
            CreateMap<Models.CompaniesHouse.RegisteredOfficeAddress, Types.CompaniesHouse.Address>()
                .ForMember(dest => dest.AddressLine1, opt => opt.ResolveUsing(source => $"{source.po_box} {source.premises} {source.address_line_1}".TrimStart()))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(source => source.address_line_2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.locality))
                .ForMember(dest => dest.County, opt => opt.MapFrom(source => source.region))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(source => source.country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.postal_code))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHouseOfficerAddressProfile : Profile
    {
        public CompaniesHouseOfficerAddressProfile()
        {
            CreateMap<Models.CompaniesHouse.OfficerAddress, Types.CompaniesHouse.Address>()
                .ForMember(dest => dest.AddressLine1, opt => opt.ResolveUsing(source => $"{source.po_box} {source.premises} {source.address_line_1}".TrimStart()))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(source => source.address_line_2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.locality))
                .ForMember(dest => dest.County, opt => opt.MapFrom(source => source.region))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(source => source.country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.postal_code))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHouseOfficerProfile : Profile
    {
        public CompaniesHouseOfficerProfile()
        {
            CreateMap<Models.CompaniesHouse.Officer, Types.CompaniesHouse.Officer>()
                .ForMember(dest => dest.Id, opt => opt.ResolveUsing(source => source.links?.officer?.appointments?.Replace("/officers/", string.Empty).Replace("/appointments", string.Empty)))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.name))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(source => source.officer_role))
                .ForMember(dest => dest.DateOfBirth, opt => opt.ResolveUsing(source => source.date_of_birth is null ? DateTime.MinValue : new DateTime(source.date_of_birth.year, source.date_of_birth.month, source.date_of_birth.day ?? 1)))
                .ForMember(dest => dest.AppointedOn, opt => opt.MapFrom(source => source.appointed_on))
                .ForMember(dest => dest.ResignedOn, opt => opt.MapFrom(source => source.resigned_on))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.CompaniesHouse.OfficerAddress, Types.CompaniesHouse.Address>(source.address)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHouseOfficerDisqualificationProfile : Profile
    {
        public CompaniesHouseOfficerDisqualificationProfile()
        {
            CreateMap<Models.CompaniesHouse.Disqualification, Types.CompaniesHouse.Disqualification>()
                .ForMember(dest => dest.DisqualifiedFrom, opt => opt.MapFrom(source => source.disqualified_from))
                .ForMember(dest => dest.DisqualifiedUntil, opt => opt.MapFrom(source => source.disqualified_until))
                .ForMember(dest => dest.CaseIdentifier, opt => opt.MapFrom(source => source.case_identifier))
                .ForMember(dest => dest.Reason, opt => opt.ResolveUsing(source => source.reason?.act))
                .ForMember(dest => dest.ReasonDescription, opt => opt.ResolveUsing(source => source.reason?.description_identifier))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHousePersonWithSignificantControlProfile : Profile
    {
        public CompaniesHousePersonWithSignificantControlProfile()
        {
            CreateMap<Models.CompaniesHouse.PersonWithSignificantControl, Types.CompaniesHouse.PersonWithSignificantControl>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.links.self))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.name))
                .ForMember(dest => dest.DateOfBirth, opt => opt.ResolveUsing(source => source.date_of_birth is null ? DateTime.MinValue : new DateTime(source.date_of_birth.year, source.date_of_birth.month, source.date_of_birth.day ?? 1)))
                .ForMember(dest => dest.NaturesOfControl, opt => opt.MapFrom(source => source.natures_of_control))
                .ForMember(dest => dest.NotifiedOn, opt => opt.MapFrom(source => source.notified_on))
                .ForMember(dest => dest.CeasedOn, opt => opt.MapFrom(source => source.ceased_on))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(source => Mapper.Map<Models.CompaniesHouse.PersonWithSignificantControlAddress, Types.CompaniesHouse.Address>(source.address)))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }

    public class CompaniesHousePersonWithSignificantControlAddressProfile : Profile
    {
        public CompaniesHousePersonWithSignificantControlAddressProfile()
        {
            CreateMap<Models.CompaniesHouse.PersonWithSignificantControlAddress, Types.CompaniesHouse.Address>()
                .ForMember(dest => dest.AddressLine1, opt => opt.ResolveUsing(source => $"{source.po_box} {source.premises} {source.address_line_1}".TrimStart()))
                .ForMember(dest => dest.AddressLine2, opt => opt.MapFrom(source => source.address_line_2))
                .ForMember(dest => dest.City, opt => opt.MapFrom(source => source.locality))
                .ForMember(dest => dest.County, opt => opt.MapFrom(source => source.region))
                .ForMember(dest => dest.Country, opt => opt.MapFrom(source => source.country))
                .ForMember(dest => dest.PostalCode, opt => opt.MapFrom(source => source.postal_code))
                .ForAllOtherMembers(dest => dest.Ignore());
        }
    }
}

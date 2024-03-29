﻿using AutoMapper;

namespace SFA.DAS.ApplyService.Web.AutoMapper
{
    using Domain.CharityCommission;

    public class CharityCommissionProfile : Profile
    {
        public CharityCommissionProfile()
        {
            CreateMap<InternalApi.Types.CharityCommission.Charity, CharityCommissionSummary>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForMember(dest => dest.CharityNumber, opt => opt.MapFrom(source => source.CharityNumber))
                .ForMember(dest => dest.RegistrationDate, opt => opt.MapFrom(source => source.RegistrationDate ))
                .ForMember(dest => dest.RegistrationNumber, opt => opt.MapFrom(source => source.RegistrationNumber))
                .ForMember(dest => dest.Trustees, opt => opt.MapFrom(source => source.Trustees))
                .ForMember(dest => dest.TrusteeManualEntryRequired, opt => opt.MapFrom(source => source.TrusteeManualEntryRequired))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }

    public class CharityTrusteeProfile : Profile
    {
        public CharityTrusteeProfile()
        {
            CreateMap<InternalApi.Types.CharityCommission.Trustee, Trustee>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(source => source.Id))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(source => source.Name))
                .ForAllOtherMembers(opt => opt.Ignore());
        }
    }
}

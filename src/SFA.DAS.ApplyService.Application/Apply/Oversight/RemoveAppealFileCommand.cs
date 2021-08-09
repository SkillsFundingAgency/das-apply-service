﻿using System;
using MediatR;

namespace SFA.DAS.ApplyService.Application.Apply.Oversight
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public class RemoveAppealFileCommand : IRequest
    {
        public Guid ApplicationId { get; set; }
        public Guid FileId { get; set; }
        public string UserId { get; set; }
        public string UserName { get; set; }
    }
}

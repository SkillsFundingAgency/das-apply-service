﻿namespace SFA.DAS.ApplyService.Domain.Audit
{
    public enum UserAction
    {
        UpdateGatewayPageOutcome,
        UpdateGatewayPageClarificationOutcome,
        UpdateGatewayPagePostClarification,
		RecordOversightOutcome,
        RecordOversightGatewayFailOutcome,
        RecordOversightGatewayRemovedOutcome,
        UpdateGatewayReviewStatus,
        WithdrawApplication,
        MakeAppeal,
        CancelAppeal,
        UploadAppealFile,
        RemoveAppealFile,
        RecordAppealOutcome
    }
}

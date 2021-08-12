namespace SFA.DAS.ApplyService.Domain.Audit
{
    // TODO: APPEALREVIEW - Review once appeal work starts
    public enum UserAction
    {
        UpdateGatewayPageOutcome,
        UpdateGatewayPageClarificationOutcome,
        UpdateGatewayPagePostClarification,
		RecordOversightOutcome,
        RecordOversightGatewayFailOutcome,
        RecordOversightGatewayRemovedOutcome,
        UpdateGatewayReviewStatus,
        UploadAppealFile,
        RemoveAppealFile,
        WithdrawApplication,
        CreateAppeal
    }
}

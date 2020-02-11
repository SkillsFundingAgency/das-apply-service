using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SFA.DAS.ApplyService.Domain.Apply;
using SFA.DAS.ApplyService.Domain.Entities;
using SFA.DAS.ApplyService.Web.Infrastructure.Interfaces;
using SFA.DAS.ApplyService.Web.ViewModels.Roatp;

namespace SFA.DAS.ApplyService.Web.Infrastructure.Services
{
    public class PagesWithSectionsFlowService: IPagesWithSectionsFlowService
    {
        public ApplicationSection ProcessPagesInSectionsForStatusText(ApplicationSection selectedSection)
        {
            foreach (var page in selectedSection.QnAData.Pages.Where(x => x.DisplayType == SectionDisplayType.PagesWithSections))
            {
                page.StatusText = AssociatedPagesWithSectionStatus(page, selectedSection.QnAData, true);
            }

            return selectedSection;
        }

        private string AssociatedPagesWithSectionStatus(Page page, QnAData selectedSectionQnAData, bool isFirstPage)
        {
            if (isFirstPage && page.Complete != true) return "";
            if (page.Next.All(x => x.Action != "NextPage")) return TaskListSectionStatus.Completed;

            foreach (var nxt in page.Next.Where(x => x.Action == "NextPage"))
            {
                var pageId = nxt.ReturnId;

                var pageNext = selectedSectionQnAData.Pages.FirstOrDefault(x => x.PageId == pageId && x.Active);
                if (pageNext != null)
                {
                    return !pageNext.Complete ? TaskListSectionStatus.InProgress : AssociatedPagesWithSectionStatus(pageNext, selectedSectionQnAData, false);
                }
            }

            return TaskListSectionStatus.Completed;
        }
    }
}

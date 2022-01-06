﻿using System;
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
           
            foreach (var page in selectedSection.QnAData.Pages.Where(x =>
                x.DisplayType == SectionDisplayType.PagesWithSections))
            {
                var pages = new List<Page>();
                GatherListOfPages(page, selectedSection.QnAData, pages);

                page.StatusText = pages.All(x => x.Complete) ? TaskListSectionStatus.Completed :
                    pages.Any(x => x.Complete) ? TaskListSectionStatus.InProgress : string.Empty;
            }

            return selectedSection;
        }

        private static void GatherListOfPages(Page page, QnAData selectedSectionQnAData, ICollection<Page> pages)
        {
            pages.Add(page);

            if (page.Next.All(x => x.Action != NextAction.NextPage)) return;

            foreach (var nxt in page.Next.Where(x => x.Action == NextAction.NextPage))
            {
                var pageId = nxt.ReturnId;

                var pageNext = selectedSectionQnAData.Pages.FirstOrDefault(x => x.PageId == pageId && x.Active);

                if (pageNext == null)
                    return;

                GatherListOfPages(pageNext, selectedSectionQnAData, pages);
            }
        }
    }
}

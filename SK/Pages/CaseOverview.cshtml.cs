using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using System.Linq;

namespace SK.Pages
{
    [Authorize]
    public class CaseOverviewModel : PageModel
    {
        private readonly IWebHostEnvironment _rootEnv;

        private IHubContext<Hubs.NotificationHub> _hubContext;


        public CaseOverviewModel(IWebHostEnvironment env, IHubContext<Hubs.NotificationHub> hubContext)
        {
            _rootEnv = env; //for root path
            _hubContext = hubContext;
        }
        [BindProperty(SupportsGet = true)]
        public string GoToPage { get; set; } = "1";
        public int numberOfRowsOnPage { get; } = 10;
        public string DBEntriesCount { get; set; }

        [BindProperty(SupportsGet = true)]
        public string searchString { get; set; }

        [BindProperty(SupportsGet = true)]
        public string SelectedId { get; set; } = "";
        public List<Models.Case> Li { get; set; } = new List<Models.Case>();

        [BindProperty(SupportsGet = true)]
        public string CaseRadioFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public string CaseRadioSort { get; set; }

        public void OnGet()
        {
            int.TryParse(GoToPage, out int targetPage);
            int fromRowNumber = (targetPage - 1) * numberOfRowsOnPage;

            var condition = CaseRadioFilter == "deadline" ? " end_date IS NOT NULL" : "";

            if (CaseRadioFilter == "currentDeadline")
            {
                string dateNow = Convert.ToDateTime(Models.CentralEuTimeZone.Get()).ToString("yyyy-MM-dd HH:mm:ss");
                condition = $"end_date IS NOT NULL AND end_date > TO_TIMESTAMP( '{dateNow}' , 'YYYY/MM/DD HH24:MI:SS')";
            }

            var searchQuery = !string.IsNullOrEmpty(searchString) ?
               $"  (LOWER(first_name) LIKE '%{searchString.ToLower()}%' OR" +
               $"  LOWER(last_name) LIKE '%{searchString.ToLower()}%'  OR " +
               $"  LOWER(unique_number) LIKE '%{searchString.ToLower()}%' OR" +
               $"  LOWER(telephone) LIKE '%{searchString.ToLower()}%' OR " +
               $"  LOWER(email) LIKE '%{searchString.ToLower()}%' OR" +
               $"  LOWER(manufacturer) LIKE '%{searchString.ToLower()}%' OR " +
               $"  LOWER(sn) LIKE '%{searchString.ToLower()}%')"
               : "";

            DBEntriesCount = Models.Case.GetNumberOfRows(condition, searchQuery);


            try
            {
                //if (CaseRadioFilter == "deadline")
                //    Li = Li.Where(x => !string.IsNullOrEmpty(x.EndDate)).ToList();

                if (CaseRadioSort == "name")
                    Li = Models.Case.GetCaseList(condition, searchQuery, "ORDER BY first_name ASC NULLS LAST",
                        fromRowNumber.ToString(), numberOfRowsOnPage.ToString());
                else if (CaseRadioSort == "lastName")
                    Li = Models.Case.GetCaseList(condition, searchQuery, "ORDER BY last_name ASC NULLS LAST",
                        fromRowNumber.ToString(), numberOfRowsOnPage.ToString());
                else if (CaseRadioSort == "deadline")
                    Li = Models.Case.GetCaseList(condition, searchQuery, "ORDER BY end_date ASC NULLS LAST",
                        fromRowNumber.ToString(), numberOfRowsOnPage.ToString());
                else
                    Li = Models.Case.GetCaseList(condition, searchQuery, "ORDER BY start_date DESC NULLS LAST",
                        fromRowNumber.ToString(), numberOfRowsOnPage.ToString());
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sorting CaseList: " + ex);
            }



            /*
            if(!string.IsNullOrEmpty(searchString))
                Li = Models.Case.SearchCase(searchString.ToLower());
            else
                Li = Models.Case.GetCaseList();
            try
            {
                if (CaseRadioFilter == "deadline")
                    Li = Li.Where(x => !string.IsNullOrEmpty(x.EndDate)).ToList();

                if (CaseRadioSort == "name")
                    Li = Li.OrderBy(x => string.IsNullOrWhiteSpace(x.FirstName))
                        .ThenBy(x => x.FirstName).ThenBy(x => x.LastName).ToList();
                else if (CaseRadioSort == "lastName")
                    Li = Li.OrderBy(x => string.IsNullOrWhiteSpace(x.LastName))
                        .ThenBy(x => x.LastName).ThenBy(x => x.FirstName).ToList();
                else if (CaseRadioSort == "deadline")
                    Li = Li.OrderBy(x => string.IsNullOrWhiteSpace(x.EndDate)).ThenBy(x => string.IsNullOrEmpty(x.EndDate) ?
                          new DateTime() : Convert.ToDateTime(x.EndDate)).ToList(); //new DateTime() as empty date 
                else
                    Li = Li.OrderByDescending(x => string.IsNullOrEmpty(x.StartDate) ?
                         new DateTime() : Convert.ToDateTime(x.StartDate)).ToList(); //new DateTime() as empty date 
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error sorting CaseList: "+ex);
            }
          */

        }

        public void OnPostAjaxmarkTableRow(string idToMark, bool boolMark)
        {
            string dateNow = Convert.ToDateTime(Models.CentralEuTimeZone.Get()).ToString("yyyy-MM-dd HH:mm");

            if (boolMark)
                Models.Case.UpdateCaseStatus(idToMark, Models.Case.StatusType.Inactive, dateNow);
            else //reverse
                Models.Case.UpdateCaseStatus(idToMark, "", "");


            var CurrentLoggeId = User.Identity.Name;
            if (CurrentLoggeId != null)
                _hubContext.Clients.Group(CurrentLoggeId).SendAsync("ReceiveMessage", "sss");
        }

        public void OnPostAjaxUpdateCaseDescription(string caseId, string description)
        {
            Models.Case.UpdateSingleColumn(caseId, Models.DbColumns.description, description);
        }

        public IActionResult OnPostAjaxRemoveCase(string idToMark)
        {

            var imageIdList = Models.Case.GetImageNames(idToMark).resultLiImageNamesOnly;
            foreach (var fileName in imageIdList)
            {
                string webRootPath = _rootEnv.WebRootPath;
                var FilePath = Path.Combine(webRootPath, "uploads/img");
                var filePath = Path.Combine(FilePath, fileName);
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
            }

            var result = Models.Case.RemoveCase(idToMark);
            return new JsonResult(result);
        }

        public void OnPostAjaxHilightCase(string idToMark, bool boolMark)
        {
            Console.WriteLine("ojojsadas::::" + boolMark);
            if (boolMark)
                Models.Case.UpdateSingleColumn(idToMark, Models.DbColumns.highlight, "y");
            else
                Models.Case.UpdateSingleColumn(idToMark, Models.DbColumns.highlight, "n");

        }

        public IActionResult OnPostAjaxGetCaseImages(string caseId)
        {
            var imageObjLi = Models.Case.GetImages(caseId);
            var resultLi = new List<string>();

            foreach (var el in imageObjLi)
            {
                resultLi.Add(Convert.ToBase64String(el.ImageBytes));
            }

            return new JsonResult(resultLi);
        }

        public void OnGetAjaxSearchCase()
        {
            Li = Models.Case.SearchCase(searchString.ToLower());
        }

        public void OnPostSaveDisplayOptions()
        {
            Console.WriteLine(CaseRadioFilter);
            Console.WriteLine(CaseRadioSort);
        }
    }
}

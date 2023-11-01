using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SK.Pages
{
    public class ViewCaseModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SelectedId { get; set; } = "";

        [BindProperty]
        public Models.Case Case { get; set; }

        [BindProperty]
        public List<Models.CaseImage> ImageObjLi { get; set; } = new List<Models.CaseImage>();
        public void OnGet()
        {
            Case = Models.Case.GetSingleCase(SelectedId);
            ImageObjLi = Models.Case.GetImages(SelectedId);

            try
            {
                if (!string.IsNullOrEmpty(Case.StartDate))
                    Case.StartTimeOnly = Case.StartDate.Split()[1];
                if (!string.IsNullOrEmpty(Case.EndDate))
                    Case.EndTimeOnly = Case.EndDate.Split()[1];
                if (!string.IsNullOrEmpty(Case.StartDate))
                    Case.StartDate = Convert.ToDateTime(Case.StartDate).Date.ToString("yyyy-MM-dd");
                if (!string.IsNullOrEmpty(Case.EndDate))
                    Case.EndDate = Convert.ToDateTime(Case.EndDate).Date.ToString("yyyy-MM-dd");
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
            }
        }
    }
}

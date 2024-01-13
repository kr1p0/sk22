using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace SK.Pages
{
    public class PrintCaseModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string SelectedId { get; set; } = "";

        [BindProperty(SupportsGet = true)]
        public string SelectedPrintType { get; set; } = "";
        public Models.Case Case { get; set; } = new Models.Case();
        public void OnGet()
        {
            Case = Models.Case.GetSingleCase(SelectedId);
        }
    }
}

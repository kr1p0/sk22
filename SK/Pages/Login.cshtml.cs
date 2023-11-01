using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Security.Claims;

namespace SK.Pages
{
    public class LoginModel : PageModel
    {
        [BindProperty]
        public string LoginVal { get; set; }
        [BindProperty]
        public string PasswordVal { get; set; }
        [BindProperty]
        public string AlertLoginResult { get; set; }
        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPostLogin(string returnurl)
        {
            if(string.IsNullOrEmpty(LoginVal)) return Page();

            Console.WriteLine(ModelState.IsValid);

            var userInfo = Models.Account.GetUser(LoginVal);

            if (!string.IsNullOrEmpty(userInfo.Password))
            {
                if (PasswordVal == userInfo.Password)
                {
                    var claims = new List<Claim>();
                  
                    claims.Add(new Claim(ClaimTypes.Name, userInfo.Name));

                    var identity = new ClaimsIdentity(claims, "Ciastko");
                    ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(identity);
                    await HttpContext.SignInAsync("Ciastko", claimsPrincipal);
                    return string.IsNullOrEmpty(returnurl) ? RedirectToPage("/Index") : Redirect(returnurl); //"/Index"
                    //The RedirectToPage() method is referring to a Razor cshtml "page" rather than a html page
                    //If you want to redirect to a url you can use Redirect()
                }
                else
                    AlertLoginResult = "Nieprawidłowe hasło";
            }
            else
                AlertLoginResult = "Nieprawidłowy login";
            
            return Page();

        }

        public void OnPostPassRecover()
        {
            /*
            var userInfo = new User(ForgotenPasswordEmail);

            if (string.IsNullOrEmpty(userInfo.Email))
            {
                passAlert = "(🗙) Email nie widnieje w bazie";
                return;
            }

            var verifyingHash = CustomStr.GenerateRandomString(300);
            //var verifyingHash = BCrypt.Net.BCrypt.HashPassword(userInfo.Email + rndString);
            var obj = new VerifyingHash();
            obj.InsertHash(userInfo.UzytkownikId, verifyingHash);
            var currentHost = HttpContext.Request.Host;
            var link = $"https://{currentHost}/Account/ForgotPass?verifyingHash={verifyingHash}";
            string firstRow = $"<h1>Witaj, {userInfo.Imie} {userInfo.Nazwisko}. Prosiłeś o zmianę hasła.</h1><br>";
            string secondRow = "<h2>Aby potwierdzić tą czynność <a href= '" + link + "' > KLIK </a> </h2>";
            string divStyle = "background-color:#f6f6f8;border-radius:8px;align-content:center;" +
                "text-align:center;padding:10px;border:solid thin #e8e8ea";
            string mailbody = "<div style= '" + divStyle + "' >" + firstRow + secondRow + "</div>";
            var emaiList = new List<string>();
            emaiList.Add(userInfo.Email);
            passAlert = Email.sendMail(emaiList, "Potwierdzenie prośby o reset hasła", mailbody);


            ForgotenPasswordEmail = null;
            ModelState.Clear();
            */
        }
    }
}

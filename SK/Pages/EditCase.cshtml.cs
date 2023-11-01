using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;

namespace SK.Pages
{
    [Authorize]
    public class EditCaseModel : PageModel
    {
        private readonly IWebHostEnvironment _rootEnv;

        public EditCaseModel(IWebHostEnvironment env)
        {
            _rootEnv = env; //for root path
        }


        [BindProperty]
        public Models.Case Case { get; set; } = new Models.Case();
        [BindProperty]
        public List<Models.CaseImage> ImageObjLi { get; set; } = new List<Models.CaseImage>();

        [BindProperty (SupportsGet =true)]
        public string SelectedId { get; set; }

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
            catch(Exception ex)
            {
                Console.WriteLine(ex);
            }
     
        }

      
        private string checkDirectory(string originalPath)
        {
            int i = 1;
            string fileExtension = Path.GetExtension(originalPath);

            string fullPath = Path.GetFullPath(originalPath);
            var fullPathWithNoExtension = fullPath.Substring(0, fullPath.LastIndexOf(fileExtension));

            string addToName = "";
            FileInfo dir0 = new FileInfo(originalPath);

            while (dir0.Exists)
            {
                i++;
                addToName = "(" + i + ")";
                string temp = fullPathWithNoExtension + addToName + fileExtension;
                dir0 = new FileInfo(temp);
            }
            var result = fullPathWithNoExtension + addToName + fileExtension;
            return result;
        }


        public IActionResult OnPostAjaxEditCase(Models.Case CaseObj, List<string> fileUploadRemove = null, List<IFormFile>? fileUploadEdit = null)
        {



            var ImageDataLi = new List<Models.CaseImage>();
            var encoder = new JpegEncoder()
            {
                Quality = 90 //Use variable to set between 5-100 based on your requirements
            };

            foreach (var singleFile in fileUploadEdit)
            {
                if (singleFile != null)
                {
                    Models.CaseImage imageObj = new Models.CaseImage();
                    imageObj.ImageName = singleFile.FileName;


                    using (var image = Image.Load(singleFile.OpenReadStream()))
                    {
                        using (var ms = new MemoryStream())
                        {
                            if (image.Height > 1920)
                                image.Mutate(x => x.Resize(1920, 0));
                            image.Save(ms, encoder);
                            byte[] logoByteArray = ms.ToArray();
                            imageObj.ImageBytes = logoByteArray;
                        }

                    }

                    ImageDataLi.Add(imageObj);
                }
            }
            Models.Case.RemoveImage(fileUploadRemove);
            var result = Models.Case.UpdateCase(CaseObj, ImageDataLi);


            //SAVING IN DB , KEEPING THE ORIGINAL IMAGE SIZE
            /*
            var ImageDataLi = new List<Models.CaseImage>();

            foreach (var singleFile in fileUploadEdit)
            {
                if (singleFile != null)
                {
                    Models.CaseImage imageObj = new Models.CaseImage();
                    imageObj.ImageName = singleFile.FileName;

                    MemoryStream ms = new MemoryStream();
                    singleFile.CopyTo(ms);
                    byte[] logoByteArray = ms.ToArray();
                    imageObj.ImageBytes = logoByteArray;
                    ImageDataLi.Add(imageObj);
                }
            }
            Models.Case.RemoveImage(fileUploadRemove);
            var result = Models.Case.UpdateCase(CaseObj, ImageDataLi);
            */



            //FOR SAVING IMAGE ON SERVER
            /*
            var fileNameList = new List<string>();
            foreach (var singleFile in fileUploadEdit)
            {
               
                if (singleFile != null)
                {
                    var fileName = singleFile.Name;
                    string fileExtension = Path.GetExtension(singleFile.FileName);
                    fileName = Path.GetFileNameWithoutExtension(singleFile.FileName);
                    string webRootPath = _rootEnv.WebRootPath;
                    var FilePath = Path.Combine(webRootPath, "uploads/img");
                    if (!Directory.Exists(FilePath))
                        Directory.CreateDirectory(FilePath);
                    //fileName = Guid.NewGuid() + fileExtension; //unique file name
                    var dateT = "-" + DateTime.Now.ToString("dd-MM-yyyy");
                    fileName += dateT + fileExtension;
                    var filePath = Path.Combine(FilePath, fileName.ToString());
                    filePath = checkDirectory(filePath);
                    using (FileStream fs = System.IO.File.Create(filePath))
                    {
                        singleFile.CopyTo(fs);
                    }
                    Console.WriteLine(Path.GetFileName(filePath));
                    fileNameList.Add(Path.GetFileName(filePath));
                }
            }

            var result = Models.Case.UpdateCase(CaseObj, fileNameList);
           
            OnPostAjaxRemoveImage(fileUploadRemove);
            */

            return new JsonResult(result);
        }


        public void OnPostAjaxRemoveImage(List<string> imageIdList)
        {
            
           
            foreach(var imageId in imageIdList)
            {
                var fileName = Models.Case.GetImageNameByImgId(imageId);
                string webRootPath = _rootEnv.WebRootPath;
                var FilePath = Path.Combine(webRootPath, "uploads/img");
                var filePath = Path.Combine(FilePath, fileName);
                FileInfo file = new FileInfo(filePath);
                if (file.Exists)//check file exsit or not  
                {
                    file.Delete();
                }
            }
            Models.Case.RemoveImage(imageIdList);
        }


    }
}

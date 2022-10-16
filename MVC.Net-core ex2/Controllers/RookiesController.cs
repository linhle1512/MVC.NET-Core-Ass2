using MVC2.Models;

namespace MVC2.Controllers
{
    [Route("NashTech/Rookies")]
    public class RookiesController : Controller
    {
        private static List<MembersModel> listMember = new List<MembersModel>
        {
            new MembersModel
            {
                FirstName = "Dung",
                LastName = "Do",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 8, 21),
                BirthPlace = "Ha Noi",
                PhoneNumber = "24254535633",
                IsGraduated = false
            },

            new MembersModel
            {
                FirstName = "Long",
                LastName = "Le",
                Gender = "Male",
                DateOfBirth = new DateTime(1997, 9, 8),
                BirthPlace = "Hai Phong",
                PhoneNumber = "35646567",
                IsGraduated = true
            },

            new MembersModel
            {
                FirstName = "Linh",
                LastName = "Tai",
                Gender = "Male",
                DateOfBirth = new DateTime(2001, 12, 1),
                BirthPlace = "Ha Noi",
                PhoneNumber = "476470942",
                IsGraduated = false
            },

            new MembersModel
            {
                FirstName = "Quynh",
                LastName = "Do",
                Gender = "Female",
                DateOfBirth = new DateTime(2000, 9, 4),
                BirthPlace = "Ha Noi",
                PhoneNumber = "5324676879",
                IsGraduated = false
            }
        };

        private readonly ILogger<RookiesController> _logger;

        public RookiesController(ILogger<RookiesController> logger)
        {
            _logger = logger;
        }

        [Route("Index")]

        public IActionResult Index()
        {
            return Json(listMember);
        }

        #region #1

        [Route("GetMaleMembers")]

        public IActionResult GetMaleMembers()
        {
            var data = listMember.Where(p => p.Gender == "Male");
            return Json(data);
        }

        #endregion

        #region #2
        
        [Route("GetOldestMembers")]

        public IActionResult GetOldestMembers()
        {
            var maxAge = listMember.Max(p => p.Age);
            var oldest = listMember.FirstOrDefault(p => p.Age == maxAge);
            return Json(oldest);
        }

        #endregion

        #region #3

        [Route("GetFullName")]

        public IActionResult GetFullName()
        {
            var fullNames = listMember.Select(p => p.FullName);
            return Json(fullNames);
        }

        #endregion

        #region #4

        [Route("GetNembersByBirthYear")]

        public IActionResult GetNembersByBirthYear(int year, string compareType)
        {
            switch (compareType)
            {
                case "equals":
                    return Json(listMember.Where(p => p.DateOfBirth.Year == year));
                case "greaterthan":
                    return Json(listMember.Where(p => p.DateOfBirth.Year > year));
                case "lessthan":
                    return Json(listMember.Where(p => p.DateOfBirth.Year < year));
                default:
                    return Json(null);
            }
        }

        [Route("GetNembersByBirthYear/GetMembersWhoBornIn2000")]

        public IActionResult GetMembersWhoBornIn2000()
        {
            return RedirectToAction("GetNembersByBirthYear", new { year = 2000, compareType = "equals" });
        }

        [Route("GetNembersByBirthYear/GetMembersWhoBornAfter2000")]

        public IActionResult GetMembersWhoBornAfter2000()
        {
            return RedirectToAction("GetNembersByBirthYear", new { year = 2000, compareType = "greaterthan" });
        }

        [Route("GetNembersByBirthYear/GetMembersWhoBornBefore2000")]

        public IActionResult GetMembersWhoBornBefore2000()
        {
            return RedirectToAction("GetNembersByBirthYear", new { year = 2000, compareType = "lessthan" });
        }

        #endregion

        #region #5

        public byte[] WriteCsvToMemory(IEnumerable<MembersModel> people)
        {
            using (var memoryStream = new MemoryStream())
            using (var streamWriter = new StreamWriter(memoryStream))
            using (var csvWriter = new CsvWriter(streamWriter, System.Globalization.CultureInfo.CurrentCulture))
            {
                csvWriter.WriteRecords(people);
                streamWriter.Flush();
                return memoryStream.ToArray();
            }
        }

        public FileStreamResult Export()
        {
            var result = WriteCsvToMemory(listMember);
            var memoryStream = new MemoryStream(result);
            return new FileStreamResult(memoryStream, "text/csv") { FileDownloadName = "member.csv" };
        }

        #endregion
    }
}
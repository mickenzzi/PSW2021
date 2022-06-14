using IronPdf;
using PSW.DTO;
using PSW.Model;
using PSW.Repository.Interface;
using System;
using System.Collections.Generic;
using WinSCP;

namespace PSW.Service
{
    public class DoctorService
    {
        private readonly IUserRepository _userRepository;

        public DoctorService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public List<User> GetAllDoctors()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach (User u in users)
            {
                if (u.Role.Equals("Doctor"))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }

        public List<User> GetAllSpecialist()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach (User u in users)
            {
                if (u.Role.Equals("Doctor") && !u.Specialization.ToLower().Equals("GeneralPractitioner".ToLower()))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }
        public List<User> GetAllNonSpecialist()
        {
            List<User> users = _userRepository.GetAll();
            List<User> doctors = new List<User>();
            foreach (User u in users)
            {
                if (u.Role.Equals("Doctor") && u.Specialization.ToLower().Equals("GeneralPractitioner".ToLower()))
                {
                    doctors.Add(u);
                }
            }
            return doctors;
        }

        public void CreateRecipe(RecipeDTO recipeDTO)
        {
            string id = "recipes_" + Guid.NewGuid();
            DateTime now = DateTime.Now;
            string html = "<br><h1 style = \"text-align: center;\">Recept</h1><br>" +
                "<label style = \"font-size: medium;\">Doktor: " + recipeDTO.Doctor + "</label>" +
                "<label style = \"margin-left: 40%; font-size: medium; position:fixed\">Datum: " + now.ToString() + "</label><br><br><br>" +
                "<label style = \"font-size: large; margin-left: 45%;\">Upotreba: </label><br><br><label>Preporuka da pacijent koristi lek " + recipeDTO.Medicine + " u naznacenoj dozi " + recipeDTO.Dose + " mg." + "</label><br><br><br><br>" +
                "<label style = \"font-size: medium;\">Pacijent: " + recipeDTO.Patient + "</label>";
            ChromePdfRenderer render = new ChromePdfRenderer();
            PdfDocument pdf = render.RenderHtmlAsPdf(html);
            pdf.SaveAs(@"C:\Users\HP\Desktop\PSW\PSW2021\Backend\recipes\" + id + ".pdf");
            shareFiles(1);
        }

        private void shareFiles(int transferOption)
        {
            try
            {
                // Setup session options
                SessionOptions sessionOptions = new SessionOptions
                {
                    Protocol = Protocol.Sftp,
                    HostName = "192.168.1.3",
                    UserName = "tester",
                    Password = "password",
                    PortNumber = 2222,
                    SshHostKeyFingerprint = "ssh-rsa 2048 2tSJ/aFUHpMia3eUXFfCdMeov2c2Ry1k9PgaKVZ4M7k="
                };

                using (WinSCP.Session session = new WinSCP.Session())
                {
                    // Connect
                    session.Open(sessionOptions);

                    // Upload files
                    TransferOptions transferOptions = new TransferOptions
                    {
                        TransferMode = TransferMode.Binary
                    };

                    TransferOperationResult transferResult;
                    if (transferOption == 1)
                    {
                        transferResult = session.PutFiles(@"C:\Users\HP\Desktop\PSW\PSW2021\Backend\recipes\*.pdf", "/public/", false, transferOptions);
                    } else
                    {
                        transferResult = session.GetFiles(@"\public\*.pdf", @"C:\Users\HP\Downloads\RebexTinySftpServer-Binaries-Latest\local\*.pdf", false, transferOptions);
                    }
                    // Throw on any error
                    transferResult.Check();
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: {0}", e);
            }
        }
    }
}

using EventManagementWeb.ViewModels;
using EventPlannerData;
using System;
using System.Linq;
using System.Security.Cryptography;
using System.Web.Mvc;
using System.Web.Security;
using User = EventPlannerData.User;

namespace EventManagementWeb.Controllers
{
    public class UserController : Controller
    {
        private dbEventPlannerEntities db = new dbEventPlannerEntities();
        // GET: User
        // GET: /Account/Register
        [AllowAnonymous]
        public ActionResult Register()
        {
            return View();
        }

        // POST: /Account/Register
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Register(UserRegistration model)
        {
            if (ModelState.IsValid)
            {
                // Check if the email is already in use
                if (IsEmailTaken(model.Email))
                {
                    ModelState.AddModelError("Email", "Email is already registered.");
                    return View(model);
                }

                // Hash the password
                //string hashedPassword = Convert.ToString(HashPassword(model.Password));

                // Create a new user
                var user = new User
                {
                    FullName = model.FullName,
                    Email = model.Email,
                    PasswordHash = model.Password
                    // Set other properties as needed
                };

                // Add the user to the database
                db.Users.Add(user);
                db.SaveChanges();

                // You can optionally sign the user in after registration
                FormsAuthentication.SetAuthCookie(user.Email, false);

                // Redirect to a success page or other action
                return RedirectToAction("RegistrationSuccess");
            }

            // If the model is not valid, return to the registration form with validation errors
            return View(model);
        }
        [AllowAnonymous]
        public ActionResult RegistrationSuccess()
        {
            return View();
        }

        // Helper method to check if an email is already registered
        private bool IsEmailTaken(string email)
        {
            return db.Users.Any(u => u.Email == email);
        }

        // Helper method to hash the password (you should use a secure hashing algorithm)
        [AllowAnonymous]
        public ActionResult HashPassword(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                // Handle the case where the password is empty
                return Content("Password cannot be empty.");
            }

            try
            {
                // Generate a salt
                byte[] salt;
                new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                // Create a new Rfc2898DeriveBytes object to perform the hashing
                var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

                // Get the hashed password bytes
                byte[] hashBytes = pbkdf2.GetBytes(20);

                // Combine the salt and hashed password
                byte[] hashWithSaltBytes = new byte[36];
                Array.Copy(salt, 0, hashWithSaltBytes, 0, 16);
                Array.Copy(hashBytes, 0, hashWithSaltBytes, 16, 20);

                // Convert the byte array to a base64-encoded string
                string hashedPassword = Convert.ToBase64String(hashWithSaltBytes);

                return Content(hashedPassword);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during password hashing
                return Content("An error occurred: " + ex.Message);
            }
        }

        // Add other actions as needed, such as login and logout

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        // GET: /Account/Logout
        [AllowAnonymous]
        public ActionResult Logout()
        {
            // Sign the user out
            FormsAuthentication.SignOut();
            Session["UserID"] = 0;
            Session["UserName"] = "";
            // Redirect to the home page or any other desired page
            return RedirectToAction("Index", "Home");

        }

        #region Login

        // GET: /Account/Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        // POST: /Account/Login
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(string Email, string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                // Handle the case where the password is empty
                return Content("Password cannot be empty.");
            }

            try
            {
                //// Generate a salt
                //byte[] salt;
                //new RNGCryptoServiceProvider().GetBytes(salt = new byte[16]);

                //// Create a new Rfc2898DeriveBytes object to perform the hashing
                //var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000);

                //// Get the hashed password bytes
                //byte[] hashBytes = pbkdf2.GetBytes(20);

                //// Combine the salt and hashed password
                //byte[] hashWithSaltBytes = new byte[36];
                //Array.Copy(salt, 0, hashWithSaltBytes, 0, 16);
                //Array.Copy(hashBytes, 0, hashWithSaltBytes, 16, 20);

                //// Convert the byte array to a base64-encoded string
                //string hashedPassword = Convert.ToBase64String(hashWithSaltBytes);
                var User = (from tb in db.Users where tb.Email == Email && tb.PasswordHash == password select tb).FirstOrDefault();
                if (User != null)
                {
                    if (Convert.ToInt32(User.UserId) > 0)
                    {
                        Session["UserID"] = User.UserId.ToString();
                        Session["UserName"] = User.FullName.ToString();
                        return RedirectToAction("Index", "Home", new { UserId = User.UserId });
                    }
                    else
                    {
                        return View();
                    }
                }
                else
                {
                    ViewBag.message = "Invalid Login Credentials!!";
                    return View();
                }
                //return Content(password);
            }
            catch (Exception ex)
            {
                // Handle any exceptions that may occur during password hashing
                return Content("An error occurred: " + ex.Message);
            }
        }


        #endregion


    }
}
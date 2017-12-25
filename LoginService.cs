using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Xmu.Crms.Shared.Models;
using Xmu.Crms.Shared.Exceptions;
using Xmu.Crms.Shared.Service;
using System.Numerics;
using System.Text;
using System.Security;

using System.Security.Cryptography;


namespace Xmu.Crms.Services.HighGrade
{

    public class LoginService : ILoginService
    {

        private readonly CrmsContext _db;

        public LoginService(CrmsContext db)
        {
            _db = db;
        }

        public string GetMd5(string strPwd)
        {
            using (MD5 md5 = MD5.Create())
            {
                byte[] byteHash = md5.ComputeHash(System.Text.Encoding.Default.GetBytes(strPwd));
                string strRes = BitConverter.ToString(byteHash).Replace("-", "");
                return strRes.ToUpper();
            }
        }

        public UserInfo SignInWeChat(long userId, string code, string state, string successUrl)
        {
            var us = new UserInfo();
            return us;
        }


        public UserInfo SignInPhone(UserInfo user)
        {
            //	MD5 md5I = new MD5CryptoServiceProvider();

            var us = _db.UserInfo.SingleOrDefault(u => u.Phone == user.Phone);
            if (us == null)
            {
                throw new UserNotFoundException();
            }
            /*  byte[] byteArray1= System.Text.Encoding.Default.GetBytes(user.Password);
              byte[] byteArray2 = System.Text.Encoding.Default.GetBytes(us.Password); 

             if (md5I.ComputeHash(byteArray1) !=byteArray2)
              {
                  throw new PasswordErrorException();
              }       */
            if (GetMd5(user.Password) != us.Password)
            {
                throw new PasswordErrorException();
            }



            return us;
        }


        public UserInfo SignUpPhone(UserInfo user)
        {
            // MD5 md5 = new MD5CryptoServiceProvider();
            //byte[] byteArray1= System.Text.Encoding.Default.GetBytes(user.Password);


            var u = new UserInfo
            {
                Phone = user.Phone,
                Password = GetMd5(user.Password),
            };


            //Password= md5.ComputeHash(user.Password)


            _db.UserInfo.Add(u);
            _db.SaveChanges();
            return u;
        }

        public void DeleteTeacherAccount(long userId)
        {
            var teacher = _db.UserInfo.SingleOrDefault(u => u.Id == userId);

            if (teacher == null)
                throw new UserNotFoundException();

            teacher.Phone = null;
            _db.SaveChanges();
        }

        public void DeleteStudentAccount(long userId)
        {
            var student = _db.UserInfo.SingleOrDefault(u => u.Id == userId);

            if (student == null)
                throw new UserNotFoundException();

            student.Phone = null;
            _db.SaveChanges();
        }

    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ClinicAutomationProject.GenericCode
{
    public class MethodToGetRandomOrderNumber
    {
        public static string GenerateOrderNumber()
        {
            string alphabets = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            //string small_alphabets = "abcdefghijklmnopqrstuvwxyz";
            string numbers = "1234567890";

            string characters = numbers;
            characters += alphabets + numbers;

            int length = 10;

            string otp = string.Empty;

            for (int i = 0; i < length; i++)
            {
                string character = string.Empty;
                do
                {
                    int index = new Random().Next(0, characters.Length);
                    character = characters.ToCharArray()[index].ToString();
                } while (otp.IndexOf(character) != -1);

                otp += character;
            }
            return otp;
        }

    }
}
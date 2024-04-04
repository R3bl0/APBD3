using System;

namespace LegacyApp
{
    public class UserService
    {
        public bool AddUser(string firstName, string lastName, string email, DateTime dateOfBirth, int clientId)
        {
            if (!IsFirstNameCorrect(firstName) || !IsLastNameCorrect(lastName))
            {
                return false;
            }

            if (!IsEmailCorrect(email))
            {
                return false;
            }

            var age = CalculateAgeUsingBirthDate(dateOfBirth);

            if (!IsAgeCorrect(age))
            {
                return false;
            }

            var clientRepository = new ClientRepository();
            var client = clientRepository.GetById(clientId);
            
            var user = NewUser(firstName, lastName, email, dateOfBirth, client);

            SetUserCreditLimit(client, user);

            if (!IsCreditLimitCorrect(user))
            {
                return false;
            }

            UserDataAccess.AddUser(user);
            return true;
        }

        static private void SetUserCreditLimit(Client client, User user)
        {
            if (client.Type == "VeryImportantClient")
            {
                user.HasCreditLimit = false;
            }
            else if (client.Type == "ImportantClient")
            {
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    creditLimit = creditLimit * 2;
                    user.CreditLimit = creditLimit;
                }
            }
            else
            {
                user.HasCreditLimit = true;
                using (var userCreditService = new UserCreditService())
                {
                    int creditLimit = userCreditService.GetCreditLimit(user.LastName, user.DateOfBirth);
                    user.CreditLimit = creditLimit;
                }
            }
        }

        static private User NewUser(string firstName, string lastName, string email, DateTime dateOfBirth, Client client)
        {
            var user = new User
            {
                Client = client,
                DateOfBirth = dateOfBirth,
                EmailAddress = email,
                FirstName = firstName,
                LastName = lastName
            };

            return user;
        }

        static private bool IsAgeCorrect(int age)
        {
            return age >= 21;
        }

        static private bool IsCreditLimitCorrect(User user)
        {
            return !user.HasCreditLimit && user.CreditLimit >= 500;
        }

        static private int CalculateAgeUsingBirthDate(DateTime dateOfBirth)
        {
            var now = DateTime.Now;
            int age = now.Year - dateOfBirth.Year;
            if (now.Month < dateOfBirth.Month || (now.Month == dateOfBirth.Month && now.Day < dateOfBirth.Day)) age--;
            return age;
        }

        static private bool IsEmailCorrect(string email)
        {
            return email.Contains("@") && email.Contains(".");
        }

        static private bool IsLastNameCorrect(string lastName)
        {
            return !string.IsNullOrEmpty(lastName);
        }

        static private bool IsFirstNameCorrect(string firstName)
        {
            return !string.IsNullOrEmpty(firstName);
        }
    }
}

using JetBrains.Annotations;
using LegacyApp;
using System;
using Xunit;

namespace LegacyApp.Tests
{
    [TestSubject(typeof(UserService))]
    public class UserServiceTest
    {

        [Fact]
        public void AddUser_Should_Return_False_When_FirstName_Is_Missing()
        {
            // Arrange
            var userService = new UserService();
            // Act
            var addResult = userService.AddUser("", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
            // Assert
            Assert.False(addResult);
        }

        [Fact]
        public void AddUser_Should_Return_False_When_LastName_Is_Missing()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("Joe", "", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
            Assert.False(addResult);
        }

        [Fact]
        public void AddUser_Should_Return_False_When_Age_Is_Under_21()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("Joe", "Doe", "johndoe@gmail.com", DateTime.Parse("2020-03-21"), 1);
            Assert.False(addResult);
        }
        
        [Fact]
        public void AddUser_Should_Return_False_When_Age_Is_21_But_Birthday_Hasnt_Passed_This_Year_Yet()
        {
            var userService = new UserService();
            var dateOfBirth = DateTime.Now.AddYears(-20).AddDays(5); 
            var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", dateOfBirth, 1);
            Assert.False(addResult); 
        }

        [Fact]
        public void AddUser_Should_Return_False_When_Email_Is_Incorrect()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("Joe", "Doe", "johndoegmailcom", DateTime.Parse("1982-03-21"), 1);
            Assert.False(addResult);
        }
        
        [Fact]
        public void AddUser_Should_Throw_ArgumentException_When_ClientId_Does_Not_Exist()
        {
            var userService = new UserService();
            Assert.Throws<ArgumentException>(() =>
            {
                userService.AddUser("Joe", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 0);
            });
        }
        
        [Fact]
        public void AddUser_Should_Throw_ArgumentException_When_LastName_Does_Not_Exist()
        {
            var userService = new UserService();
            Assert.Throws<ArgumentException>(() =>
            {
                userService.AddUser("Joe", "Boe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 0);
            });
        }
        
        [Fact]
        public void AddUser_Should_Return_False_When_CreditLimit_Is_Under_Than_500()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("Joe", "Kowalski", "kowalski@wp.pl", DateTime.Parse("1982-03-21"), 1);
            Assert.False(addResult);
        }

        [Fact]
        public void AddUser_Should_Return_True_When_ClientType_Is_VeryImportantClient()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("Tom", "Malewski", "malewski@gmail.pl", new DateTime(1982, 3, 21), 2);
            Assert.True(addResult);
        }
        
        [Fact]
        public void AddUser_Should_Return_True_When_Client_Is_ImportantClient()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("John", "Doe", "doe@gmail.com", new DateTime(1982, 3, 21), 4);
            Assert.True(addResult);
        }
        
        [Fact]
        public void AddUser_Should_Return_True_When_User_Is_NormalClient()
        {
            var userService = new UserService();
            var addResult = userService.AddUser("John", "Doe", "johndoe@gmail.com", DateTime.Parse("1982-03-21"), 1);
            Assert.True(addResult);
            
        }
    }
}
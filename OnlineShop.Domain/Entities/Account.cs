﻿using System.ComponentModel.DataAnnotations;

namespace OnlineShop.Domain.Entities
{
    public class Account : IEntity
    {
        private Guid _id;
        private string? _name;
        private string? _email;
        private string? _hashedPassword;

        public Account()
        {
        }

        public Account(string name, string email, string hashedPassword)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException($"\"{nameof(name)}\" не может быть неопределенным или пустым.", nameof(name));
            }

            if (string.IsNullOrEmpty(email))
            {
                throw new ArgumentException($"\"{nameof(email)}\" не может быть неопределенным или пустым.", nameof(email));
            }

            if (string.IsNullOrEmpty(hashedPassword))
            {
                throw new ArgumentException($"\"{nameof(hashedPassword)}\" не может быть неопределенным или пустым.", nameof(hashedPassword));
            }

            if(!new EmailAddressAttribute().IsValid(email))
            {
                throw new ArgumentException("Значение не валидно",nameof(email));
            }

            Id = Guid.NewGuid();
            _name = name;
            _email = email;
            _hashedPassword = hashedPassword;
              
        }

        public Guid Id
        {
            get => _id;
            set => _id = value;
        }

        public string? Name 
        {
            get => _name;
            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Значение не может быть пустым или содержать null", nameof(value));
                _name = value;
            }
                
        }
        public string? Email
        { 
            get => _email;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                {
                    throw new ArgumentException("Значение не может быть пустым или содержать null", nameof(value));
                }
                    
                if(!new EmailAddressAttribute().IsValid(value))
                {
                    throw new ArgumentException("Не валидное значение Email", nameof(value));
                }
                _email = value;
            }
        }
        public string? Password
        { 
            get => _hashedPassword;

            set
            {
                if (string.IsNullOrWhiteSpace(value))
                    throw new ArgumentException("Не валидное значение Email" + nameof(value));
                _hashedPassword = value;
            }
        }

        //private bool IsValidPassword(string password)
        //{
        //    if(string.IsNullOrWhiteSpace(password) || password.Length < 8)
        //    { 
        //        return false;
        //    }

        //    string pattern = @"^(?=.*[a-z])(?=.*[A-Z]).{8,}$";

        //    if(!Regex.IsMatch(password, pattern))
        //    {
        //        return false;
        //    }
        //    return true;
        //}
    }
}

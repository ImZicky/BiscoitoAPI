using System;
using System.Collections.Generic;
using System.Text;

namespace Service
{
    public interface IUserService
    {
        bool UnitTest(bool value);
    }

    public class UserService : IUserService
    {
        public bool UnitTest(bool value)
        {
            return !value;
        }
    }
}

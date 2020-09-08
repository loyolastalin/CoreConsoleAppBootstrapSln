using System;
using System.Collections.Generic;
using System.Text;

namespace ServiceCollectionCoreApp.Model
{
    public class User
    {
        public string Name { get; set; }

        public string Sex { get; set; }
    }

    public class UserDto
    {
        public string Name { get; set; }

        public string Sex { get; set; }

        public int Age { get; set; }

    }
}

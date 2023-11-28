using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services.DTO
{
    public class ProfileData
    {
        public int Id_Profile { get; set; }
        public string Username { get; set; }
        public int ProfileImage { get; set; }
        public int Victories { get; set; }
        public int Losses { get; set; }
        public int Id_Account { get; set; }
    }
}

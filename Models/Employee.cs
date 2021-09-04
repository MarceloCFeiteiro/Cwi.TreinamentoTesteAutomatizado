using System;

namespace Cwi.TreinamentoTesteAutomatizado.Models
{
    public class Employee
    {
        public string Name { get; set; }

        public string Email { get; set; }

        public bool Active { get; set; }

        public override bool Equals(object obj)
        {
            return obj is Employee employee &&
                   Name == employee.Name &&
                   Email == employee.Email &&
                   Active == employee.Active;
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(Name, Email, Active);
        }

        public override string ToString()
        {
            return $"Name = {Name}, Email = {Email}, Active = {Active}";
        }
    }
}

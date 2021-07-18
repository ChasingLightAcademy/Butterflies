namespace Progynova.Enums
{
    public enum UserPermission
    {
        Unactivated = 0b1, 
        Student = 0b1 << 1,
        Teacher = 0b1 << 2,
        Admin = 0b1 << 3,
    }
}
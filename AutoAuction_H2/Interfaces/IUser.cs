namespace AutoAuction_H2.Interfaces;

public interface IUser
{
    int ID { get; set; }
    string UserName { get; set; }
    string PasswordHash { get; set; }
    decimal Saldo { get; set; }
    int ZipCode { get; set; }
}

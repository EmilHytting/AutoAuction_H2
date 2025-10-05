namespace AutoAuction_H2.Models.Interfaces;
public interface IValidatable
{
    bool ValidateUserName(string userName);
    bool ValidatePassword(string password);
}

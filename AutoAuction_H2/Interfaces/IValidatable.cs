namespace AutoAuction_H2.Interfaces;

public interface IValidatable
{
    bool ValidateUserName(string userName);
    bool ValidatePassword(string password);
}

namespace AutoAuction_H2.Models.Interfaces;
public interface ISeller
{
    // Method to notify seller about a bid on a vehicle
    void GetNotificationAboutBid(object vehicle, decimal bid);
}

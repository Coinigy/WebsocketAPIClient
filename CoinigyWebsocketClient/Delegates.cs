using CoinigyWebsocketClient.Models;
using CoinigyWebsocketClient.Types;

namespace CoinigyWebsocketClient
{
    public delegate void ClientIsReady();
    public delegate void Message(MessageType messageType, string data);
    public delegate void TradeMessage(string exchange, string curr1, string curr2, TradeItem trade);
    public delegate void OrderMessage(string exchange, string curr1, string curr2, OrderItem[] orders);
    public delegate void BlockMessage(string curr, BlockItem block);
    public delegate void FavoriteMessage(FavoriteDataItem[] favorites);
    public delegate void NewMarketMessage(NewMarketDataItem markets);
    public delegate void NotificationMessage(NotificationDataItem notification);
    public delegate void NewsMessage(NewsDataItem news);
}

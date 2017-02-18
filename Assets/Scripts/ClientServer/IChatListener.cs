using UnityEngine;
using System.Collections;

interface IChatListener {
    void onDisConnect();
    void OnLogin(Message message);
    void OnMessageServer(string message);
    void OnPopupNotify(Message message);
    void OnProfile(Message message);
    void OnRateScratchCard(Message message);
    void OnListBetMoney(Message message);
    void OnListProduct(Message message);
    void OnTop(Message message);
    void OnInboxMessage(Message message);
    void OnGetAlertLink(Message message);
    void OnInfoSMS(Message message);
    void OnSMS9029(Message message);

    void OnJoinGame(Message message);
    void OnJoinRoom(Message message);
    void OnGameID(Message message);
    void OnListTable(int totalTB, Message message);

    void OnJoinTablePlay(Message message);
}
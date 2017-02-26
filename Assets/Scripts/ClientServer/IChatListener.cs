using UnityEngine;
using System.Collections;

interface IChatListener {
    void onDisConnect();
    void OnLogin(Message message);
    void OnRegister(Message message);
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
    void OnInvite(Message message);
    void OnJoinGame(Message message);
    void OnJoinRoom(Message message);
    void OnGameID(Message message);
    void OnListTable(int totalTB, Message message);
    void OnUpdateRoom(Message message);

    void OnJoinTablePlay(Message message);
    void OnUserExitTable(Message message);
    void InfoCardPlayerInTbl(Message message);
    void OnReady(Message message);
    void OnStartFail(string info);
    void OnStartSuccess(Message message);
    void OnStartForView(Message message);
    void OnSetNewMaster(string nick);
    void OnNickSkip(string nick, string turnName);
    void OnNickSkip(string nick, Message msg);
    void OnFrieCard(Message message);
    void OnFinishGame(Message message);
    void OnAllCardPlayerFinish(Message message);
    void OnFinishTurnTLMN(Message message);
    void OnSetTurn(Message message);
    void OnBaoSam(Message message);
}
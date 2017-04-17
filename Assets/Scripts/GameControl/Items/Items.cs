using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataBase {
    #region ItemTableData
	public struct ItemTableData {
		public int Id{ get; set; }
		public int Status{ get; set; }
		public string TableName{ get; set; }
		public string MasId{ get; set; }
		public int NUser{ get; set; }
		public int MaxUser{ get; set; }
		public long Money{ get; set; }
		public long NeedMoney{ get; set; }
		public long MaxMoney{ get; set; }
		public int IsLock{ get; set; }
    }
    #endregion
    #region PlayerData
	public struct PlayerData {
        public string Name { get; set; }
        public string DisplaName { get; set; }
        public long Money { get; set; }
        public long FolowMoney { get; set; }
        public bool IsMaster { get; set; }
        public bool IsReady { get; set; }
        public int Avata_Id { get; set; }
        public string Avata_Link { get; set; }
        public int SitOnSever { get; set; }
        public int SitOnClient { get; set; }
    }
    #endregion
    #region ItemRankData
	public struct ItemRankData {
		public int Rank{ get; set; }
		public string Name { get; set; }
		public int Avata_Id { get; set; }
		public long Money { get; set; }
    }
    #endregion
    #region ItemNotiData
	public struct ItemNotiData {
        public int Id;
		public string Title { get; set; }
		public string Content { get; set; }
    }
    #endregion
    #region ItemInviteData
	public struct ItemInviteData {
		public string Name { get; set; }
		public string Dispayname { get; set; }
		public long Money{ get; set; }
    }
	#endregion
	#region ItemRateCardData
	public struct ItemRateCardData{
		public int Card_Cost{ get; set; }
		public int Card_Value{ get; set; }
	}
	#endregion
	#region ItemHistoryTranferData
	public struct ItemHistoryTranferData{
		public long Id{ get; set; }
		public long Money{ get; set; }
		public string TimeTranfer{ get; set; }
	}
	#endregion

	#region ItemInfoGiftData
	public struct ItemInfoGiftData{
		public int Id{ get; set; }
		/// <summary>
		/// type 1: the cao
		/// type 2: vat pham
		/// </summary>
		public int Type{ get; set; }
		public string Telco{ get; set; }
		public string Name{ get; set; }
		public long Cost{ get; set; }
		public long Price{ get; set; }
		public long Balance{ get; set; }
		public string Des{ get; set; }
		public string Links{ get; set; }
//		gift.id = message.reader().readInt();
//		gift.type = message.reader().readInt();
//		// type 1: the cao
//		// type 2: vat pham
//		gift.name = message.reader().readUTF();
//		gift.cost = message.reader().readLong();
//		gift.telco = message.reader().readUTF();
//		gift.price = message.reader().readLong();
//		gift.balance = message.reader().readLong();
//		gift.des = message.reader().readUTF();
//		gift.links = message.reader().readUTF();
	}
	#endregion
}

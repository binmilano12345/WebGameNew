using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataBase {
    #region ItemTableData
    public class ItemTableData {
        public int Id;
        public int Status;
        public string TableName = "";
        public string MasId = "";
        public int NUser;
        public int MaxUser;
        public long Money;
        public long NeedMoney;
        public long MaxMoney;
        public int IsLock = 0;
    }
    #endregion
    #region PlayerData
    public class PlayerData {
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
    public class ItemRankData {
        public int Rank = 1;
        public string Name = "";
        public int Avata_Id = 0;
        public long Money = 0;
    }
    #endregion
    #region ItemRankData
    public class ItemNotiData {
        public int Id;
        public string Title = "";
        public string Content = "";
    }
    #endregion
}

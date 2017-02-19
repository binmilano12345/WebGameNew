using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace DataBase {
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
}

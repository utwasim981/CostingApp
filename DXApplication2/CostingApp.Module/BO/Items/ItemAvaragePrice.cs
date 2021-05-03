using CostingApp.Module.BO.Masters;
using CostingApp.Module.CommonLibrary.General.Model;
using DevExpress.ExpressApp.Model;
using DevExpress.Xpo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CostingApp.Module.BO.Items {
    //public class ItemAvaragePrice : WXafBaseObject {
    //    ItemCard fItemCard;
    //    [Association("ItemCard-ItemAvaragePrice")]
    //    [ModelDefault("AllowEdit", "False")]
    //    public ItemCard ItemCard {
    //        get { return fItemCard; }
    //        set { SetPropertyValue<ItemCard>(nameof(ItemCard), ref fItemCard, value); }
    //    }
    //    Shop fShop;
    //    [ModelDefault("AllowEdit", "False")]
    //    public Shop Shop {
    //        get { return fShop; }
    //        set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
    //    }
    //    DateTime fTransactionDate;
    //    public DateTime TransactionDate {
    //        get { return fTransactionDate; }
    //        set { SetPropertyValue<DateTime>(nameof(TransactionDate), ref fTransactionDate, value); }
    //    }
    //    double fAvaragePrice;
    //    public double AvaragePrice {
    //        get { return fAvaragePrice; }
    //        set { SetPropertyValue<double>(nameof(AvaragePrice), ref fAvaragePrice, value); }
    //    }
    //    [PersistentAlias("AvaragePrice / Item.StockUnit.ConversionRate")]
    //    public double StockAvaragePrice {
    //        get { return Convert.ToDouble(EvaluateAlias(nameof(StockAvaragePrice))); }
    //    }
    //    public ItemAvaragePrice(Session session) : base(session) { }
    //}
}

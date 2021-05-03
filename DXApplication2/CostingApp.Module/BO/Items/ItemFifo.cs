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
    //public class ItemFifo : WXafBaseObject {
    //    ItemCard fItemCard;
    //    [Association("ItemCard-ItemFifo")]
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
    //    double fQuanityIn;
    //    public double QuanityIn {
    //        get { return fQuanityIn; }
    //        set { SetPropertyValue<double>(nameof(QuanityIn), ref fQuanityIn, value); }
    //    }
    //    [PersistentAlias("QuantitIn / Item.StockUnit.ConversionRate")]
    //    public double StockQuanityIn {
    //        get { return Convert.ToDouble(EvaluateAlias(nameof(StockQuanityIn))); }
    //    }
    //    double fPrice;
    //    public double Price {
    //        get { return fPrice; }
    //        set { SetPropertyValue<double>(nameof(Price), ref fPrice, value); }
    //    }
    //    [PersistentAlias("Price / Item.StockUnit.ConversionRate")]
    //    public double StockPrice {
    //        get { return Convert.ToDouble(EvaluateAlias(nameof(StockPrice))); }
    //    }
    //    double fQuantityOut;
    //    public double QuantityOut {
    //        get { return fQuantityOut; }
    //        set { SetPropertyValue<double>(nameof(QuantityOut), ref fQuantityOut, value); }
    //    }
    //    [PersistentAlias("QuantitOut / Item.StockUnit.ConversionRate")]
    //    public double StockQuanityOut {
    //        get { return Convert.ToDouble(EvaluateAlias(nameof(StockQuanityOut))); }
    //    }
    //    public ItemFifo(Session session) : base(session) { }
    //}
}

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
    public class ItemQuantity: WXafBaseObject {
        ItemCard fItemCard;
        [Association("ItemCard-ItemQuantity")]
        [ModelDefault("AllowEdit", "False")]
        public ItemCard ItemCard {
            get { return fItemCard; }
            set { SetPropertyValue<ItemCard>(nameof(ItemCard), ref fItemCard, value); }
        }
        Shop fShop;
        [ModelDefault("AllowEdit", "False")]
        public Shop Shop {
            get { return fShop; }
            set { SetPropertyValue<Shop>(nameof(Shop), ref fShop, value); }
        }
        DateTime fLastPurchaseDate;
        [ModelDefault("AllowEdit", "False")]
        public DateTime LastPurchaseDate {
            get { return fLastPurchaseDate; }
            set { SetPropertyValue<DateTime>(nameof(LastPurchaseDate), ref fLastPurchaseDate, value); }
        }
        double fLastPurchasePrice;
        [ModelDefault("AllowEdit", "False")]
        public double LastPurchasePrice {
            get { return fLastPurchasePrice; }
            set { SetPropertyValue<double>(nameof(LastPurchasePrice), ref fLastPurchasePrice, value); }
        }
        double fQuantityOnHand;
        [ModelDefault("AllowEdit", "False")]
        public double QuantityOnHand {
            get { return fQuantityOnHand; }
            set { SetPropertyValue<double>(nameof(QuantityOnHand), ref fQuantityOnHand, value); }
        }
        public ItemQuantity(Session session) : base(session) { }
    }
}

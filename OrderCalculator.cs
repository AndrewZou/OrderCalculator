using System;
using System.Collections.Generic;

namespace OrderCalculator
{
    public class OrderCalculator
    {  
        public Order CustomerOrder { set; get; }
        //5 clients that will use this application. 1 from GA, 1 from FL, 1 from NY, 1 from NM, and 1 from NV.
        public readonly List<string> statesSpecial = new List<string>() { "FL", "NM", "NV" };

        public void calculateOrderPayment()
        {
            double totalCharge = 0.00;
            try
            {
                double couponDiscountAmount = 0.00;
                double promotionDiscountAmount = 0.00;
                double taxCharge = 0.00;

                foreach( Product product in CustomerOrder.productsBasket)
                {
                    if( product.BulckPrice != null && product.BulckPrice > 0 )
                    {
                        totalCharge += (double)product.BulckPrice;
                    }
                    else if( product.UnitPrice != null && product.UnitPrice > 0)
                    {
                        totalCharge += (double)product.UnitPrice;
                    }

                    couponDiscountAmount = GetCouponDiscount(product.Code);
                    promotionDiscountAmount = GetPromotionDiscount(product.Code, (double)product.UnitPrice);

                    if( statesSpecial.Contains(CustomerOrder.State))
                    {
                        //Calculate tax liability
                        taxCharge += totalCharge * product.GetTaxRate();
                    }
                    else
                    {
                        taxCharge = (totalCharge - (couponDiscountAmount > promotionDiscountAmount ? couponDiscountAmount : promotionDiscountAmount)) * product.GetTaxRate();
                    }
                    //Apply Discount or Promotion
                    totalCharge -= (couponDiscountAmount > promotionDiscountAmount ? couponDiscountAmount : promotionDiscountAmount);
                    //In case anything could be wrong here
                    if (totalCharge < 0.00) totalCharge = 0.00;

                }
                this.CustomerOrder.TaxCharge = taxCharge;
                this.CustomerOrder.TotalCost = totalCharge;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }



        public double GetCouponDiscount( string productCode )
        {
            double couponDiscount = 0.00;
            foreach(Coupon coupon in this.CustomerOrder.appliedCoupons)
            {
                if ( coupon.ExpirationDate < DateTime.Now || coupon.ProductCode != productCode || coupon.Applied ) continue; //Expired Coupon
                
                if( coupon.DeductAmount != null && coupon.DeductAmount > 0.00)
                {
                    couponDiscount = (double)coupon.DeductAmount;
                    break;
                }

                coupon.Applied = true;
            }
            return couponDiscount;
        }

        public double GetPromotionDiscount(string productCode, double productPrice)
        {
            double promotionDiscount = 0.00;
            foreach (Promotion promotion in this.CustomerOrder.appliedPromotions)
            {
                if (promotion.ExpirationDate < DateTime.Now || promotion.ProductCode != productCode || promotion.Applied)
                {
                    continue; //Expired Promotion, Already Applied, or not matched
                }
                if (promotion.DeductRate != null && promotion.DeductRate > 0.00)
                {
                    promotionDiscount = (double)(productPrice * promotion.DeductRate);
                    break;
                }
                promotion.Applied = true;
            }
            return promotionDiscount;
        }
    }

    public enum Type { Luxury, Normal };
    
    public class Product
        {
            //Product code
            public string Code { get; set; }

            //Enumerator
            public Type Type { get; set; }
            public double? UnitPrice { get; set; }
            public double? BulckPrice { get; set; }
            public string Name { get; set; }

        private double TaxRate { set; get; } = 0.08;
            public double GetTaxRate()
            {
                double appliedTaxRate = TaxRate;
                if ( Type == Type.Luxury)
                {
                    appliedTaxRate *= 2;
                }
                return appliedTaxRate;
            }
        }
    
    public class Order
    {
        public string CustomerId { get; set; }
        public string State { set; get;  }
        public List<Product> productsBasket;
        public double TotalCost;
        public double TaxCharge;
        public List<Promotion> appliedPromotions;
        public List<Coupon> appliedCoupons;

        public void AddProduct( Product product)
        {
            this.productsBasket.Add(product);
        }

        public void AddCoupon( Coupon coupon)
        {
            if( !this.appliedCoupons.Contains( coupon ))
                this.appliedCoupons.Add(coupon);
        }

        public void AddPromotion( Promotion promotion)
        {
            if(!this.appliedPromotions.Contains(promotion))
            {
                this.appliedPromotions.Add(promotion);
            }
        }
    }

    public class Coupon
    {
        //Applied product
        public string ProductCode { get; set; }
        //Promotion code
        public string Code { get; set; }

        public double? DeductAmount { get; set; }
        public DateTime ExpirationDate { get; set; }
        public bool Applied { set; get; } = false;
    }

    public class Promotion
    {
        //Applied product
        public string ProductCode { get; set; }
        //Promotion code
        public string Code { get; set; }
       
        public double? DeductRate { get; set; }

        public DateTime ExpirationDate { get; set; }

        public bool Applied { set; get; } = false;
    }
}

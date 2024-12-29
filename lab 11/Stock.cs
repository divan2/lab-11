using System;

namespace StockServer
{
    public class Stock
    {
        public int Id { get; set; }
        public string Ticker { get; set; }
        public double Price { get; set; }  
        public DateTime Date { get; set; }
    }
}

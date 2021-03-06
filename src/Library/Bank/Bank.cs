using System.Collections.Generic;
using System.Text;

namespace Bankbot
{
    /*La clase Bank consta de un Singleton para no generar mas de una instancia del mismo ya que solo queremos
    almacenar los objetos Currency creados en una lista global.
    Dicha clase también cumple con el principio OCP ya que se encuentra abierta a la extensión y cerrada a la modificación,
    como también con el patrón Expert y Creator de los principios GRASP, esto se debe a que esta clase es experta en información
    relacionada con el objeto Currency, por lo que es la que se encarga de crear instancias del mismo y almacenarlas.
    A su vez es la encargada de realizar las conversiones monetarias requeridas entre sus elementos.*/

    /// <summary>
    /// Se encarga de realizar conversiones entre tipos de divisas.
    /// </summary>
    public class Bank
    {
        public List<Currency> CurrencyList { get; set; }
        private static Bank instance;
        public static Bank Instance
        {
            get
            {
                if (instance == null) instance = new Bank();
                return instance;
            }
        }

        private Bank()
        {
            this.CurrencyList = new List<Currency>() { new Currency("UYU", "U$", 0.023), new Currency("USS", "US$", 1), new Currency("ARG", "AR$", 0.012) };
        }
      
        /// <summary>
        /// Se agrega una moneda con su códigoISO en caso que le solicitemos.
        /// </summary>
        /// <param name="codeISO"></param>
        /// <param name="symbol"></param>

        public void AddCurrency(string codeISO, string symbol, double rate)
        {
            if (!CurrencyExists(codeISO, symbol))
            {
                Currency newCurrency = new Currency(codeISO, symbol, rate);
                CurrencyList.Add(newCurrency);
            }
        }
        /// <summary>
        /// Remueve una divisa(tipo de moneda).
        /// </summary>
        /// <param name="codeISO"></param>
        /// <param name="symbol"></param>
        public void RemoveCurrency(string codeISO, string symbol)
        {
            foreach (var currency in CurrencyList)
            {
                if (currency.CodeISO == codeISO && currency.Symbol == symbol)
                {
                    CurrencyList.Remove(currency);
                    return;
                }
            }
        }

        /// <summary>
        /// Realiza la conversión entre divisas.
        /// </summary>
        /// <param name="amount"></param>
        /// <param name="from"></param>
        /// <param name="to"></param>
        /// <returns></returns>
        public double Convert(double amount, Currency from, Currency to)
        {
            if (from.CodeISO != "USS")
            {
                amount = amount * from.ConvertionRate;
            }

            return amount / to.ConvertionRate;
        }

        public bool CurrencyExists(string iso, string symbol)
        {
            foreach (var item in CurrencyList)
            {
                if (item.CodeISO == iso && item.Symbol == symbol) return true;
            }
            return false;
        }

        /// <summary>
        /// Muestra la lista de divisas.
        /// </summary>
        /// <returns></returns>
        public string ShowCurrencyList()
        {
            StringBuilder currencies = new StringBuilder();
            foreach (Currency currency in Bank.Instance.CurrencyList)
            {
                currencies.Append($"{Bank.Instance.CurrencyList.IndexOf(currency) + 1} - {currency.CodeISO}\n");
            }
            return currencies.ToString();
        }
    }
}

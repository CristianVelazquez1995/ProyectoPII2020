using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;

namespace Bankbot
{
    /// <summary>
    /// Esta clase cumple con los principios GRAPS, ya que es experta en información sobre los usuarios, se encarga de 
    /// crear instancias de la clase Account para luego almacenarlos. Por esta razón cumple con los patrones Expert
    /// y Creator dentro de estos principios.
    /// Por otro lado cumple con el patrón OCP al ser una clase abierta a la extensión y cerrada a la modificación.
    /// </summary>
    public class User
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        private Guid Id { get; set; }
        public List<Account> Accounts { get; set; }
        public List<String> IncomeList { get; set; }
        public List<String> OutcomeList { get; set; }
        public static User Empty { get; internal set; }

        public User(string userName, string password)
        {
            this.UserName = userName;
            this.Password = Cypher(password);
            this.Id = Guid.NewGuid();
            this.Accounts = new List<Account> { };
            this.IncomeList = new List<String> { "Salario", "Regalo" };
            this.OutcomeList = new List<String> { "Comida", "Transporte", "Ocio", "Alquiler", "Impuestos" };
        }
        public User(string[] user)
        {
            this.UserName = user[0];
            this.Password = user[1];
            this.Id = Guid.Parse(user[2]);
            this.Accounts = new List<Account> { };
            this.IncomeList = new List<String> { "Salario", "Regalo" };
            this.OutcomeList = new List<String> { "Comida", "Transporte", "Ocio", "Alquiler", "Impuestos" };
        }

        /// <summary>
        /// Agregar un objeto Account a la la lista List<Account>
        /// </summary>
        /// <param name="account"></param>
        public void AddAccount(string name, AccountType? type, Currency currency, double amount, double objective)
        {
            if (this.Accounts == null)
            {
                this.Accounts = new List<Account> { };
            }
            foreach (var account in Accounts)
            {
                if (account.Name == name)
                {
                    System.Console.WriteLine("Ya existe una cuenta con este nombre.");
                    return;
                }
            }
            var newAccount = new Account(name, type, currency, amount, objective);
            this.Accounts.Add(newAccount);
        }

        /// <summary>
        /// Quita un objeto Account de la lista List<Account>
        /// </summary>
        /// <param name="account"></param>
        public void RemoveAccount(string accountName)
        {
            foreach (Account account in this.Accounts)
            {
                if (account.Name == accountName)
                {
                    this.Accounts.Remove(account);
                    return;
                }
            }
            System.Console.WriteLine("No se ha encontrado la cuenta " + accountName);
        }

        /// <summary>
        /// Cambia el nombre de usuario
        /// </summary>
        /// <param name="newUserName"></param>
        public void ChangeUserName(string newUserName)
        {
            this.UserName = newUserName;
        }


        /// <summary>
        /// Cambia la contraseña generando un nuevo string cifrado
        /// </summary>
        /// <param name="newPassword"></param>
        public void ChangePassword(string newPassword)
        {
            this.Password = Cypher(newPassword);
        }


        /// <summary>
        /// Muestra todas las cuentas disponibles en consola de forma indexada
        /// </summary>
        public StringBuilder ShowAccounts()
        {
            StringBuilder accounts = new StringBuilder();
            if (this.Accounts.Count == 0)
            {
                accounts.Append("No hay cuentas asociadas a este usuario.");
            }
            else
            {
                foreach (Account account in this.Accounts)
                {
                    accounts.Append($"{this.Accounts.IndexOf(account) + 1} - {account.Name}\n");
                }
            }
            System.Console.WriteLine(accounts);
            return accounts;
        }
        public void AddItem(string name, string option)
        {
            //Income
            if (option == "1" && !this.OutcomeList.Contains(name))
            {
                this.IncomeList.Add(name);
            }
            //Outcome
            else if (option == "2" && !this.IncomeList.Contains(name))
            {
                this.OutcomeList.Add(name);
            }
            else if (this.IncomeList.Contains(name) || this.OutcomeList.Contains(name))
            {
                System.Console.WriteLine("Este elemento ya existe.");
            }
            else
            {
                System.Console.WriteLine("Opción incorrecta");
            }
        }
        public void RemoveItem(string name)
        {
            //Income
            if (this.IncomeList.Contains(name))
            {
                this.IncomeList.Remove(name);
            }
            //Outcome
            else if (this.OutcomeList.Contains(name))
            {
                this.OutcomeList.Remove(name);
            }
            else
            {
                System.Console.WriteLine("No existe dicho elemento en los rubros.");
            }
        }
        public void ShowItems()
        {
            StringBuilder res = new StringBuilder();
            res.Append("Income:\n\n");
            foreach (String item in this.IncomeList)
            {
                res.Append((this.IncomeList.IndexOf(item) + 1).ToString() + " - " + item + "\n");
            }
            res.Append("Outcome:\n\n");
            foreach (string item in this.OutcomeList)
            {
                res.Append((this.IncomeList.IndexOf(item) + 1).ToString() + " - " + item + "\n");
            }
        }


        //PasswordCode

        /// <summary>
        /// Clase que utilizando la funcion de derivacion clave PBKDF2 genera una contraseña cifrada y es capaz de descifrarla
        /// </summary>
        private const int SaltByteSize = 24;
        private const int HashByteSize = 20;
        private const int Pbkdf2Iterations = 1000;
        private const int IterationIndex = 0;
        private const int SaltIndex = 1;
        private const int Pbkdf2Index = 2;

        /// <summary>
        /// Recibiendo un string por parametro este metodo se encarga de generar un Salt, para luego por medio del metodo GetPbkdf2Bytes recibir un hash
        /// de esta forma generando una contraseña cifrada
        /// </summary>
        /// <param name="password"></param>
        /// <returns></returns>

        public bool Login(string password)
        {
            return Decrypt(password, this.Password);
        }

        private string Cypher(string password)
        {
            var cryptoProvider = new RNGCryptoServiceProvider();
            byte[] salt = new byte[SaltByteSize];
            cryptoProvider.GetBytes(salt);

            var hash = GetPbkdf2Bytes(password, salt, Pbkdf2Iterations, HashByteSize);
            return Pbkdf2Iterations + ":" +
                   Convert.ToBase64String(salt) + ":" +
                   Convert.ToBase64String(hash);
        }

        /// <summary>
        /// Recibe un string y una contraseña cifrada, reevierte el proceso de Cypher y devuelve un bool comparando ambas contraseñas
        /// </summary>
        /// <param name="password"></param>
        /// <param name="correctHash"></param>
        /// <returns></returns>

        private bool Decrypt(string password, string correctHash)
        {
            char[] delimiter = { ':' };
            var split = correctHash.Split(delimiter);
            var iterations = Int32.Parse(split[IterationIndex]);
            var salt = Convert.FromBase64String(split[SaltIndex]);
            var hash = Convert.FromBase64String(split[Pbkdf2Index]);

            var testHash = GetPbkdf2Bytes(password, salt, iterations, hash.Length);
            return SlowEquals(hash, testHash);
        }

        /// <summary>
        /// Compara ambas contraseñas provistas
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>

        private bool SlowEquals(byte[] a, byte[] b)
        {
            var diff = (uint)a.Length ^ (uint)b.Length;
            for (int i = 0; i < a.Length && i < b.Length; i++)
            {
                diff |= (uint)(a[i] ^ b[i]);
            }
            return diff == 0;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="password"></param>
        /// <param name="salt"></param>
        /// <param name="iterations"></param>
        /// <param name="outputBytes"></param>
        /// <returns></returns>

        private byte[] GetPbkdf2Bytes(string password, byte[] salt, int iterations, int outputBytes)
        {
            var pbkdf2 = new Rfc2898DeriveBytes(password, salt);
            pbkdf2.IterationCount = iterations;
            return pbkdf2.GetBytes(outputBytes);
        }
        public bool AccountExist(string name)
        {
            foreach (Account account in this.Accounts)
            {
                if (account.Name == name) return true;
            }
            return false;
        }
        public Account GetAccount (string name)
        {
            foreach (Account account in this.Accounts)
            {
                if (account.Name == name) return account;
            }
            return Account.Empty;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Bankbot
{
    public class AllCommands
    {
        public List<String> CommandsList { get; set; }
        private static AllCommands instance;
        
        public static AllCommands Instance
        {
            get
            {
                if (instance == null) instance = new AllCommands();
                return instance;
            }
        }
        private AllCommands()
        {
            this.CommandsList = new List<String>();
            this.CommandsList.Add("/Abort"); 
            this.CommandsList.Add("/Login");               
            this.CommandsList.Add("/Convertion");        
            this.CommandsList.Add("/CreateUser");                
            this.CommandsList.Add("/Logout");          
            this.CommandsList.Add("/DeleteUser");     
            this.CommandsList.Add("/CreateAccount");      
            this.CommandsList.Add("/DeleteAccount");          
            this.CommandsList.Add("/Transaction");   
            //this.CommandsList.Add("/Init");         El usuario no deberia de poder editar este comando
        }
        public static String CommandsString(IMessage message)
        {
            String commandList = String.Empty;
            if ((User)AllChats.Instance.ChatsDictionary[message.id].DataDictionary["User"] == User.Empty )
            {
                foreach (String command in UnlogedCommandsList())
                {
                    commandList+=command+"\n";
                }
            }
            else if (((User)AllChats.Instance.ChatsDictionary[message.id].DataDictionary["User"]).Accounts.Count==0)
            {
                foreach (String command in HasNoAccountsCommandsList())
                {
                    commandList+=command+"\n";
                }
            }
            else
            {
                foreach (String command in HasAccountCommandsList())
                {
                    commandList+=command+"\n";
                }
            }
            return commandList;
        }
        private static List<String> UnlogedCommandsList()
        {
            List<String> unlogedList = new List<String>();
            unlogedList.Add("/Login");
            unlogedList.Add("/Convertion");
            unlogedList.Add("/CreateUser");
            return unlogedList;
        }
        private static List<String> HasNoAccountsCommandsList()
        {
            List<String> hasNoAccountsList = new List<String>();
            hasNoAccountsList.Add("/Logout");
            hasNoAccountsList.Add("/DeleteUser");
            hasNoAccountsList.Add("/CreateAccount");
            hasNoAccountsList.Add("/Convertion");
            hasNoAccountsList.Add("/CreateUser");
            return hasNoAccountsList;
        }
        private static List<String> HasAccountCommandsList()
        {
            List<String> hasAccountsList = new List<String>();
            hasAccountsList.Add("/Logout");
            hasAccountsList.Add("/DeleteUser");
            hasAccountsList.Add("/CreateAccount");
            hasAccountsList.Add("/Convertion");
            hasAccountsList.Add("/CreateUser");
            hasAccountsList.Add("/DeleteAccount");
            hasAccountsList.Add("/Transaction");
            return hasAccountsList;
        }

        public bool CommandExist(string command)
        {
            return instance.CommandsList.Contains(command);
        }
    }
}

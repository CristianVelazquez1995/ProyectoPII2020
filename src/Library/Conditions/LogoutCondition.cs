using System;

namespace Bankbot
{
    public class LogoutCondition : ICondition<IMessage>
    {
        public bool IsSatisfied(IMessage request)
        {
            Data data = Data.Empty;
            return AllChats.Instance.ChatsDictionary.TryGetValue(request.id,out data)
                && (String) data.DataDictionary["LastCommand"] == "/Logout"
            ;
        }
    }
}
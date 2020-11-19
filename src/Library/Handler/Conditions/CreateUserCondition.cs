namespace Bankbot
{
    /*Cumple con EXPERT y SRP*/
    /// <summary>
    /// Interactúa para que se pueda crear un nuevo usuario no existente.
    /// </summary>
    public class CreateUserCondition : ICondition<IMessage>
    {
        public bool IsSatisfied(IMessage request)
        {
            var data = Session.Instance.GetChat(request.Id);
            return data.State == State.HandlingCommand && data.Command.ToLower() == "/createuser";
        }
    }
}
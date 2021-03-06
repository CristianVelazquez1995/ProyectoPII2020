using System.Collections.Generic;

namespace Bankbot
{
    public class Search
    {
        /*Cumple con ## SINGLETON ## ya que se va a crear una única instancia.
        */

        /// <summary>
        /// Busqueda inteligente. 
        /// </summary>
        private static Search instance;
        public static Search Instance
        {
            get
            {
                if (instance == null) instance = new Search();
                return instance;
            }
        }

        private Search() { }
      
        /// <summary>
        /// Aplica filtros de búsqueda.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="list"></param>
        public string ApplyFilter(string id, List<Transaction> list)
        {
            var data = Session.Instance.GetChat(id);
            IPipe lastPipe = null;
            data.Filters.Reverse();


            if (data.Filters.Count == 0)
            {
                lastPipe = new FilterPipe(new FilterNull(), new PipeNull());
            }
            else
            {
                foreach (var item in data.Filters)
                {
                    IPipe nextPipe = lastPipe == null ? new PipeNull() : lastPipe;
                    IPipe pipe = new FilterPipe(item, nextPipe);
                    lastPipe = pipe;
                }
            }

            return Session.Instance.Printer.Print(lastPipe.Send(list), data.User.Username);
        }
    }
}
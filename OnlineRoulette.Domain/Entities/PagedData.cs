using System.Collections.Generic;

namespace OnlineRoulette.Domain.Entities
{
    public class PagedData<T>
    {
        public PagedData(IEnumerable<T> data, int numberOfRows)
        {
            Data = data;
            NumberOfRows = numberOfRows;
        }

        public IEnumerable<T> Data { get; set; }
        public int NumberOfRows { get; set; }
    }
}

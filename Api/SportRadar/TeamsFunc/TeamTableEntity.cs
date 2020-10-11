using Microsoft.Azure.Cosmos.Table;

namespace BlazorApp.Api.SportRadar.TeamsFunc
{
    public class TeamTableEntity:TableEntity
    {

        public TeamTableEntity(string conferenceId, string id)
        {
            PartitionKey = conferenceId;
            RowKey = id;
        }

        public TeamTableEntity()
        {
            
        }

        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string ConferenceId
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }

        public string Name { get; set; }

        public string Market { get; set; }

        public string ConferenceName { get; set; }
    }
}

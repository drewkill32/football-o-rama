


using Microsoft.Azure.Cosmos.Table;

namespace BlazorApp.Api.SportRadar.User
{
    public class UserTableEntity:TableEntity
    {
        public UserTableEntity()
        {
            PartitionKey = Keys.Azure.User;
        }

        public string UserId => RowKey;

        public string Team { get; set; }

        public string UserName { get; set; }

        public string Name { get; set; }

        public string Email { get; set; }

    }
}

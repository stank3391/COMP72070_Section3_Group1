

public class ClientManager
{
    private Dictionary<int, Client> clients = new Dictionary<int, Client>();

    /// <summary>
    /// Adds a client to the client manager and returns the unique key of the client
    /// </summary>
    public int AddClient(Client client)
    {
        // create a unqiue key for the client
        int key = clients.Count + 1;
        clients.Add(key, client);

        return key;
    }

    public Client GetClient(int key)
    {
        return clients[key];
    }
}


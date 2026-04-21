public class Router
{
    // Dictionary που κρατάει τα routes (key) και τη λογική τους (value)
    // key: π.χ. "GET /products"
    // value: μέθοδος που θα εκτελεστεί όταν καλεστεί το route

    private readonly Dictionary<string, Func<object>> _routes0 = new();
    private readonly Dictionary<string, Func<int, object>> _routes1 = new();
    // Καταχώρηση (register) ενός route στο routing table
    public void RegisterGet(string route, Func<object> action)
    {
        // Αν υπάρχει ήδη το route, το αντικαθιστούμε
        // Αν όχι, το προσθέτουμε
        _routes0[route] = action;
    }
    public void RegisterGetId(string route, Func<int, object> action)
    {
        // Αν υπάρχει ήδη το route, το αντικαθιστούμε
        // Αν όχι, το προσθέτουμε
        _routes1[route] = action;
    }

    // Εκτέλεση ενός route
    public void ExecuteGet(string route)
    {
        // TryGetValue: πιο ασφαλής τρόπος από ContainsKey + indexer
        // Αν βρεθεί το route, παίρνουμε τη μέθοδο (action)
        if (_routes0.TryGetValue(route, out var action))
        {
            // Καλούμε τη μέθοδο που είναι αποθηκευμένη
            action();
        }
        else
        {
            // Αν δεν υπάρχει το route, εμφανίζουμε μήνυμα
            Console.WriteLine("Route not found");
        }
    }
    public void ExecuteGetId(string route)
    {
        foreach (var kv in _routes1)
        {
            if (IsMatch(kv.Key, route, out int id))
            {
                kv.Value(id);
                return;
            }
        }

        Console.WriteLine("Route not found");
    }
    private bool IsMatch(string pattern, string request, out int id)
    {
        id = 0;

        var patternParts = pattern.Split('/');
        var requestParts = request.Split('/');

        if (patternParts.Length != requestParts.Length)
            return false;

        for (int i = 0; i < patternParts.Length; i++)
        {
            if (patternParts[i] == "{id}")
            {
                id = int.Parse(requestParts[i]);
            }
            else if (patternParts[i] != requestParts[i])
            {
                return false;
            }
        }

        return true;
    }
}
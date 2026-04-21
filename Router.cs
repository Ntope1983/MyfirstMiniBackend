public class Router
{
    // Dictionary που κρατάει τα routes (key) και τη λογική τους (value)
    // key: π.χ. "GET /products"
    // value: μέθοδος που θα εκτελεστεί όταν καλεστεί το route
    private readonly Dictionary<string, Func<IEnumerable<Product>>> _routes = new();

    // Καταχώρηση (register) ενός route στο routing table
    public void Register(string route, Func<IEnumerable<Product>> action)
    {
        _routes[route] = action;
    }

    // Εκτέλεση ενός route
    public void Execute(string route)
    {
        // TryGetValue: πιο ασφαλής τρόπος από ContainsKey + indexer
        // Αν βρεθεί το route, παίρνουμε τη μέθοδο (action)
        if (_routes.TryGetValue(route, out var action))
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
}
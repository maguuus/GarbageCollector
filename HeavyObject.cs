namespace GarbageExperiment;

class HeavyObject
{
    public static bool GcStarted;
    public static int FirstCollectedId;

    private readonly byte[] _data;
    private readonly int _id;

    public HeavyObject(int id)
    {
        _id = id;
        _data = new byte[20 * 1024];
    }

    ~HeavyObject()
    {
        if (!GcStarted)
        {
            GcStarted = true;
            FirstCollectedId = _id;
            Console.WriteLine(
                $"GC начал финализацию на объекте #{_id}");
        }
    }

    public static void ResetStats()
    {
        GcStarted = false;
        FirstCollectedId = -1;
    }
}
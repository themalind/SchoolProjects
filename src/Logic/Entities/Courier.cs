namespace Logic.Entities;

public class Courier
{
    public int Id { get; private set; }
    public string Name { get; set; }

#pragma warning disable CS8618
    private Courier() { }
#pragma warning restore CS8618 
    public Courier(string name)
    {
        Name = name;
    }
}
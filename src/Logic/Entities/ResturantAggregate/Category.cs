namespace Logic.Entities;

public class Category
{
    public int Id { get; private set; }
    public string Name { get; set; }

#pragma warning disable CS8618
    private Category() { }
#pragma warning restore CS8618 

    public Category(string name)
    {
        Name = name;
    }

}
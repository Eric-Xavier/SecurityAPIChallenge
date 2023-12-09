namespace IntegrationTests;
public class DummyService
{
    public string value{ get; set;}

    public DummyService()
    {
        value = Guid.NewGuid().ToString();
    }

}

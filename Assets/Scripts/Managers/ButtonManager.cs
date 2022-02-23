using Rti.Dds.Publication;
using Rti.Types.Dynamic;
using UnityEngine;


public class ButtonManager : MonoBehaviour
{
    private DDSHandler dDSHandler;
    private protected DataWriter<DynamicData> writer { get; private set; }
    private DynamicData sample = null;
    private bool init = false;
    private int count = 1;
    public GameObject teleoppanel;

    // Start is called before the first frame update
    void Start()
    {
        dDSHandler = gameObject.AddComponent<DDSHandler>();
    }

    void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;
            var OperatorButtons = typeFactory.BuildEnum()
                .WithName("OperatorButtons")
                .AddMember(new EnumMember("RESET", ordinal: 0))
                .AddMember(new EnumMember("ABORT", ordinal: 1))
                .AddMember(new EnumMember("HOME", ordinal: 2))
                .AddMember(new EnumMember("TELEOP", ordinal: 3))
                .AddMember(new EnumMember("PATH", ordinal: 4))
                .AddMember(new EnumMember("OPEN", ordinal: 5))
                .AddMember(new EnumMember("CLOSE", ordinal: 6))
                .Create();

            var OperatorRequest = typeFactory.BuildStruct()
               .WithName("OperatorRquests")
               .AddMember(new StructMember("Buttons", OperatorButtons))
               .AddMember(new StructMember("Samples", typeFactory.GetPrimitiveType<int>()))
               .Create();

            writer = dDSHandler.SetupDataWriter("OperatorRequests_Topic", OperatorRequest);
            sample = new DynamicData(OperatorRequest);
        }
    }

    public void RESETButton()
    {
        sample.SetValue("Buttons", 0);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void ABORTButton()
    {
        sample.SetValue("Buttons", 1);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void HOMEButton()
    {
        teleoppanel.SetActive(false);
        sample.SetValue("Buttons", 2);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void TELEOPButton()
    {
        teleoppanel.SetActive(true);
        sample.SetValue("Buttons", 3);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void PATHButton()
    {
        teleoppanel.SetActive(false);
        sample.SetValue("Buttons", 4);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void OPENButton()
    {
        sample.SetValue("Buttons", 5);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }

    public void CLOSEButton()
    {
        sample.SetValue("Buttons", 6);
        sample.SetValue("Samples", count);
        count++;
        writer.Write(sample);
    }
}

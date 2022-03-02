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
    private void Start()
    {
        dDSHandler = gameObject.GetComponent<DDSHandler>();
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
                .AddMember(new EnumMember("PATH", ordinal: 3))
                .Create();

            var OperatorRequest = typeFactory.BuildStruct()
               .WithName("OR")
               .AddMember(new StructMember("Buttons", OperatorButtons))
               .AddMember(new StructMember("S", typeFactory.GetPrimitiveType<int>()))
               .Create();

            writer = dDSHandler.SetupDataWriter("OR_Topic", OperatorRequest);
            sample = new DynamicData(OperatorRequest);
        }
    }

    public void RESETButton()
    {
        Debug.Log("RESET");
        sample.SetValue("Buttons", 0);
        sample.SetValue("S", count);
        count++;
        writer.Write(sample);
    }

    public void ABORTButton()
    {
        Debug.Log("ABORT");
        sample.SetValue("Buttons", 1);
        sample.SetValue("S", count);
        count++;
        writer.Write(sample);
    }

    public void HOMEButton()
    {
        Debug.Log("HOME");
        sample.SetValue("Buttons", 2);
        sample.SetValue("S", count);
        count++;
        writer.Write(sample);
    }

    public void PATHButton()
    {
        Debug.Log("PATH");
        sample.SetValue("Buttons", 3);
        sample.SetValue("S", count);
        count++;
        writer.Write(sample);
    }
}

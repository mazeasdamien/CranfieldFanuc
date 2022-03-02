using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using TMPro;
using UnityEngine;

public class TeleopReachability : MonoBehaviour
{
    private DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }

    private bool init = false;
    public GameObject controller;
    public TMP_Text clock;
    public TMP_Text samplesCount;

    private void Start()
    {
        dDSHandler = gameObject.GetComponent<DDSHandler>();
    }

    private void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;
            StructType Reachable = typeFactory.BuildStruct()
                .WithName("Reachability")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("isReachable", typeFactory.GetPrimitiveType<bool>()))
                .Create();

            reader = dDSHandler.SetupDataReader("Reachability_Topic", Reachable);
        }
        ProcessData(reader);
    }

    private void ProcessData(AnyDataReader anyReader)
    {
        var reader = (DataReader<DynamicData>)anyReader;
        using var samples = reader.Take();
        foreach (var sample in samples)
        {
            if (sample.Info.ValidData)
            {
                DynamicData data = sample.Data;

                clock.text = data.GetValue<string>("Clock");
                samplesCount.text = $"Samples sent: {data.GetValue<int>("Sample")}";
                if (data.GetValue<bool>("isReachable") == true)
                {
                    controller.GetComponent<MeshRenderer>().material.color = Color.green;
                }
                else
                {
                    controller.GetComponent<MeshRenderer>().material.color = Color.red;
                }
            }
        }
    }
}

using Rti.Dds.Subscription;
using Rti.Types.Dynamic;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.VFX;

public class PointCloudManager : MonoBehaviour
{
    public VisualEffect effect;

    Mesh mesh;
    readonly List<Vector3> vertices = new();
    readonly List<Color32> colors = new();
    readonly List<int> indices = new();

    private bool init = false;

    public DDSHandler dDSHandler;
    private protected DataReader<DynamicData> reader { get; private set; }
    public TMP_Text clock;
    public TMP_Text samplesCount;

    // Update is called once per frame
    private void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;
            StructType RobotPointCloud = typeFactory.BuildStruct()
                .WithName("RobotPointCloud")
                .AddMember(new StructMember("Clock", typeFactory.CreateString(bounds: 50)))
                .AddMember(new StructMember("Sample", typeFactory.GetPrimitiveType<int>()))
                .AddMember(new StructMember("sequence_Memory", typeFactory.CreateSequence(typeFactory.GetPrimitiveType<float>(), 600000)))
                .Create();
            reader = dDSHandler.SetupDataReader("RobotPointCloud_Topic", RobotPointCloud);
        }
        ProcessData(reader);
    }

    void ProcessData(AnyDataReader anyReader)
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
                vertices.Clear();
                colors.Clear();
                indices.Clear();
                mesh = new Mesh
                {
                    indexFormat = UnityEngine.Rendering.IndexFormat.UInt32,
                };

                float[] buffer = data.GetValue<float[]>("sequence_Memory");
                //Debug.Log(buffer.Length);
                int num = buffer.Length / 6;
                for (int i = 0; i < num; i++)
                {
                    indices.Add(i);
                }

                mesh.vertices = new Vector3[num];
                mesh.colors32 = new Color32[num];

                mesh.SetIndices(indices, MeshTopology.Points, 0);

                for (int i = 0; i < buffer.Length; i += 6)
                {
                    vertices.Add(new Vector3
                    {
                        x = buffer[i] * 0.001f,
                        y = -buffer[i + 1] * 0.001f,
                        z = buffer[i + 2] * 0.001f
                    });

                    colors.Add(new Color32
                    {
                        b = (byte)buffer[i + 3],
                        g = (byte)buffer[i + 4],
                        r = (byte)buffer[i + 5],
                        a = 255
                    });
                }

                mesh.vertices = vertices.ToArray();
                mesh.colors32 = colors.ToArray();
                effect.SetMesh("RemoteData", mesh);
            }
        }
    }
}
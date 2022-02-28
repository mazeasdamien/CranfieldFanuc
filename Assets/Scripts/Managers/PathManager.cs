using Rti.Dds.Publication;
using Rti.Types.Dynamic;
using UnityEngine;

public class PathManager : MonoBehaviour
{
    public DDSHandler dDSHandler;
    private protected DataWriter<DynamicData> writer { get; private set; }
    private DynamicData sample = null;
    private bool init = false;

    public int id = 0;
    public int totalId = 0;

    void Update()
    {
        if (!init)
        {
            init = true;
            var typeFactory = DynamicTypeFactory.Instance;

            var Path_position = typeFactory.BuildStruct()
               .WithName("Path_position")
               .AddMember(new StructMember("ID", typeFactory.GetPrimitiveType<int>()))
               .AddMember(new StructMember("isUpdating", typeFactory.GetPrimitiveType<bool>()))
               .AddMember(new StructMember("isDelete", typeFactory.GetPrimitiveType<bool>()))
               .AddMember(new StructMember("X", typeFactory.GetPrimitiveType<float>()))
               .AddMember(new StructMember("Y", typeFactory.GetPrimitiveType<float>()))
               .AddMember(new StructMember("Z", typeFactory.GetPrimitiveType<float>()))
               .AddMember(new StructMember("W", typeFactory.GetPrimitiveType<float>()))
               .AddMember(new StructMember("P", typeFactory.GetPrimitiveType<float>()))
               .AddMember(new StructMember("R", typeFactory.GetPrimitiveType<float>()))
               .Create();

            writer = dDSHandler.SetupDataWriter("Path_position_Topic", Path_position);
            sample = new DynamicData(Path_position);
        }
    }

    public void Update_Path_Position(float id, bool isUpdating, float x, float y , float z, float w, float p, float r)
    {
        sample.SetValue("ID", id);
        sample.SetValue("isDelete", false);
        sample.SetValue("isUpdating", isUpdating);
        sample.SetValue("X", x);
        sample.SetValue("Y", y);
        sample.SetValue("Z", z);
        sample.SetValue("W", w);
        sample.SetValue("P", p);
        sample.SetValue("R", r);
        writer.Write(sample);
    }

    public void Delete_Path_Position()
    {
        id = 0;
        sample.SetValue("ID", 0);
        sample.SetValue("isDelete", true);
        sample.SetValue("X", 0);
        sample.SetValue("Y", 0);
        sample.SetValue("Z", 0);
        sample.SetValue("W", 0);
        sample.SetValue("P", 0);
        sample.SetValue("R", 0);
        writer.Write(sample);
    }
}

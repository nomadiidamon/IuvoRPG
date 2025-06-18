// this is what all objects that will be serialized should implement
public interface ISavable
{
    SerializableGuid Id { get; set; }
}

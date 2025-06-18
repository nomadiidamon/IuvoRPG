using UnityEngine;

// every object that has serializable data should implement this to bind data to it
public interface IBind <TData> where TData : ISavable
{
    // Id to recognize this specefic individual
    SerializableGuid Id { get; set; }
    // Method to bind data to the object
    void Bind(TData data);
}

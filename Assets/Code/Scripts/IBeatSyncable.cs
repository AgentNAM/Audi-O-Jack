using UnityEngine;

public interface IBeatSyncable
{
    Conductor conductor { get; set; }
    bool truncateBeats { get; set; }
}

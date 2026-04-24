using UnityEngine;

public interface IResettable
{
    void SaveSnapshot();
    void RestoreSnapshot();
}


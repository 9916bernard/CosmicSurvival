using UnityEngine;
using Cysharp.Threading.Tasks;

public class UserDataBaseRoot
{
    virtual public void Load() { }

    virtual public async UniTask LoadAsync() { await UniTask.FromResult(0); }

    virtual public void Save() { }

    virtual public void MakeDefaultData() { }
}

public partial class UserDataBase<T> : UserDataBaseRoot where T : class, new()
{
    protected string _userDataName = null;

    protected T _data = null;
    public T data { get { return _data; } set { _data = value; } }

    public override void Load()
    {
        _data = Util.LoadJsonFile<T>(Application.temporaryCachePath, _userDataName);
    }

    public override async UniTask LoadAsync()
    {
        _data = await Util.LoadJsonFileAsync<T>(Application.temporaryCachePath, _userDataName);
    }

    public override void Save()
    {
        if (_data == null)
        {
            return;
        }

        Util.SaveJsonFile(Application.temporaryCachePath, _userDataName, _data);
    }
}

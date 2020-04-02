using System.IO.Abstractions;
using System.Threading.Tasks;
using Mt.MediaMan.AppEngine.CatalogStorage;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The structure access factory.
  /// </summary>
  internal class StructureAccessFactory : IStructureAccessFactory
  {
    private readonly IFileSystem _fileSystem;
    private readonly IItemStorage _itemStorage;

    /// <summary>
    /// Ctor.
    /// </summary>
    public StructureAccessFactory(IFileSystem fileSystem, IItemStorage itemStorage)
    {
      this._fileSystem = fileSystem;
      this._itemStorage = itemStorage;
    }

    /// <summary>
    /// IStructureAccessFactory.
    /// </summary>
    public Task<IStructureAccess> CreateAsync(int scanRootId)
    {
      var result = new StructureAccessFs(this._fileSystem, this._itemStorage, scanRootId);
      return Task.FromResult<IStructureAccess>(result);
    }
  }
}

using System.Threading.Tasks;

namespace Mt.MediaMan.AppEngine.Cataloging
{
  /// <summary>
  /// The structure access factory.
  /// </summary>
  internal interface IStructureAccessFactory
  {
    /// <summary>
    /// Creates the structure access for given scan root.
    /// </summary>
    Task<IStructureAccess> CreateAsync(int scanRootId);
  }
}

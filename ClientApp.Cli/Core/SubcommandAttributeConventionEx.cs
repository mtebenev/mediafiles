using System;
using System.Linq;
using System.Reflection;
using McMaster.Extensions.CommandLineUtils;
using McMaster.Extensions.CommandLineUtils.Abstractions;
using McMaster.Extensions.CommandLineUtils.Conventions;
using McMaster.Extensions.CommandLineUtils.Errors;
using Mt.MediaFiles.ClientApp.Cli.Core;

namespace Mt.MediaFiles.ClientApp.Cli.Configuration
{
  /// <summary>
  /// Essentially it's the copy of the original SubcommandAttributeConvention but respects experimental mode.
  /// </summary>
  internal class SubcommandAttributeConventionEx : IConvention
  {
    public SubcommandAttributeConventionEx(bool experimentalMode)
    {
      this._experimentalMode = experimentalMode;
    }

    /// <inheritdoc />
    public virtual void Apply(ConventionContext context)
    {
      var modelAccessor = context.ModelAccessor;
      if(context.ModelType == null || modelAccessor == null)
      {
        return;
      }

      var attributes = context.ModelType.GetCustomAttributes<SubcommandAttribute>();

      foreach(var attribute in attributes)
      {
        var contextArgs = new object[] { context };
        foreach(var type in attribute.Types)
        {
          var experimentalAttribute = type.GetCustomAttribute<ExperimentalCommandAttribute>();
          if(!this._experimentalMode && experimentalAttribute != null)
          {
            continue;
          }

          AssertSubcommandIsNotCycled(type, context.Application);

          var impl = s_addSubcommandMethod.MakeGenericMethod(type);
          try
          {
            impl.Invoke(this, contextArgs);
          }
          catch(TargetInvocationException ex)
          {
            // unwrap
            throw ex.InnerException ?? ex;
          }
        }
      }
    }

    private void AssertSubcommandIsNotCycled(Type modelType, CommandLineApplication? parentCommand)
    {
      while(parentCommand != null)
      {
        if(parentCommand is IModelAccessor parentCommandAccessor
            && parentCommandAccessor.GetModelType() == modelType)
        {
          throw new SubcommandCycleException(modelType);
        }
        parentCommand = parentCommand.Parent;
      }
    }

    private static readonly MethodInfo s_addSubcommandMethod
        = typeof(SubcommandAttributeConventionEx).GetRuntimeMethods()
            .Single(m => m.Name == nameof(AddSubcommandImpl));
    private bool _experimentalMode;

    private void AddSubcommandImpl<TSubCommand>(ConventionContext context)
        where TSubCommand : class
    {
      context.Application.Command<TSubCommand>(null, null);
    }
  }
}

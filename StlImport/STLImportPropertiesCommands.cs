using System;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;

namespace StlImport
{
  [Transaction( TransactionMode.ReadOnly )]
  class SetToSolidCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetModeToSolid();

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetToAnyGeometryCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetModeToAnyGeometry();

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetToPolymeshCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetModeToPolymesh();

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetStyleToNoneCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetGraphicsStyleToInvalid();

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetStyleToSketchCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      Document doc = commandData.Application.ActiveUIDocument.Document;

      StlImportProperties.GetProperties().SetGraphicsStyleToSketch( doc );

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetDataTypeToBinaryCommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetModeToBinary();

      return Result.Succeeded;
    }

    #endregion
  }

  [Transaction( TransactionMode.ReadOnly )]
  class SetDataTypeToASCIICommand : IExternalCommand
  {
    #region IExternalCommand Members

    public Result Execute( ExternalCommandData commandData, ref string message, ElementSet elements )
    {
      StlImportProperties.GetProperties().SetModelToASCII();

      return Result.Succeeded;
    }

    #endregion
  }
}

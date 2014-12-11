using System;
//using System.Collections.Generic;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using System.Windows.Media.Imaging;
using System.Windows;

namespace StlImport
{
  public class StlImportApplication : IExternalApplication
  {
    static String addinAssemblyPath = typeof( StlImportApplication ).Assembly.Location;

    #region IExternalApplication Members

    public Result OnShutdown( UIControlledApplication application )
    {
      return Result.Succeeded;
    }

    public Result OnStartup( UIControlledApplication application )
    {
      SetupUI( application );

      return Result.Succeeded;
    }

    private void SetupUI( UIControlledApplication application )
    {
      RibbonPanel panel = application.CreateRibbonPanel( "STL import" );

      PushButtonData stlImportButton = new PushButtonData( "StlImport", "Import STL file",
        addinAssemblyPath, typeof( StlImport.StlImportCommand ).FullName );
      PushButton button = panel.AddItem( stlImportButton ) as PushButton;

      //SetIconsForPushButton(button, StlImport.Properties.Resources.StrcturalWall);

      panel = application.CreateRibbonPanel( "STL import properties" );

      RadioButtonGroupData radioGroupData = new RadioButtonGroupData( "Mode" );
      RadioButtonGroup radioGroup = panel.AddItem( radioGroupData ) as RadioButtonGroup;

      ToggleButtonData solidButton = new ToggleButtonData( "SetModeToSolid", "Solid",
        addinAssemblyPath, typeof( StlImport.SetToSolidCommand ).FullName );
      ToggleButton solid = radioGroup.AddItem( solidButton );
      /*
      ToggleButtonData anyGeometryButton = new ToggleButtonData("SetModeToAnyGeometry", "Any Geometry",
        addinAssemblyPath, typeof(StlImport.SetToAnyGeometryCommand).FullName);
      ToggleButton anyGeometry = radioGroup.AddItem(anyGeometryButton) as ToggleButton;
      */
      ToggleButtonData polymeshButton = new ToggleButtonData( "SetModeToPolymesh", "Polymesh",
        addinAssemblyPath, typeof( StlImport.SetToPolymeshCommand ).FullName );
      radioGroup.AddItem( polymeshButton );

      radioGroup.Current = solid;

      panel.AddSeparator();

      radioGroupData = new RadioButtonGroupData( "Style" );
      radioGroup = panel.AddItem( radioGroupData ) as RadioButtonGroup;

      ToggleButtonData noneButton = new ToggleButtonData( "SetStyleToNone", "None",
        addinAssemblyPath, typeof( StlImport.SetStyleToNoneCommand ).FullName );
      radioGroup.AddItem( noneButton );

      ToggleButtonData sketchStyleButton = new ToggleButtonData( "SetStyleToSketch", "Sketch",
        addinAssemblyPath, typeof( StlImport.SetStyleToSketchCommand ).FullName );
      radioGroup.AddItem( sketchStyleButton );

      panel.AddSeparator();

      radioGroupData = new RadioButtonGroupData( "DataType" );
      radioGroup = panel.AddItem( radioGroupData ) as RadioButtonGroup;

      ToggleButtonData binaryButton = new ToggleButtonData( "SetDataTypeToBinary", "Binary",
        addinAssemblyPath, typeof( StlImport.SetDataTypeToBinaryCommand ).FullName );
      radioGroup.AddItem( binaryButton );

      ToggleButtonData asciiButton = new ToggleButtonData( "SetDataTypeToASCII", "ASCII",
        addinAssemblyPath, typeof( StlImport.SetDataTypeToASCIICommand ).FullName );
      radioGroup.AddItem( asciiButton );
    }

    #endregion


    /// <summary>
    /// Utility for adding icons to the button.
    /// </summary>
    /// <param name="button">The push button.</param>
    /// <param name="icon">The icon.</param>
    private static void SetIconsForPushButton( PushButton button, System.Drawing.Icon icon )
    {
      button.LargeImage = GetStdIcon( icon );
      button.Image = GetSmallIcon( icon );
    }

    /// <summary>
    /// Gets the standard sized icon as a BitmapSource.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <returns>The BitmapSource.</returns>
    private static BitmapSource GetStdIcon( System.Drawing.Icon icon )
    {
      return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
        icon.Handle,
        Int32Rect.Empty,
        BitmapSizeOptions.FromEmptyOptions() );
    }

    /// <summary>
    /// Gets the small sized icon as a BitmapSource.
    /// </summary>
    /// <param name="icon">The icon.</param>
    /// <returns>The BitmapSource.</returns>
    private static BitmapSource GetSmallIcon( System.Drawing.Icon icon )
    {
      System.Drawing.Icon smallIcon = new System.Drawing.Icon( icon, new System.Drawing.Size( 16, 16 ) );
      return System.Windows.Interop.Imaging.CreateBitmapSourceFromHIcon(
        smallIcon.Handle,
        Int32Rect.Empty,
        BitmapSizeOptions.FromEmptyOptions() );
    }
  }
}

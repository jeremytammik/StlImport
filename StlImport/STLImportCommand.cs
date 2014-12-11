using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using Autodesk.Revit.Attributes;
using Autodesk.Revit.DB;
using Autodesk.Revit.UI;
using QuantumConcepts.Formats.StereoLithography;

namespace StlImport
{
  [Transaction( TransactionMode.Manual )]
  class StlImportCommand : IExternalCommand
  {
    private XYZ FromVertex( Vertex v )
    {
      return new XYZ( (double) v.X, (double) v.Y, (double) v.Z );
    }

    private TessellatedFace FromFacet( Facet f )
    {
      List<XYZ> xyzs = new List<XYZ>();

      foreach( Vertex v in f.Vertices )
      {
        xyzs.Add( FromVertex( v ) );
      }
      return new TessellatedFace( xyzs, ElementId.InvalidElementId );
    }

    private string SelectSTLFile( string initialDirectory )
    {
      OpenFileDialog dialog = new OpenFileDialog();

      dialog.Filter
        = "STL files (*.stl)|*.stl|All files (*.*)|*.*";

      dialog.InitialDirectory = initialDirectory;
      dialog.Title = "Select STL file";
      return ( dialog.ShowDialog() == DialogResult.OK )
        ? dialog.FileName
        : null;
    }

    private void ImportSTLDocument(
      STLDocument stlDocument,
      Document doc,
      string stlDocumentName )
    {
      StlImportProperties properties = StlImportProperties.GetProperties();
      using( Transaction t = new Transaction( doc, "Import STL" ) )
      {
        t.Start();

        TessellatedShapeBuilder builder = new TessellatedShapeBuilder();

        builder.OpenConnectedFaceSet( false );
        int i = 0;

        foreach( Facet facet in stlDocument.Facets )
        {
          builder.AddFace( FromFacet( facet ) );
          i++;
        }

        builder.CloseConnectedFaceSet();

        TessellatedShapeBuilderResult result
          = builder.Build( properties.Target,
            properties.Fallback,
            properties.GraphicsStyleId );

        // Pre-release code from DevDays

        //DirectShape ds = DirectShape.CreateElement(
        //  doc, result.GetGeometricalObjects(), "A", "B");

        //ds.SetCategoryId(new ElementId(
        //  BuiltInCategory.OST_GenericModel));

        // Code updated for Revit UR1

        ElementId categoryId = new ElementId( BuiltInCategory.OST_GenericModel );
        DirectShape ds = DirectShape.CreateElement( doc, categoryId, "A", "B" );
        ds.SetShape( result.GetGeometricalObjects() );
        ds.Name = stlDocumentName;

        t.Commit();
      }
    }

    public void ImportSTL( Document doc )
    {
      String assemblyPath = this.GetType().Assembly.Location;
      String stlPath = Path.Combine( assemblyPath, "STL files" );
      String filename = SelectSTLFile( stlPath );
      if( filename == null )
        return;

      String documentName = Path.GetFileName( filename );

      // Read from file

      if( StlImportProperties.GetProperties().Binary )
      {
        using( BinaryReader reader = new BinaryReader( File.Open( filename, FileMode.Open ) ) )
        {
          STLDocument document = STLDocument.Read( reader );

          ImportSTLDocument( document, doc, documentName );

          reader.Close();
        }
      }
      else
      {
        using( StreamReader reader = new StreamReader( filename ) )
        {
          STLDocument document = STLDocument.Read( reader );

          ImportSTLDocument( document, doc, documentName );

          reader.Close();
        }
      }
    }

    #region IExternalCommand Members

    public Result Execute(
      ExternalCommandData commandData,
      ref string message,
      ElementSet elements )
    {
      Document doc = commandData.View.Document;

      ImportSTL( doc );

      return Result.Succeeded;
    }
    #endregion
  }
}

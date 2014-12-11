using System;
using System.Linq;
using Autodesk.Revit.DB;

namespace StlImport
{
  class StlImportProperties
  {
    private static StlImportProperties s_properties = null;

    private StlImportProperties()
    {
      Target = TessellatedShapeBuilderTarget.AnyGeometry;
      Fallback = TessellatedShapeBuilderFallback.Mesh;
      Binary = true;
      GraphicsStyleId = ElementId.InvalidElementId;
    }

    public static StlImportProperties GetProperties()
    {
      if( s_properties == null )
      {
        s_properties = new StlImportProperties();
      }

      return s_properties;
    }

    public void SetModeToSolid()
    {
      Target = TessellatedShapeBuilderTarget.Solid;
      Fallback = TessellatedShapeBuilderFallback.Abort;
    }

    public void SetModeToAnyGeometry()
    {
      Target = TessellatedShapeBuilderTarget.AnyGeometry;
      Fallback = TessellatedShapeBuilderFallback.Mesh;
    }

    public void SetModeToPolymesh()
    {
      Target = TessellatedShapeBuilderTarget.Mesh;
      Fallback = TessellatedShapeBuilderFallback.Salvage;
    }

    public TessellatedShapeBuilderTarget Target
    {
      get;
      private set;
    }

    public TessellatedShapeBuilderFallback Fallback
    {
      get;
      private set;
    }

    public void SetGraphicsStyleToInvalid()
    {
      GraphicsStyleId = ElementId.InvalidElementId;
    }

    public void SetGraphicsStyleToSketch( Document doc )
    {
      // Find GraphicsStyle

      FilteredElementCollector collector
        = new FilteredElementCollector( doc )
          .OfClass( typeof( GraphicsStyle ) );

      GraphicsStyle style = collector
        .Cast<GraphicsStyle>()
        .FirstOrDefault<GraphicsStyle>( gs
          => gs.Name.Equals( "<Sketch>" ) );

      if( style != null )
      {
        GraphicsStyleId = style.Id;
      }
    }

    public ElementId GraphicsStyleId
    {
      get;
      private set;
    }

    public void SetModeToBinary()
    {
      Binary = true;
    }

    public void SetModelToASCII()
    {
      Binary = false;
    }

    public bool Binary
    {
      get;
      private set;
    }
  }
}

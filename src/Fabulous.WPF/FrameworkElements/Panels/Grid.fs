namespace Fabulous.WPF

open System.Runtime.CompilerServices
open Fabulous
open System.Windows
open System.Windows.Controls

type IGrid=
    inherit IPanel

module GridUpdaters =
    let updateGridColumnDefinitions _ (newValueOpt: Dimension [] voption) (node: IViewNode) =
        let grid = node.Target :?> Grid

        match newValueOpt with
        | ValueNone -> grid.ColumnDefinitions.Clear()
        | ValueSome coll ->
            grid.ColumnDefinitions.Clear()

            for c in coll do
                let gridLength =
                    match c with
                    | Auto -> GridLength.Auto
                    | Star -> GridLength(1, GridUnitType.Star)
                    | Stars x -> GridLength(x, GridUnitType.Star)
                    | Absolute x -> GridLength(x, GridUnitType.Pixel)

                grid.ColumnDefinitions.Add(ColumnDefinition(Width = gridLength))

    let updateGridRowDefinitions _ (newValueOpt: Dimension [] voption) (node: IViewNode) =
        let grid = node.Target :?> Grid

        match newValueOpt with
        | ValueNone -> grid.RowDefinitions.Clear()
        | ValueSome coll ->
            grid.RowDefinitions.Clear()

            for c in coll do
                let gridLength =
                    match c with
                    | Auto -> GridLength.Auto
                    | Star -> GridLength(1, GridUnitType.Star)
                    | Stars x -> GridLength(x, GridUnitType.Star)
                    | Absolute x -> GridLength(x, GridUnitType.Pixel)

                grid.RowDefinitions.Add(RowDefinition(Height = gridLength))

module Grid =
    let WidgetKey = Widgets.register<Grid>()

    let ColumnDefinitions =
        Attributes.defineSimpleScalarWithEquality<Dimension array>
            "Grid_ColumnDefinitions"
            GridUpdaters.updateGridColumnDefinitions
    
    let RowDefinitions =
        Attributes.defineSimpleScalarWithEquality<Dimension array>
            "Grid_RowDefinitions"
            GridUpdaters.updateGridRowDefinitions
    
    let Column =
        Attributes.defineDependencyInt Grid.ColumnProperty
    
    let Row =
        Attributes.defineDependencyInt Grid.RowProperty
    
    //let ColumnSpacing =
    //    Attributes.defineDependencyFloat Grid.ColumnSpacingProperty
    
    //let RowSpacing =
    //    Attributes.defineDependencyFloat Grid.RowSpacingProperty
    
    let ColumnSpan =
        Attributes.defineDependencyInt Grid.ColumnSpanProperty
    
    let RowSpan =
        Attributes.defineDependencyInt Grid.RowSpanProperty
    
[<AutoOpen>]
module GridBuilders =
    type Fabulous.WPF.View with
        static member inline Grid<'msg>(coldefs: seq<Dimension>, rowdefs: seq<Dimension>) =
            CollectionBuilder<'msg, IGrid, IFrameworkElement>(
                Grid.WidgetKey,
                Panel.Children,
                Grid.ColumnDefinitions.WithValue(Array.ofSeq coldefs),
                Grid.RowDefinitions.WithValue(Array.ofSeq rowdefs)
            )
    
        static member inline Grid<'msg>() = View.Grid<'msg>([ Star ], [ Star ])

[<Extension>]
type GridModifiers =
   //[<Extension>]
   //static member inline columnSpacing(this: WidgetBuilder<'msg, #IGrid>, value: float) =
   //    this.AddScalar(Grid.ColumnSpacing.WithValue(value))

   //[<Extension>]
   //static member inline rowSpacing(this: WidgetBuilder<'msg, #IGrid>, value: float) =
   //    this.AddScalar(Grid.RowSpacing.WithValue(value))

   /// <summary>Link a ViewRef to access the direct Grid control instance</summary>
   [<Extension>]
   static member inline reference(this: WidgetBuilder<'msg, IGrid>, value: ViewRef<Grid>) =
       this.AddScalar(ViewRefAttributes.ViewRef.WithValue(value.Unbox))

[<Extension>]
type GridAttachedModifiers =
   [<Extension>]
   static member inline gridColumn(this: WidgetBuilder<'msg, #IFrameworkElement>, value: int) =
       this.AddScalar(Grid.Column.WithValue(value))

   [<Extension>]
   static member inline gridRow(this: WidgetBuilder<'msg, #IFrameworkElement>, value: int) =
       this.AddScalar(Grid.Row.WithValue(value))

   [<Extension>]
   static member inline gridColumnSpan(this: WidgetBuilder<'msg, #IFrameworkElement>, value: int) =
       this.AddScalar(Grid.ColumnSpan.WithValue(value))

   [<Extension>]
   static member inline gridRowSpan(this: WidgetBuilder<'msg, #IFrameworkElement>, value: int) =
       this.AddScalar(Grid.RowSpan.WithValue(value))

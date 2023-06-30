Imports System.ComponentModel
Imports System.Reflection.Emit
Imports System.Runtime.InteropServices.JavaScript.JSType
Imports CircuitTester.ECircuits
Imports CircuitTester.ECircuits.EComponent

Public Class ECircuitViewer

    <TypeConverterAttribute(GetType(System.ComponentModel.ExpandableObjectConverter))>
    <EditorBrowsable(EditorBrowsableState.Always)>
    <Category("Circuit")>
    Public Property Circuit As ECircuit

    <Category("Circuit")>
    Public Property CircuitOrigin As Point
    <Category("Circuit")>
    Public Property CircuitZoom As Single = 1

    Private _viewMode As ViewerMode = ViewerMode.Zoom
    <Category("Circuit")>
    Public Property CircuitViewMode As ViewerMode
        Get
            Return _viewMode
        End Get
        Set(value As ViewerMode)
            _viewMode = value
            If value = ViewerMode.Zoom Then
                Me.ContextMenuStrip = Nothing
            ElseIf value = ViewerMode.Edit Then
                Me.ContextMenuStrip = CircuitContextMenu
            End If
        End Set
    End Property
    <Category("Circuit")>
    Public Property CircuitGridSize As Single = 30
    <Category("Circuit")>
    Public Property CircuitGridColor As Color = SystemColors.Control
    <Category("Circuit")>
    Public Property CircuitSnapToGrid As Boolean = True

    Public Enum ViewerMode
        Zoom
        Edit
    End Enum

    Public Event ComponentChangedVisually(sender As Object, e As EventArgs)
    Public Event ComponentDeleted(sender As Object, e As EventArgs)
    Public Event SelectedComponentsChanged(sender As Object, e As EventArgs)
    'Public ReadOnly Property SelectedComponents As EComponent()
    '    Get
    '        Return Circuit.CircuitComponents.FindAll(Function(p) p.IsSelected = True).ToArray
    '    End Get
    'End Property

    Public Function IsAnySelected() As Boolean
        For Each component In Circuit.CircuitComponents
            If component.IsSelected Then
                Return True
            End If
        Next
        For Each connection In Circuit.CircuitConnections
            If connection.IsSelected Then
                Return True
            End If
        Next
        Return False
    End Function
    Public Function IsAnyUnselected() As Boolean
        For Each component In Circuit.CircuitComponents
            If Not component.IsSelected Then
                Return True
            End If
        Next
        For Each component In Circuit.CircuitConnections
            If Not component.IsSelected Then
                Return True
            End If
        Next

        Return False
    End Function


    Private CircuitTemporalWire As New EComponentConnection
    Private CircuitTemporalWireStartPoint As PointF
    Private CircuitTemporalWireEndPoint As PointF
    <Category("Circuit")>
    Public Property CircuitTemporalWireColor As Color = Color.FromArgb(128, Color.Black)


    Private Sub DuplicateSelectedComponents()
        If Not IsAnySelected() Then Exit Sub

        ' duplicate selected components
        For x = Circuit.CircuitComponents.Count - 1 To 0 Step -1
            Dim comp = Circuit.CircuitComponents(x)
            If comp.IsSelected Then
                Circuit.CircuitComponents.Add(comp.Copy())
                Circuit.CircuitComponents(x).IsSelected = False
            End If
        Next
        RaiseEvent ComponentChangedVisually(Me, New EventArgs)

    End Sub
    Private Sub RemoveSelectedComponentsAndWires()
        If Not IsAnySelected() Then Exit Sub

        ' delete selected components
        For x = Circuit.CircuitComponents.Count - 1 To 0 Step -1
            If Circuit.CircuitComponents(x).IsSelected Then

                ' delete associated connections
                For xx = Circuit.CircuitConnections.Count - 1 To 0 Step -1
                    If Circuit.CircuitConnections(xx).OriginComponentID = x Or Circuit.CircuitConnections(xx).DestinationComponentID = x Then
                        Circuit.CircuitConnections.RemoveAt(xx)
                    End If
                Next

                ' delete component
                Circuit.CircuitComponents.RemoveAt(x)
            End If
        Next

        ' delete selected connections
        For x = Circuit.CircuitConnections.Count - 1 To 0 Step -1
            If Circuit.CircuitConnections(x).IsSelected Then
                Circuit.CircuitConnections.RemoveAt(x)
            End If
        Next

        RaiseEvent ComponentDeleted(Me, New EventArgs)

    End Sub
    Private Function GetRelativeMouseLocation(mouse) As Point
        Dim newMouseLocation As Point
        newMouseLocation.X = CInt((mouse.X - CircuitOrigin.X) / CircuitZoom)
        newMouseLocation.Y = CInt((mouse.Y - CircuitOrigin.Y) / CircuitZoom)
        Return newMouseLocation
    End Function
    Private Sub CheckSeletedLinesAndComponents(mouse As PointF)
        Dim newMouseLocation = GetRelativeMouseLocation(mouse)

        ' CHECK SELECTED LINES

        For Each wire In Circuit.CircuitConnections
            If wire.DestinationComponentID < 0 Then Continue For
            If wire.DestinationComponentPin < 0 Then Continue For

            Dim orgPoint = Circuit.CircuitComponents(wire.OriginComponentID).GetAbsolutePinLocation(wire.OriginComponentPin)
            Dim destPoint = Circuit.CircuitComponents(wire.DestinationComponentID).GetAbsolutePinLocation(wire.DestinationComponentPin)

            If IsControlKeyDown Then
                If IsPointInsideLine(newMouseLocation, orgPoint, destPoint, WIRE_WIDTH) Then
                    wire.IsSelected = Not wire.IsSelected
                End If
            Else
                wire.IsSelected = IsPointInsideLine(newMouseLocation, orgPoint, destPoint, WIRE_WIDTH)
            End If

        Next

        ' CHECK SELECTED COMPONENTS

        Dim compIndex As Integer = 0
        For Each comp In Circuit.CircuitComponents

            ' Select components
            If IsControlKeyDown Then
                If IsPointInsideRectWithAngle(newMouseLocation, comp.Rect, comp.Angle) Then
                    comp.IsSelected = Not comp.IsSelected
                End If
            Else
                comp.IsSelected = IsPointInsideRectWithAngle(newMouseLocation, comp.Rect, comp.Angle)
            End If

            ' Check if new temp wire
            For pinIndex = 0 To comp.PinOut.Count - 1
                Dim point = comp.GetAbsolutePinLocation(pinIndex)
                Dim pinArea = New RectangleF(point.X - PIN_HEIGHT / 2, point.Y - PIN_HEIGHT / 2, PIN_HEIGHT, PIN_HEIGHT)
                If IsPointInsideRect(newMouseLocation, pinArea) Then
                    CircuitTemporalWireStartPoint = newMouseLocation
                    CircuitTemporalWire.OriginComponentID = compIndex
                    CircuitTemporalWire.OriginComponentPin = pinIndex
                End If
            Next

            compIndex += 1
        Next
        RaiseEvent SelectedComponentsChanged(Me, New EventArgs)
    End Sub
    Private Sub AddOrConfirmTemporalWire(mouse As Point)
        Dim newMouseLocation = GetRelativeMouseLocation(mouse)


        If CircuitTemporalWireStartPoint = Nothing Then Exit Sub


        Dim compIndex As Integer = 0
        For Each comp In Circuit.CircuitComponents
            ' Check if new temp wire
            For pinIndex = 0 To comp.PinOut.Count - 1
                Dim point = comp.GetAbsolutePinLocation(pinIndex)
                Dim pinArea = New RectangleF(point.X - PIN_HEIGHT / 2, point.Y - PIN_HEIGHT / 2, PIN_HEIGHT, PIN_HEIGHT)
                If IsPointInsideRect(newMouseLocation, pinArea) Then
                    CircuitTemporalWireStartPoint = newMouseLocation
                    CircuitTemporalWire.DestinationComponentID = compIndex
                    CircuitTemporalWire.DestinationComponentPin = pinIndex
                End If
            Next

            compIndex += 1
        Next

        CircuitTemporalWireStartPoint = Nothing
        CircuitTemporalWireEndPoint = Nothing

        Dim combination1 = CStr(CircuitTemporalWire.OriginComponentID) + "-" + CStr(CircuitTemporalWire.OriginComponentPin)
        Dim combination2 = CStr(CircuitTemporalWire.DestinationComponentID) + "-" + CStr(CircuitTemporalWire.DestinationComponentPin)

        If combination1 = combination2 Then Exit Sub

        If CircuitTemporalWire.OriginComponentID < 0 Or CircuitTemporalWire.DestinationComponentID < 0 Then Exit Sub
        If CircuitTemporalWire.OriginComponentPin < 0 Or CircuitTemporalWire.DestinationComponentPin < 0 Then Exit Sub
        If CircuitTemporalWire.OriginComponentID >= Circuit.CircuitComponents.Count Or CircuitTemporalWire.DestinationComponentID >= Circuit.CircuitComponents.Count Then Exit Sub
        If CircuitTemporalWire.OriginComponentID >= Circuit.CircuitComponents.Count Or CircuitTemporalWire.DestinationComponentID >= Circuit.CircuitComponents.Count Then Exit Sub

        Dim allWires As New List(Of String)
        For Each compWire In Circuit.CircuitConnections
            ' agregar las dos combinaciones: actual y reversa
            allWires.Add(CStr(compWire.OriginComponentID) + "-" + CStr(compWire.OriginComponentPin) + "_" + CStr(compWire.DestinationComponentID) + "-" + CStr(compWire.DestinationComponentPin))
            allWires.Add(CStr(compWire.DestinationComponentID) + "-" + CStr(compWire.DestinationComponentPin) + "_" + CStr(compWire.OriginComponentID) + "-" + CStr(compWire.OriginComponentPin))
        Next

        If Not allWires.Contains(combination1 + "_" + combination2) Then
            Circuit.CircuitConnections.Add(New EComponentConnection With {
                .OriginComponentID = CircuitTemporalWire.OriginComponentID,
                .DestinationComponentID = CircuitTemporalWire.DestinationComponentID,
                .OriginComponentPin = CircuitTemporalWire.OriginComponentPin,
                .DestinationComponentPin = CircuitTemporalWire.DestinationComponentPin,
                .Color = SystemColors.ControlDarkDark
            })
            RaiseEvent ComponentChangedVisually(Me, New EventArgs)
        End If


    End Sub
    Private Sub MoveCircuit(e As MouseEventArgs)
        ' Mover circuito completo
        Dim loc = Me.PointToScreen(e.Location)
        If loc.X = 0 Then
            Cursor.Position = New Point(My.Computer.Screen.Bounds.Width - 2, loc.Y)
            CircuitOrigin -= New Point(My.Computer.Screen.Bounds.Width, 0)
        ElseIf loc.X = My.Computer.Screen.Bounds.Width - 1 Then
            Cursor.Position = New Point(1, loc.Y)
            CircuitOrigin += New Point(My.Computer.Screen.Bounds.Width, 0)
        ElseIf loc.Y = 0 Then
            Cursor.Position = New Point(loc.X, My.Computer.Screen.Bounds.Height - 2)
            CircuitOrigin -= New Point(0, My.Computer.Screen.Bounds.Height)
        ElseIf loc.Y = My.Computer.Screen.Bounds.Height - 1 Then
            Cursor.Position = New Point(loc.X, 1)
            CircuitOrigin += New Point(0, My.Computer.Screen.Bounds.Height)
        End If

        CircuitOrigin = New Point(CircuitOrigin.X + (e.X - mouseOffset.X), CircuitOrigin.Y + (e.Y - mouseOffset.Y))
    End Sub
    Private Sub MoveSelectedComponents(e As MouseEventArgs)
        'If Not IsAnySelected() Then Exit Sub

        ' Mover componentes seleccionados
        For Each comp In Circuit.CircuitComponents
            If CircuitTemporalWireStartPoint <> Nothing Then
                CircuitTemporalWireEndPoint = GetRelativeMouseLocation(e.Location)
            ElseIf comp.IsSelected Then
                Dim rect As New RectangleF With {
                    .X = comp.Rect.X + (e.X - mouseOffset.X),
                    .Y = comp.Rect.Y + (e.Y - mouseOffset.Y),
                    .Width = comp.Rect.Width,
                    .Height = comp.Rect.Height
                }
                comp.Rect = rect
            End If
        Next
    End Sub
    Private Sub ApplyZoom(e As MouseEventArgs)

        Dim wheel = (e.Delta / 120) ' Utilizamos 120 en lugar de 1000 para obtener la cantidad de pasos de la rueda del mouse
        Dim zoomFactor = 1.1 ' Factor de zoom para acercarse o alejarse
        ' Calcular la posición relativa del mouse con respecto al centro del formulario
        Dim mouseX As Integer = e.X - Me.Width / 2
        Dim mouseY As Integer = e.Y - Me.Height / 2
        ' Aplicar el zoom
        Dim lastZoom As Double = CircuitZoom
        CircuitZoom *= Math.Pow(zoomFactor, wheel)
        CircuitZoom = Math.Min(Math.Max(CircuitZoom, 0.05), 10)
        ' Calcular el desplazamiento del origen relativo basado en el zoom y la posición del mouse
        If lastZoom <> CircuitZoom Then
            CircuitOrigin = New Point(CircuitOrigin.X - (mouseX * (1 - 1 / zoomFactor)), CircuitOrigin.Y - (mouseY * (1 - 1 / zoomFactor)))
        End If

    End Sub
    Private Sub RotateSeletedComponents(e As MouseEventArgs)
        If Not IsAnySelected() Then Exit Sub

        Dim wheel = -10
        If e.Delta > 0 Then
            wheel = 10
        End If

        For Each comp In Circuit.CircuitComponents
            If comp.IsSelected Then
                comp.Angle = comp.Angle + wheel
                If comp.Angle < 0 Then
                    comp.Angle += 360
                End If
                If comp.Angle >= 360 Then
                    comp.Angle -= 360
                End If
            End If
        Next

        RaiseEvent ComponentChangedVisually(Me, New EventArgs)
    End Sub



    Private mouseCaptured As Boolean = False
    Private mouseOffset As Point
    Private IsControlKeyDown As Boolean
    Private Sub ECircuitViewer_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        IsControlKeyDown = e.Control

        If e.KeyData = Keys.Delete Then
            RemoveSelectedComponentsAndWires()
            Me.Invalidate()
        End If

    End Sub
    Private Sub ECircuitViewer_KeyUp(sender As Object, e As KeyEventArgs) Handles MyBase.KeyUp
        IsControlKeyDown = e.Control
    End Sub

    Private LastMousePositionDetected As Point
    Private Sub ECircuitViewer_MouseDown(ByVal sender As Object, ByVal e As MouseEventArgs) Handles MyBase.MouseDown
        Me.Cursor = Cursors.SizeAll
        mouseCaptured = True
        mouseOffset = e.Location

        If (e.Button = MouseButtons.Left Or e.Button = MouseButtons.Right) And CircuitViewMode = ViewerMode.Edit Then
            CheckSeletedLinesAndComponents(e.Location)
        End If

        Me.Invalidate()
    End Sub
    Private Sub ECircuitViewer_MouseUp(sender As Object, e As MouseEventArgs) Handles MyBase.MouseUp
        AddOrConfirmTemporalWire(e.Location)
        mouseCaptured = False
        Me.Cursor = Cursors.Default
        If (e.Button = MouseButtons.Left And CircuitViewMode = ViewerMode.Edit And IsAnySelected()) Then
            RaiseEvent ComponentChangedVisually(Me, New EventArgs)
        End If


        Me.Invalidate()
    End Sub
    Private Sub ECircuitViewer_MouseMove(sender As Object, e As MouseEventArgs) Handles MyBase.MouseMove
        LastMousePositionDetected = e.Location
        If (mouseCaptured) Then

            If (e.Button = MouseButtons.Middle And CircuitViewMode = ViewerMode.Edit) Or (CircuitViewMode = ViewerMode.Zoom) Then
                MoveCircuit(e)
            ElseIf (e.Button = MouseButtons.Left And CircuitViewMode = ViewerMode.Edit) Then
                MoveSelectedComponents(e)
            End If

            mouseOffset = e.Location
            Me.Invalidate()
        End If
    End Sub
    Private Sub ECircuitViewer_MouseWheel(sender As Object, e As MouseEventArgs) Handles MyBase.MouseWheel
        If (IsControlKeyDown And CircuitViewMode = ViewerMode.Edit) Or (CircuitViewMode = ViewerMode.Zoom) Then
            ApplyZoom(e)
            Me.Invalidate()

        ElseIf CircuitViewMode = ViewerMode.Edit Then
            RotateSeletedComponents(e)
            Me.Invalidate()

        End If
    End Sub



    Private Sub ECircuitViewer_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.DoubleBuffered = True
    End Sub
    Private Sub ECircuitViewer_Resize(sender As Object, e As EventArgs) Handles MyBase.Resize
        Me.Invalidate()
    End Sub
    Private Sub ECircuitViewer_Paint(sender As Object, e As PaintEventArgs) Handles MyBase.Paint
        If DesignMode Then
            DrawTable(e.Graphics, CircuitGridColor, 0, 0, Me.Width, Me.Height, CircuitGridSize, CircuitGridSize)
        End If


        If Circuit IsNot Nothing Then
            Me.UseWaitCursor = True

            ' Background
            Dim tableSize As Integer = Math.Max(Me.Width, Me.Height)
            Dim tableWidth As Single = (tableSize * 2) / CircuitGridSize
            Dim tableX As Single = ((-tableWidth * CircuitGridSize) / 2)
            DrawTable(e.Graphics, CircuitGridColor, CircuitOrigin.X + tableX, CircuitOrigin.Y + tableX, tableWidth, tableWidth, CircuitGridSize, CircuitGridSize)

            ' Zoom and Origin
            With e.Graphics
                .TextRenderingHint = Drawing.Text.TextRenderingHint.ClearTypeGridFit
                .SmoothingMode = Drawing2D.SmoothingMode.AntiAlias
                .TranslateTransform(CircuitOrigin.X, CircuitOrigin.Y)
                .ScaleTransform(CircuitZoom, CircuitZoom)
            End With


            ' Circuit
            Circuit.Draw(e.Graphics)

            ' TemporalWire
            If CircuitTemporalWireStartPoint <> Nothing AndAlso CircuitTemporalWireEndPoint <> Nothing Then
                Using WirePen As New Pen(CircuitTemporalWireColor, 4)
                    WirePen.StartCap = Drawing2D.LineCap.Round
                    WirePen.EndCap = Drawing2D.LineCap.Round
                    e.Graphics.DrawLine(WirePen, CircuitTemporalWireStartPoint, CircuitTemporalWireEndPoint)
                End Using
            End If

            Me.UseWaitCursor = False
        End If
    End Sub


    ' DROPDOWN
    Private Sub EliminarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BtnDelete.Click
        RemoveSelectedComponentsAndWires()
        Me.Invalidate()
    End Sub
    Private Sub DuplicarToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BtnDuplicate.Click
        DuplicateSelectedComponents()
        Me.Invalidate()
    End Sub
    Private Sub SelectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BtnSelectAll.Click
        For Each comp In Circuit.CircuitComponents
            comp.IsSelected = True
        Next
        For Each conn In Circuit.CircuitConnections
            conn.IsSelected = True
        Next
        Me.Invalidate()
    End Sub
    Private Sub UnselectAllToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles BtnUnselectAll.Click
        For Each comp In Circuit.CircuitComponents
            comp.IsSelected = False
        Next
        For Each conn In Circuit.CircuitConnections
            conn.IsSelected = False
        Next
        Me.Invalidate()
    End Sub
    Private Sub CircuitContextMenu_Opening(sender As Object, e As CancelEventArgs) Handles CircuitContextMenu.Opening
        Dim AnySelected = IsAnySelected()
        Dim AnyUnselected = IsAnyUnselected()

        BtnDuplicate.Visible = AnySelected
        BtnDelete.Visible = AnySelected
        Separator1.Visible = AnySelected
        BtnSelectAll.Visible = AnyUnselected
        BtnUnselectAll.Visible = AnySelected

        e.Cancel = (AnySelected = False) And (AnyUnselected = False)

    End Sub

End Class

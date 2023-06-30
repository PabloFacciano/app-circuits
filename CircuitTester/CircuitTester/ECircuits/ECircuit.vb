Imports System.Drawing.Drawing2D
Imports CircuitTester.ECircuits
Imports CircuitTester.ECircuits.EComponents
Imports System.ComponentModel
Imports System.Collections.ObjectModel

Namespace ECircuits
    <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    <EditorBrowsable(EditorBrowsableState.Always)>
    Public Class ECircuit
        Implements ICloneable

        <TypeConverterAttribute(GetType(System.ComponentModel.ExpandableObjectConverter))>
        <EditorBrowsable(EditorBrowsableState.Always)>
        <Category("Circuit")>
        Public Property CircuitComponents As List(Of EComponent)

        <TypeConverterAttribute(GetType(System.ComponentModel.ExpandableObjectConverter))>
        <EditorBrowsable(EditorBrowsableState.Always)>
        <Category("Circuit")>
        Public Property CircuitConnections As List(Of EComponentConnection)

        <Category("Circuit")>
        Public Property LastModifiedDate As Date

        Public Shared Sub InitSample(ByRef Circuit As ECircuit)
            ' Content
            Circuit.CircuitComponents = New List(Of EComponent)
            Circuit.CircuitConnections = New List(Of EComponentConnection)

            ' 1. Battery + Led
            Circuit.CircuitComponents.Add(New Battery With {
                .Name = "Battery 1",
                .Rect = New RectangleF(-180, -80, 120, 70),
                .Angle = 90
            })
            Circuit.CircuitComponents.Add(New LED With {
                .Name = "LED 1",
                .Rect = New RectangleF(-50, -50, 35, 35),
                .Angle = 130
            })

            Dim conn1 As New EComponentConnection
            With conn1
                .OriginComponentID = 0
                .OriginComponentPin = 0
                .DestinationComponentID = 1
                .DestinationComponentPin = 0
                .Color = Color.DarkRed
            End With
            Circuit.CircuitConnections.Add(conn1)

            Dim conn2 As New EComponentConnection
            With conn2
                .OriginComponentID = 1
                .OriginComponentPin = 1
                .DestinationComponentID = 0
                .DestinationComponentPin = 1
                .Color = Color.FromArgb(30, 30, 30)
            End With
            Circuit.CircuitConnections.Add(conn2)



            ' 2. Battery -> Resistor -> Led
            ' TO-DO


        End Sub

        Public Sub Init()
            For Each Component In CircuitComponents
                Component.Init()
            Next
        End Sub
        Public Sub RunStep()
            Dim componentsRan As New List(Of Integer)

            ' implementacion openai
            ' Iterar sobre las conexiones del circuito
            For Each Wire As EComponentConnection In CircuitConnections
                ' Obtener los componentes de origen y destino de la conexión
                Dim originComponent As EComponent = CircuitComponents(Wire.OriginComponentID)
                Dim destinationComponent As EComponent = CircuitComponents(Wire.DestinationComponentID)

                ' Ejecuta el componente origen
                If Not componentsRan.Contains(Wire.OriginComponentID) Then
                    originComponent.Run()
                    componentsRan.Add(Wire.OriginComponentID)
                End If

                ' Ejecutar el componente de destino
                ' Actualizar los valores de voltaje, amperaje y resistencia en la conexión  
                destinationComponent.PinOut(Wire.DestinationComponentPin).Voltage = originComponent.PinOut(Wire.OriginComponentPin).Voltage
                destinationComponent.PinOut(Wire.DestinationComponentPin).Amperage = originComponent.PinOut(Wire.OriginComponentPin).Amperage
                destinationComponent.PinOut(Wire.DestinationComponentPin).Resistance = originComponent.PinOut(Wire.OriginComponentPin).Resistance

                destinationComponent.Run()

            Next
            componentsRan = Nothing
        End Sub

        Public Sub Draw(ByRef g As Graphics)

            ' Componentes
            For Each C As EComponent In CircuitComponents
                C.Draw(g)
            Next

            ' Cables
            For Each P As EComponentConnection In CircuitConnections
                If P.OriginComponentID < 0 Or P.DestinationComponentID < 0 Then Continue For
                If P.OriginComponentPin < 0 Or P.DestinationComponentPin < 0 Then Continue For
                If P.OriginComponentID >= CircuitComponents.Count Or P.DestinationComponentID >= CircuitComponents.Count Then Continue For
                If P.OriginComponentID >= CircuitComponents.Count Or P.DestinationComponentID >= CircuitComponents.Count Then Continue For

                Dim c1, c2 As EComponent
                c1 = CircuitComponents(P.OriginComponentID)
                c2 = CircuitComponents(P.DestinationComponentID)


                Dim p1 As PointF = c1.GetAbsolutePinLocation(P.OriginComponentPin)
                Dim p2 As PointF = c2.GetAbsolutePinLocation(P.DestinationComponentPin)


                Using WirePen As New Pen(Color.FromArgb(128, P.Color), WIRE_WIDTH * 2)
                    WirePen.StartCap = Drawing2D.LineCap.Round
                    WirePen.EndCap = Drawing2D.LineCap.Round

                    ' Dibujar la línea de seleccion
                    If P.IsSelected Then
                        g.DrawLine(WirePen, p1, p2)
                    End If

                    ' Dibujar la línea sin patrón (sólida)
                    WirePen.Color = Color.FromArgb(192, P.Color)
                    WirePen.Width = WIRE_WIDTH
                    g.DrawLine(WirePen, p1, p2)

                    WirePen.Color = P.Color

                    WirePen.StartCap = Drawing2D.LineCap.Flat
                    WirePen.EndCap = Drawing2D.LineCap.Triangle

                    Dim segmentLength As Integer = 5 ' Longitud de cada segmento de línea o espacio

                    ' Obtener la longitud total de la línea
                    Dim lineLength As Single = Math.Sqrt((p2.X - p1.X) ^ 2 + (p2.Y - p1.Y) ^ 2)

                    ' Calcular el número de segmentos necesarios
                    Dim numSegments As Integer = CInt(lineLength / segmentLength)

                    ' Crear un objeto de cap personalizado para la flecha invertida
                    Dim customPath As New GraphicsPath()
                    customPath.AddLine(0F, 0F, 1.0F, 1.0F)
                    customPath.AddLine(0F, 0F, -1.0F, 1.0F)
                    Dim customCap As New CustomLineCap(Nothing, customPath, LineCap.Round)

                    WirePen.StartCap = LineCap.Custom
                    WirePen.CustomStartCap = customCap
                    WirePen.DashPattern = New Single() {0.01F, 10.0F}
                    WirePen.EndCap = LineCap.NoAnchor

                    ' Dibujar la línea con el patrón de barras
                    WirePen.Width = 1.5
                    For i As Integer = 0 To numSegments - 1
                        g.DrawLine(WirePen, GetPointAlongLine(p1, p2, i * segmentLength), GetPointAlongLine(p1, p2, (i + 1) * segmentLength))
                        If i Mod 2 = 0 Then
                            ' Segmento de línea
                        Else
                            ' Espacio (sin dibujar)
                        End If
                    Next


                End Using

            Next

        End Sub
        ' Calcular un punto a lo largo de una línea dada una distancia desde el punto de inicio
        Private Function GetPointAlongLine(ByVal startPoint As PointF, ByVal endPoint As PointF, ByVal distance As Single) As PointF
            Dim dx As Single = endPoint.X - startPoint.X
            Dim dy As Single = endPoint.Y - startPoint.Y
            Dim length As Single = Math.Sqrt(dx * dx + dy * dy)
            Dim t As Single = distance / length

            Dim newX As Single = startPoint.X + dx * t
            Dim newY As Single = startPoint.Y + dy * t

            Return New PointF(newX, newY)
        End Function


        Public Function AllPowerSourceIsDead() As Boolean
            For Each comp In CircuitComponents

                If TypeOf comp Is EPowerSource Then
                    If CType(comp, EPowerSource).HasPower() Then
                        Return False
                    End If
                End If


            Next
            Return True
        End Function
        Public Function Clone() As Object Implements ICloneable.Clone
            Dim CI As New ECircuit
            CI.CircuitComponents = New List(Of EComponent)
            For Each c In Me.CircuitComponents
                Dim cc = c.Clone()
                CI.CircuitComponents.Add(cc)
            Next
            CI.CircuitConnections = New List(Of EComponentConnection)
            For Each c In Me.CircuitConnections
                Dim cc = c.Clone()
                CI.CircuitConnections.Add(cc)
            Next

            Return CI
        End Function

    End Class
End Namespace
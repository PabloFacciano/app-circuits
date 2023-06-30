Imports System.ComponentModel
Imports CircuitTester.ECircuits.EComponents

Namespace ECircuits
    <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    <EditorBrowsable(EditorBrowsableState.Always)>
    Public Class EComponentPinout
        Implements ICloneable
        Public Property PinLabel As String

        ' Current Electricity
        <Category("Electricity")>
        Public Property Voltage As Double
        <Category("Electricity")>
        Public Property Amperage As Double
        <Category("Electricity")>
        Public Property Resistance As Double ' Wire resistance: not used yet

        '' Min y max de electricidad para el funcionamiento
        'Public Property MinVolts As Double
        'Public Property MaxVolts As Double
        'Public Property MinAmp As Double
        'Public Property MaxAmp As Double

        '' Entrada y Salida de electricidad (volts + ampers)
        'Public Property InVolts As Double
        'Public Property InAmps As Double
        'Public Property OutVolts As Double
        'Public Property OutAmps As Double

        Public Function Clone() As Object Implements ICloneable.Clone
            Dim CI As EComponentPinout = DirectCast(MemberwiseClone(), EComponentPinout)
            Return CI
        End Function
    End Class
End Namespace
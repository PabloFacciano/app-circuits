Imports System.ComponentModel

Namespace ECircuits
    <TypeConverter(GetType(System.ComponentModel.ExpandableObjectConverter))>
    <EditorBrowsable(EditorBrowsableState.Always)>
    Public Class EComponentConnection
        Implements ICloneable

        <Category("Origin")>
        Public Property OriginComponentID As Integer = -1
        <Category("Origin")>
        Public Property OriginComponentPin As Integer = -1
        <Category("Destination")>
        Public Property DestinationComponentID As Integer = -1
        <Category("Destination")>
        Public Property DestinationComponentPin As Integer = -1
        <Category("Wire")>
        Public Property IsSelected As Boolean
        Public Property Color As Color
        Public Function Clone() As Object Implements ICloneable.Clone
            Return DirectCast(MemberwiseClone(), EComponentConnection)
        End Function
    End Class
End Namespace

'Namespace ECircuits
'    Public Structure EComponentConnection
'        Public OriginComponentID As Integer
'        Public OriginComponentPin As Integer
'        Public DestinationComponentID As Integer
'        Public DestinationComponentPin As Integer
'        Public IsSelected As Boolean
'        Public Color As Color
'    End Structure
'End Namespace

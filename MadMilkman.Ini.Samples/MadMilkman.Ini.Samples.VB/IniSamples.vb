﻿Imports System
Imports System.IO
Imports System.Text
Imports System.Collections.Generic
Imports MadMilkman.Ini

Module IniSamples

    Private Sub HelloWorld()
        ' Create new file.
        Dim file As New IniFile()

        ' Add new section.
        Dim section As IniSection = file.Sections.Add("Section Name")

        ' Add new key and its value.
        Dim key As IniKey = section.Keys.Add("Key Name", "Hello World")

        ' Read file's specific value.
        Console.WriteLine(file.Sections("Section Name").Keys("Key Name").Value)
    End Sub

    Private Sub Create()
        ' Create new file with default formatting.
        Dim file As New IniFile(New IniOptions())

        ' Add new content.
        Dim section As New IniSection(file, IniSection.GlobalSectionName)
        Dim key As New IniKey(file, "Key 1", "Value 1")
        file.Sections.Add(section)
        section.Keys.Add(key)

        ' Add new content.
        file.Sections.Add("Section 2").Keys.Add("Key 2", "Value 2")

        ' Add new content.
        file.Sections.Add(
            New IniSection(file, "Section 3",
                New IniKey(file, "Key 3.1", "Value 3.1"),
                New IniKey(file, "Key 3.2", "Value 3.2")))

        ' Add new content.
        file.Sections.Add(
            New IniSection(file, "Section 4",
                New Dictionary(Of String, String)() From {
                    {"Key 4.1", "Value 4.1"},
                    {"Key 4.2", "Value 4.2"}
                }))
    End Sub

    Private Sub Load()
        Dim options As New IniOptions()
        Dim iniFile As New IniFile(options)

        ' Load file from path.
        iniFile.Load("..\..\..\MadMilkman.Ini.Samples.Files\Load Example.ini")

        ' Load file from stream.
        Using stream As Stream = File.OpenRead("..\..\..\MadMilkman.Ini.Samples.Files\Load Example.ini")
            iniFile.Load(stream)
        End Using

        ' Load file's content from string.
        Dim iniContent As String = "[Section 1]" + Environment.NewLine +
                                   "Key 1.1 = Value 1.1" + Environment.NewLine +
                                   "Key 1.2 = Value 1.2" + Environment.NewLine +
                                   "Key 1.3 = Value 1.3" + Environment.NewLine +
                                   "Key 1.4 = Value 1.4"
        Using stream As Stream = New MemoryStream(options.Encoding.GetBytes(iniContent))
            iniFile.Load(stream)
        End Using

        ' Read file's content.
        For Each section In iniFile.Sections
            Console.WriteLine("SECTION: {0}", section.Name)
            For Each key In section.Keys
                Console.WriteLine("KEY: {0}, VALUE: {1}", key.Name, key.Value)
            Next
        Next
    End Sub

    Private Sub Style()
        Dim file As New IniFile()
        file.Sections.Add("Section 1").Keys.Add("Key 1", "Value 1")
        file.Sections.Add("Section 2").Keys.Add("Key 2", "Value 2")
        file.Sections.Add("Section 3").Keys.Add("Key 3", "Value 3")

        ' Add leading comments.
        file.Sections(0).LeadingComment.Text = "Section 1 leading comment."
        file.Sections(0).Keys(0).LeadingComment.Text = "Key 1 leading comment."

        ' Add trailing comments.
        file.Sections(1).TrailingComment.Text = "Section 2 trailing comment."
        file.Sections(1).Keys(0).TrailingComment.Text = "Key 2 trailing comment."

        ' Add left space, indentation.
        file.Sections(1).LeftIndentation = 4
        file.Sections(1).TrailingComment.LeftIndentation = 4
        file.Sections(1).Keys(0).LeftIndentation = 4
        file.Sections(1).Keys(0).TrailingComment.LeftIndentation = 4

        ' Add above space, empty lines.
        file.Sections(2).TrailingComment.EmptyLinesBefore = 2
    End Sub

    Private Sub Save()
        Dim options As New IniOptions()
        Dim iniFile As New IniFile(options)
        iniFile.Sections.Add(
            New IniSection(iniFile, "Section 1",
                New IniKey(iniFile, "Key 1.1", "Value 1.1"),
                New IniKey(iniFile, "Key 1.2", "Value 1.2"),
                New IniKey(iniFile, "Key 1.3", "Value 1.3"),
                New IniKey(iniFile, "Key 1.4", "Value 1.4")))

        ' Save file to path.
        iniFile.Save("..\..\..\MadMilkman.Ini.Samples.Files\Save Example.ini")

        ' Save file to stream.
        Using stream As Stream = File.Create("..\..\..\MadMilkman.Ini.Samples.Files\Save Example.ini")
            iniFile.Save(stream)
        End Using

        ' Save file's content to string.
        Dim iniContent As String
        Using stream As Stream = New MemoryStream()
            iniFile.Save(stream)
            iniContent = New StreamReader(stream, options.Encoding).ReadToEnd()
        End Using

        Console.WriteLine(iniContent)
    End Sub

    Private Sub Custom()
        ' Create new file with custom formatting.
        Dim file As New IniFile(
                        New IniOptions() With {
                            .CommentStarter = IniCommentStarter.Hash,
                            .KeyDelimiter = IniKeyDelimiter.Colon,
                            .KeySpaceAroundDelimiter = True,
                            .SectionWrapper = IniSectionWrapper.CurlyBrackets,
                            .Encoding = Encoding.UTF8
                        })

        ' Load file.
        file.Load("..\..\..\MadMilkman.Ini.Samples.Files\Custom Example Input.ini")

        ' Change first section's fourth key's value.
        file.Sections(0).Keys(3).Value = "NEW VALUE"

        ' Save file.
        file.Save("..\..\..\MadMilkman.Ini.Samples.Files\Custom Example Output.ini")
    End Sub

    Private Sub Copy()
        ' Create new file.
        Dim file As New IniFile()

        ' Add new content.
        Dim section As IniSection = file.Sections.Add("Section")
        Dim key As IniKey = section.Keys.Add("Key")

        ' Add duplicate section.
        file.Sections.Add(section.Copy())

        ' Add duplicate key.
        section.Keys.Add(key.Copy())

        ' Create new file.
        Dim newFile As New IniFile(New IniOptions())

        ' Import first file's section to second file.
        newFile.Sections.Add(section.Copy(newFile))
    End Sub

    Private Sub Parse()
        Dim file As New IniFile()
        Dim content As String = "[Highest Score]" + Environment.NewLine +
                                "Name = John Doe" + Environment.NewLine +
                                "Score = 3200000" + Environment.NewLine +
                                "Date = 12/31/2010" + Environment.NewLine +
                                "Time = 11:59:59"
        Using stream As Stream = New MemoryStream(Encoding.ASCII.GetBytes(content))
            file.Load(stream)
        End Using

        Dim scoreSection As IniSection = file.Sections("Highest Score")

        Dim playerName As String = scoreSection.Keys("Name").Value

        ' Retrieve key's value as long.
        Dim playerScore As Long
        scoreSection.Keys("Score").TryParseValue(playerScore)

        ' Retrieve key's value as DateTime.
        Dim scoreDate As DateTime
        scoreSection.Keys("Date").TryParseValue(scoreDate)

        ' Retrieve key's value as TimeSpan.
        Dim gameTime As TimeSpan
        scoreSection.Keys("Time").TryParseValue(gameTime)
    End Sub

    Sub Main()
        HelloWorld()

        Create()

        Load()

        Style()

        Save()

        Custom()

        Copy()

        Parse()
    End Sub

End Module
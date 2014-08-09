CodeUnit Project Update Folder Preparation
==========================================

Over the years I've developed a pretty rigid and standard file and folder layout for my various CodeUnit projects. As a quick overview, the projects are all situated in a base 'Work Projects' folder. The completed projects sit in a 'Completed Projects' folder and each project has the following structure: 

Each project sits in a folder with the Project Name suffixed with its original date of creation - e.g. 'Bitmap to Jpeg - 30 October 2006'. In this folder we have a whole lot of descriptive files regarding the project and then two folders, the 'Release' folder housing the final generated installer files and the 'Source Code' folder which holds both the actual project directory and the project installer directory. 

Everytime I wish to update a project I first make a copy of the existing source code folder, using this naming convention: Bitmap to Jpeg Version 20080110.01 where 20080110.01 is the build number of that particular version of the source code.

Other than the backup folder creation, I also need to modify the Build.txt file located in the project home directory, remove the installer files from the 'Release' folder and also make an updated entry in the Description.txt file.

CodeUnit Project Update Folder Preparation is a quick and dirty application that simply automates this tedious task for me.

Created by Craig Lotter, January 2008

Note: Created for CodeUnit Projects

*********************************

Project Details:

Coded in Visual Basic .NET using Visual Studio .NET 2008
Implements concepts such as Folder manipulation, File manipulation.
Level of Complexity: Very Simple

*********************************

Update 20080121.02:

- Fixed Project Selection dialog display errors

# CB Output EDS Internal

CB Output EDS Internal is a .NET Framework task application designed to automate the process of unzipping and processing files. It is intended to be run as a scheduled task.

## Description

This application monitors a specified directory for new `.zip` files. When a new zip file is found, the application unzips it to a designated wafer mapping directory. It handles both new and existing folders, updates relevant data files, and maintains a log of its operations. After successful processing, the original zip file is renamed with a `.bak` extension to prevent it from being processed again.

## Features

  * **Automated Unzipping**: Automatically extracts `.zip` files from a source directory.
  * **Dynamic Folder Creation**: Creates destination folders based on the name of the zip file.
  * **Update Handling**: For existing folders, it updates the contents with the new files from the zip archive.
  * **Binary File Modification**: After unzipping, it modifies a `LOT.DAT` binary file, updating specific byte offsets based on the `.DAT` files found in the unzipped archive.
  * **Logging**: Records successes and errors to a log file, including timestamps and detailed error messages.
  * **File Archiving**: Renames processed `.zip` files to `.zip.bak` to mark them as completed.

## Workflow

1.  A Chipbank Import process places a `.zip` file into the `\\172.16.0.3\CBOutput\BACKUP` directory.
2.  A scheduled task runs the `CBUnZip.exe` application.
3.  The application finds the `.zip` file and unzips its contents to the `\\172.16.0.115\WaferMapping` directory.

## Configuration

Before running the application, you need to configure the paths in the `App.config` file.

```xml
<configuration>
	<appSettings>
		<add key="PathCBOutput" value="C:\Users\user446\Desktop\CBOutput\BACKUP"/>
		<add key="PathWaferMapping" value="C:\Users\user446\Desktop\WaferMapping\"/>
		<add key="Logfile" value="C:\Users\user446\Desktop\log\LogFile.txt"/>
	</appSettings>
</configuration>
```

  * **`PathCBOutput`**: The source directory where the application looks for `.zip` files.
  * **`PathWaferMapping`**: The destination directory where the files will be unzipped.
  * **`Logfile`**: The full path, including the filename, for the log file.

## Installation and Setup

1.  Clone or download the project.
2.  Open the solution in Visual Studio.
3.  Update the `App.config` file with the correct paths for your environment.
4.  Build the solution to generate the `CBUnZip.exe` executable.
5.  Create a task in Windows Task Scheduler to run the `CBUnZip.exe` executable at your desired interval.

## How It Works

The application's logic is contained within the `Program.cs` file.

1.  **Get Zip Files**: The program starts by getting a list of all files with a `.zip` extension from the `PathCBOutput` directory specified in `App.config`.

2.  **Process Each Zip File**: It iterates through each zip file and performs the following steps:

      * **Check for Existing Directory**: It checks if a directory with the same name as the zip file (without the extension) already exists in the `PathWaferMapping` directory.
      * **New Directory**: If the directory does not exist, it creates the directory and extracts the contents of the zip file into it.
      * **Existing Directory**: If the directory already exists, it opens the zip archive and extracts each entry, overwriting any existing files.

3.  **Update `LOT.DAT`**: After extracting the files, if the destination folder already existed, the application looks for `W-NO-*.DAT` files. It then opens the `LOT.DAT` file and writes data to specific byte offsets based on the number and names of the `.DAT` files found. It also converts the lot name to ASCII and writes it to the beginning of the `LOT.DAT` file.

4.  **Logging**: A log entry is created in the file specified by `Logfile` in `App.config` for both successful unzips and any errors that occur during the process.

5.  **Rename Processed File**: After a zip file has been processed successfully, it is renamed to `[original_name].zip.bak`. If a `.bak` file with the same name already exists, the new file is named with a timestamp to ensure uniqueness.

# Asset Deprecation Marker

This package renders strike through on deprecated asset files.

In a project under development, there are often assets that become obsolete and need to be removed from time to time. However, removing a file without breaking the project can be challenging. To ensure productivity, it is common practice to mark assets as deprecated first and then schedule a dedicated time to clean up all the dependencies to the removed assets.

Instead of moving deprecated asset files to a specific location, which can cause issues with version control systems, a better approach is to add a tag to the file. This package utilizes Unity's asset label feature and adds a "Deprecated" label to a deprecated file or folder, making it easier for users to view and search for them.

## Features

+ Display strike through on assets labeled as "Deprecated" in the Project window.
+ Display strike through on deprecated prefab instances in the Hierarchy window.
+ Enable nested deprecation to display assets under any deprecated folders as deprecated.

## How To Use

### Mark an Asset

To mark an asset as deprecated or clear the flag, right click the asset and select `Asset Deprecate Marker/Deprecate`.

### Find Deprecated Assets

To find all deprecated assets, search for `l:Deprecated` in the Project window.

Alternatively, all deprecated files can be listed in the settings window.

### Settings

To change the settings, go to `Edit/Project Settings...` and navigate to `Packages/Asset Deprecation Marker`. In this settings window, you can enable Nested Deprecation.


paketeditor
===========

Paket editor

TODO
====
UI
--
- [ ] Panel BAS: add a button for loading an installer
		+ Initially:
			- MSI
		+ Later:
			- NSIS
			- InstallShield
- [X] Panel InstallerTree: a TreeView of available IInstallers (for now only MSI) starting from a specified Directory


Bugs
----

- [] Everytime I drag and drop an item from the Treeview into the Edit window it automatically drop to the top of the window/.bas file  

- [] the green plus symbol and the red X symbol flip through to inst.bas and deinst.bas. The panes and the bottom named sample and Bas are suppose to do that 

- [] What is the purpose of installers pane at the buttom of the treeview pane.

Backend
-------
- [X] IInstaller interface to be implemented for each type of installer, with the following info's
		+ Name
		+ Version
		+ Date Created
		+ Creator
		+ Company

- [X] MSIInstaller class using:
		+ System.Configuration.Install.dll
		+ msi.dll

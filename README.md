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
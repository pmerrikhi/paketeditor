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
			

Backend
-------
- [ ] IInstaller interface to be implemented for each type of installer, with the following info's
		+ Name
		+ Version
		+ Date Created
		+ Creator
		+ Company

- [ ] MSIInstaller class using:
		+ System.Configuration.Install.dll
		+ msi.dll
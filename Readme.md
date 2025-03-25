

![alt text](/screenshot.JPG)



The application as it is provides safe and efficient directory browsing and file fetching from an external source.
It also comes with built-in user management features, role management and enables setting custom access rules for individual files. 

For future development I'd like to add:

- Multimedia features e.g. embedded Image and video viewing, maybe thumbnails ✓

- Multiple remote sources ✓

- Option to upload files ✓

- Option to share specific files to non authenticated users / entitys

- Maybe later adding Nextcloud-like features  



To Initialize the Database:

>dotnet tool install --global dotnet-ef

>dotnet ef migrations add Initial

>dotnet ef database update



Q&A 

Q Will it be possible to delete or move files on the set locations?

A As the main focus is security, it might become available on specific locations.
